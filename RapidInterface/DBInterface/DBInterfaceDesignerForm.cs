using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.ObjectModel;
using DevExpress.XtraTreeList;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraSplashScreen;
using System.ComponentModel.Design;
using System.Collections;
using System.IO;
using System.Drawing.Imaging;
using RapidInterface.Classes;


namespace RapidInterface
{
    public partial class DBInterfaceDesignerForm : Form
    {
        #region Contructors
        public DBInterfaceDesignerForm(DBInterface dbInterface)
        {
            InitializeComponent();
            DBInterface = dbInterface;
            TypeCurrent = typeof(string);
            drbtnAddColletion.Text = string.Format("Добавить (String)");

            MemberInits = new TableMemberInfos();
            MemberExists = new TableMemberInfos();
            LevelMax = 6;
            RecordCountMax = 100000;

            treeExist.StateImageList = DBInterface.Icons;
            
            TypeTables = new XPTables();
            TypeTables.FillTable(DBInterface.TypeDiscoveryService, typeof(XPBaseObject));

            UpdateExistData();

            CreateInitData();       

            treeInit.DataSource = MemberInits;
            treeExist.DataSource = MemberExists;
            dbInterface.ItemsChanged += dbInterface_ItemsChanged;

            gluInits.Properties.DataSource = TypeTables;

            gluInits.EditValue = TypeTables.Find(DBInterface.TableType);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Основной компонент.
        /// </summary>
        DBInterface DBInterface { get; set; }

        /// <summary>
        /// Поля таблицы, сгенированные по описанию класса таблицы.
        /// </summary>
        TableMemberInfos MemberInits { get; set; }

        /// <summary>
        /// Поля таблицы, заданные для компонента.
        /// </summary>
        TableMemberInfos MemberExists { get; set; }

        /// <summary>
        /// Текущий тип создаваемых записей таблицы.
        /// </summary>
        Type TypeCurrent { get; set; }

        /// <summary>
        /// Максимальное допустимое количество записей в исходном дереве.
        /// </summary>
        int RecordCountMax { get; set; }

        /// <summary>
        /// Максимальное уровень вложений записей в исходном дереве.
        /// </summary>
        int LevelMax {get; set;}

        /// <summary>
        /// Текущее выделенная запись среди существующих в списке.
        /// </summary>
        TableMemberInfo SelectedExistInfo { get; set; }

        /// <summary>
        /// Список возможных типов таблицы.
        /// </summary>
        XPTables TypeTables { get; set; }

        #endregion

        #region Metods Common
        /// <summary>
        /// Анализ PropertyInfoEx объекта и добавление полученных данных в объект TableMemberInfo.
        /// </summary>
        public TableMemberInfo AnalisProperty(PropertyInfoEx propertyInfo, TableMemberInfo tableInfo, ref int recordCount)
        {
            TableMemberInfo result = new TableMemberInfo();

            result.FieldName = propertyInfo.PropertyInfo.Name;
            result.Caption = PropertyInfoEx.GetDisplayNameAttribute(propertyInfo.PropertyInfo);
            result.PropertyType = propertyInfo.PropertyInfo.PropertyType;
            if (propertyInfo.IsXPCollection)
                result.PropertyTypeCollection = propertyInfo.PropertyInfoCollection.PropertyType;
            result.IsXPBaseObject = propertyInfo.IsXPBaseObject;
            result.IsXPCollection = propertyInfo.IsXPCollection;

            if (result.IsXPBaseObject || result.IsXPCollection)
            {
                string name = "";
                if (result.IsXPBaseObject)
                    name = DBAttribute.GetIconFile(result.PropertyType);
                if (result.IsXPCollection)
                    name = DBAttribute.GetIconFile(result.PropertyTypeCollection);

                string dir = DBInterface.GetImageFullName(name);
                if (dir != "" && File.Exists(dir) && !ImageEx.IsExist(imgInit, name))
                    imgInit.AddImage(Image.FromFile(dir), name);

                result.ImageIndex = ImageEx.GetImageIndex(imgInit, name);
                result.ImageName = name;
            }

            if (tableInfo != null)
                tableInfo.Add(result);

            if (MemberExists.FindInfo(result) != null)
                result.IsUsed = true;

            // Ограничение по количеству записей 
            recordCount++;
            if (recordCount >= RecordCountMax)
                return null;
            else
                return result;
        }

        /// <summary>
        /// Получения списка свойст определенного типа.
        /// </summary>
        public PropertyInfoExs GetProperties(PropertyInfoEx info, Type type = null)
        {
            Type typeIn;

            if (type == null)
                if (info.IsXPCollection && info.PropertyInfoCollection != null)
                    typeIn = info.PropertyInfoCollection.PropertyType;
                else
                    typeIn = info.PropertyInfo.PropertyType;
            else
                typeIn = type;

            PropertyInfoExs result = new PropertyInfoExs(info);
            if (PropertyInfoEx.isXPComplex(typeIn))
            {
                PropertyInfo[] prInfos = typeIn.GetProperties();

                foreach (PropertyInfo prInfoIn in prInfos)
                    if (prInfoIn.Name != "Loading" && prInfoIn.Name != "IsLoading" && prInfoIn.Name != "IsDeleted" &&
                        (PropertyInfoEx.isXPComplex(prInfoIn.PropertyType) ||
                        prInfoIn.PropertyType == typeof(bool) ||
                        prInfoIn.PropertyType == typeof(short) ||
                        prInfoIn.PropertyType == typeof(int) ||
                        prInfoIn.PropertyType == typeof(float) ||
                        prInfoIn.PropertyType == typeof(double) ||
                        prInfoIn.PropertyType == typeof(object) ||
                        prInfoIn.PropertyType == typeof(DateTime) ||
                        prInfoIn.PropertyType == typeof(string)))
                    {
                        if (!PropertyInfoEx.isXPCollection(prInfoIn.PropertyType))
                            result.Add(new PropertyInfoEx(prInfoIn));
                        else
                        {
                            PropertyInfo prInfoCollection = prInfoIn.PropertyType.GetProperty("Object");
                            if (prInfoCollection != null)
                                result.Add(new PropertyInfoEx(prInfoIn, prInfoCollection));
                        }
                    }
            }

            return result;
        }

        public PropertyInfoExs GetProperties(Type type)
        {
            return GetProperties(null, type);
        }

        /// <summary>
        /// Рекурсивный перебор всех элементов дерева.
        /// </summary>
        public void CreateInitData(PropertyInfoExs infos, TableMemberInfo info, ref int recordCount, int level)
        {
            if (level < LevelMax)
                foreach (PropertyInfoEx prInfo in infos)
                {
                    if (!(prInfo.IsXPCollection &&
                        prInfo.Parent != null && prInfo.Parent.IsXPBaseObject))
                    {
                        TableMemberInfo tmInfo2 = AnalisProperty(prInfo, info, ref recordCount);
                        if (tmInfo2 == null) break;

                        PropertyInfoExs prInfos2 = GetProperties(prInfo);

                        CreateInitData(prInfos2, tmInfo2, ref recordCount, level + 1);
                    }
                    else
                    {
                    }
                }
        }

        /// <summary>
        /// Создание начальных записей в левой колонке.
        /// </summary>
        public void CreateInitData()
        {
            if (DBInterface != null &&
                DBInterface.TableType != null)
            {
                MemberInits.Clear();

                Type initType = DBInterface.TableType;

                int recordCount = 0;
                PropertyInfoExs infos1 = GetProperties(initType);
                foreach (PropertyInfoEx prInfo1 in infos1)
                {
                    TableMemberInfo tmInfo2 = AnalisProperty(prInfo1, null, ref recordCount);
                    if (tmInfo2 == null) break;                    
                    MemberInits.Add(tmInfo2);

                    PropertyInfoExs prInfos2 = GetProperties(prInfo1);

                    CreateInitData(prInfos2, tmInfo2, ref recordCount, 1);
                }
                lblTotalCount.Caption = string.Format("Всего: {0}", recordCount);

                treeInit.DataSource = null;
                treeInit.DataSource = MemberInits;
            }
        }

        /// <summary>
        /// Рекурсивный метод обновления записей уже созданных записей.
        /// </summary>
        private void CreateExistData(DBInterfaceItemXPComplex itemComplex, TableMemberInfos infos, int level)
        {
            foreach (DBInterfaceItemBase item in itemComplex.Items)
            {
                TableMemberInfo tmInfo = DBInterface.PrintExistData(treeExist, item, infos);
                if (item is DBInterfaceItemXPComplex)
                    CreateExistData((DBInterfaceItemXPComplex)item, tmInfo.Items, level + 1);
            }
        }

        /// <summary>
        /// Обновления дерева уже созданных записей.
        /// </summary>
        public void UpdateExistData()
        {
            MemberExists.Clear();
            foreach (DBInterfaceItemBase item in DBInterface.Items)
            {
                TableMemberInfo info = DBInterface.PrintExistData(treeExist, item, MemberExists);

                if (item is DBInterfaceItemXPComplex)
                    CreateExistData((DBInterfaceItemXPComplex)item, info.Items, 1);
            }
            treeExist.ExpandAll();
            UpdateExistButtons();
            DBInterface.ItemsSeq.CorrectImageIndex();
        }

        /// <summary>
        /// Рекурсивный метод обновления надписей и иконок..
        /// </summary>
        private void UpdateExistCaptionAndIcon(TableMemberInfos infos, int level)
        {
            foreach (TableMemberInfo info in infos)
            {
                TableMemberInfo infoExist = MemberExists.FindInfo(info);
                if (infoExist != null)
                {
                    infoExist.Caption = info.Caption;
                    DBInterface.CreateIcon(infoExist.Item, info);

                    if (info.Items.Count > 0)
                        UpdateExistCaptionAndIcon(info.Items, level + 1);
                }
            }
        }

        /// <summary>
        /// Обновления надписей и иконок.
        /// </summary>
        public void UpdateExistCaptionAndIcon()
        {
            foreach (TableMemberInfo info in MemberInits)
            {
                TableMemberInfo infoExist = MemberExists.FindInfo(info);
                if (infoExist != null)
                {
                    infoExist.Caption = info.Caption;
                    DBInterface.CreateIcon(infoExist.Item, info);

                    if (info.Items.Count > 0)
                        UpdateExistCaptionAndIcon(info.Items, 1);
                }
            }

            DBInterface.TableCaption = DBAttribute.GetCaption(DBInterface.TableType);
        }

        /// <summary>
        /// Ручное создание элемента коллекции.
        /// </summary>
        public DBInterfaceItemBase CreateInstance(Type type)
        {
            return DBInterface.CreateInstance(type, infoSelect: SelectedExistInfo, treeList: treeExist, MemberExists: MemberExists);
        }

        /// <summary>
        /// Автоматическое создание элемента коллекции.
        /// </summary>
        public DBInterfaceItemBase CreateInstance(TableMemberInfo tmInfo)
        {
            DBInterfaceItemBase item = null;
            TableMemberInfo infoParent = null;
            TableMemberInfo infoSelect = null;

            int level = tmInfo.GetLevel();

            for (int i = 0; i < level; i++)
            {
                infoParent = tmInfo.GetParent(level - i);
                infoSelect = MemberExists.FindInfo(infoParent.Parent);
                if (MemberExists.FindInfo(infoParent) == null)
                    item = DBInterface.CreateInstance(infoParent.PropertyType, infoParent, infoSelect, treeExist, MemberExists);
            }

            return item; 
        }

        public DBInterfaceItemBase CreateInstanceEx(Type type)
        {
            DBInterfaceItemBase dbItem = CreateInstance(type);
            TypeCurrent = type;
            drbtnAddColletion.Text = string.Format("Добавить ({0})", dbItem.TypeName);
            treeExist.ExpandAll();
            return dbItem;
        }

        /// <summary>
        /// Получшение текущей саписи TreeList, зная координаты щелчка мыши.
        /// </summary>
        private TreeListNode GetTreeListNode(TreeList treeList, Point point)
        {
            TreeListHitInfo info = treeList.CalcHitInfo(point);

            if (info.Node != null)
            {
                TreeListNode node = info.Node;
                return node;
            }
            return null;
        }

        /// <summary>
        /// Очистка записи.
        /// </summary>
        public void UnuseInitTable(TableMemberInfo tmInfo)
        {
            TableMemberInfo infoInit = MemberInits.FindInfo(tmInfo);
            if (infoInit != null)
                infoInit.IsUsed = false;
        }

        /// <summary>
        /// Рекурсивный метод очистки записей.
        /// </summary>
        private void EraseExistDate(TableMemberInfo tmInfo, int level)
        {
            foreach (TableMemberInfo item in tmInfo.Items)
            {
                UnuseInitTable(item);
                EraseExistDate(item, level + 1);
            }
        }

        /// <summary>
        /// Очистка всех вложеннных записей.
        /// </summary>
        private void EraseExistDate(TableMemberInfo tmInfo)
        {
            EraseExistDate(tmInfo, 1);
        }

        /// <summary>
        /// Изменение позиции записи в дереве.
        /// </summary>
        private void SetNodeIndex(int inc)
        {
            if (treeExist.Selection.Count > 0)
            {
                TreeListNode node = treeExist.Selection[0];
                int index = treeExist.GetNodeIndex(node);                    
                treeExist.SetNodeIndex(node, index + inc);

                TableMemberInfo tmInfo = treeExist.GetDataRecordByNode(node) as TableMemberInfo;
                if (tmInfo != null && tmInfo.Item != null)
                {
                    DBInterfaceItemBases dbItems = tmInfo.Item.Owner;
                    if (0 <= index + inc && index + inc < dbItems.Count)
                    {
                        DBInterfaceItemBases dbBuffs = new DBInterfaceItemBases();
                        dbItems.CopyTo(dbBuffs);
                        dbItems.Clear();
                        dbBuffs.Change(index, index + inc);
                        dbBuffs.CopyTo(dbItems);
                        //dbItems.CorrectVisibleIndex();
                    }
                }                
            }
        }

        /// <summary>
        /// Обновление визуального представление кнопок "Выше" и "Ниже".
        /// </summary>
        public void UpdateExistButtons()
        {
            TableMemberInfo info = SelectedExistInfo;
            if (info != null)
            {
                btnExistUp.Enabled = !info.Item.IsFirst();
                btnExistDown.Enabled = !info.Item.IsLast();
            }
            else
            {
                btnExistUp.Enabled = false;
                btnExistDown.Enabled = false;
            }
        }
        #endregion
        
        #region Metods Handler
        private void RapidInterfaceDesignerForm_Load(object sender, EventArgs e)
        {
            treeExist.ExpandAll();
            treeExist.FocusedNode = treeExist.Nodes.FirstNode;
        }

        private void treeExist_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            TableMemberInfo tmInfo = treeExist.GetDataRecordByNode(e.Node) as TableMemberInfo;
            SelectedExistInfo = tmInfo;
            if (tmInfo != null && tmInfo.Item != null)
            {
                proExist.SelectedObject = tmInfo.Item;
                UpdateExistButtons();
            }
        }

        private void treeExist_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            proExist.Refresh();
        }

        private void btnAddInit_Click(object sender, EventArgs e)
        {
            foreach (TreeListNode node in treeInit.Selection)
            {
                TableMemberInfo info = treeInit.GetDataRecordByNode(node) as TableMemberInfo;
                CreateInstance(info);
            }
            treeExist.ExpandAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            TableMemberInfos deletings = new TableMemberInfos(rewriteParent: false);
            foreach (TreeListNode node in treeExist.Selection)
            {
                TableMemberInfo info = treeExist.GetDataRecordByNode(node) as TableMemberInfo;
                if (info != null &&
                    info.Item != null)
                {
                    DBInterface.DestroyInstance(info.Item);

                    UnuseInitTable(info);
                    EraseExistDate(info);

                    deletings.Add(info);                    
                }
            }

            // Удаление записей
            foreach (TableMemberInfo infoInter in deletings)
                MemberExists.RemoveEx(infoInter);
        }

        private void drbtnAddColletion_Click(object sender, EventArgs e)
        {
            CreateInstanceEx(TypeCurrent);
        }

        private void treeInit_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeListNode node = GetTreeListNode(treeInit, e.Location);
            if (node != null)
            {
                TableMemberInfo info = treeInit.GetDataRecordByNode(node) as TableMemberInfo;
                CreateInstance(info);
            }
        }

        private void barString_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateInstanceEx(typeof(string));
        }

        private void barDateTime_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateInstanceEx(typeof(DateTime));
        }

        private void barNumeric_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateInstanceEx(typeof(int));
        }

        private void barBoolean_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateInstanceEx(typeof(bool));
        }

        private void barXPObject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateInstanceEx(typeof(XPObject));
        }

        private void barXPCollection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateInstanceEx(typeof(XPCollection));
        }

        void dbInterface_ItemsChanged(object sender, EventArgs e)
        {
            UpdateExistData();
        }

        private void proExist_SelectedObjectsChanged(object sender, EventArgs e)
        {
            UpdatePropertyGridSite();
            proExist.ShowEvents(true);
        }

        protected void UpdatePropertyGridSite()
        {
            if (proExist != null)
            {
                proExist.Site = null;
                IServiceProvider provider = GetPropertyGridServiceProvider();
                if (provider != null)
                {
                    proExist.Site = (ISite)provider;
                    proExist.PropertyTabs.AddTabType(typeof(System.Windows.Forms.Design.EventsTab));
                }
            }
        }

        protected IServiceProvider GetPropertyGridServiceProvider()
        {
            object selObject = null;
            if (proExist.SelectedObjects != null && proExist.SelectedObjects.Length > 0)
                selObject = proExist.SelectedObjects[0];
            else
                selObject = proExist.SelectedObject;

            if (selObject is Component)
                return (selObject as Component).Site;

            return null;
        }

        private void btnAddInit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                /*
                TableMemberInfo info = memInit[0];
                info.IsUsed = !info.IsUsed;
                info.FieldName += "s";
                 */
                treeInit.DataSource = null;
                treeInit.DataSource = MemberInits;
            }
        }

        private void treeExist_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point point = treeExist.PointToClient(Control.MousePosition);
                TreeListNode node = GetTreeListNode(treeExist, point);
                if (node != null)
                {
                    TableMemberInfo tmInfo = treeExist.GetDataRecordByNode(node) as TableMemberInfo;
                    if (tmInfo != null)
                        MemberExists.Remove(tmInfo);
                }
            }
        }

        private void treeExist_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            TableMemberInfo info = treeExist.GetDataRecordByNode(e.Node) as TableMemberInfo;
            if (info != null)
                e.NodeImageIndex = info.Item.ImageIndex;
        }

        private void treeInit_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            TableMemberInfo info = treeInit.GetDataRecordByNode(e.Node) as TableMemberInfo;
            if (info != null)
                e.NodeImageIndex = info.ImageIndex;
        }

        private void btnExistUp_Click(object sender, EventArgs e)
        {
            SetNodeIndex(-1);
            UpdateExistButtons();
        }

        private void btnExistDown_Click(object sender, EventArgs e)
        {
            SetNodeIndex(+1);
            UpdateExistButtons();
        }

        private void BDInterfaceDesignerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DBInterface.CountOpenDesigner++;
            DBInterface.RefreshDesignCode();
        }

        private void gluInits_EditValueChanged(object sender, EventArgs e)
        {
            XPTable table = (XPTable)gluInits.EditValue;
            DBInterface.TableType = table.Type;
            CreateInitData();
        }

        private void btnUpdateExist_Click(object sender, EventArgs e)
        {
            UpdateExistData();
        }

        private void btnUpdateCaptionAndIcon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateExistCaptionAndIcon();
        }
        #endregion                
    }
}
