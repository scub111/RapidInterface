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
    public partial class DBFormDesignerForm : Form
    {
        #region Contructors
        public DBFormDesignerForm(DBForm dbForm)
        {
            InitializeComponent();
            DBForm = dbForm;
            MemberInits = new ViewMemberInfos();
            MemberExists = new ViewMemberInfos();

            treeExist.StateImageList = DBForm.Icons;

            UpdateExistData();
            GenerateInitTables();

            treeInit.DataSource = MemberInits;
            treeExist.DataSource = MemberExists;

            TypeTables = new XPTables();
            TypeTables.FillTable(DBForm.TypeDiscoveryService, typeof(DBViewBase));
            gluiViewType2.DataSource = TypeTables;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Основной компонент.
        /// </summary>
        DBForm DBForm { get; set; }

        /// <summary>
        /// Поля таблицы, сгенированные для инициализации.
        /// </summary>
        ViewMemberInfos MemberInits { get; set; }

        /// <summary>
        /// Поля таблицы, заданные для компонента.
        /// </summary>
        ViewMemberInfos MemberExists { get; set; }

        /// <summary>
        /// Текущее выделенная запись среди существующих в списке.
        /// </summary>
        ViewMemberInfo SelectedExistInfo { get; set; }

        /// <summary>
        /// Список возможных типов таблицы.
        /// </summary>
        XPTables TypeTables { get; set; }
        #endregion

        #region Metods Common

        /// <summary>
        /// Создание списка возможных типов таблицы.
        /// </summary>
        public void GenerateInitTables()
        {
            if (DBForm.TypeDiscoveryService != null)
            {
                ICollection types = DBForm.TypeDiscoveryService.GetTypes(typeof(DBViewBase), false);
                foreach (Type actionType in types)
                    if (actionType != typeof(DBViewBase))
                    {
                        ViewMemberInfo info = new ViewMemberInfo();
                        info.ViewType = actionType;
                        info.Caption = DBAttribute.GetCaption(actionType);

                        string name = DBAttribute.GetIconFile(info.ViewType);

                        string dir = DBForm.GetImageFullName(name);
                        if (dir != "" && File.Exists(dir) && !ImageEx.IsExist(imgInit, name))
                            imgInit.AddImage(Image.FromFile(dir), name);

                        info.ImageIndex = ImageEx.GetImageIndex(imgInit, name);
                        info.ImageName = name;
                        if (MemberExists.FindInfo(info) != null)
                            info.IsUsed = true;

                        MemberInits.Add(info);
                    }
                lblTotalCount.Caption = string.Format("Всего: {0}", types.Count);
            }
        }

        /// <summary>
        /// Обновления дерева уже созданных записей.
        /// </summary>
        public void UpdateExistData()
        {
            MemberExists.Clear();

            foreach (DBFormItemBase item in DBForm.Items)
                DBForm.PrintExistData(treeExist, item, MemberExists);

            treeExist.ExpandAll();

            UpdateExistButtons();

            DBForm.Items.CorrectImageIndex();
        }

        /// <summary>
        /// Обновления надписей и иконок.
        /// </summary>
        public void UpdateExistCaptionAndIcon()
        {
            foreach (ViewMemberInfo info in MemberInits)
            {
                ViewMemberInfo infoExist = MemberExists.FindInfo(info);
                if (infoExist != null)
                {
                    infoExist.Caption = info.Caption;
                    DBForm.CreateIcon(infoExist.Item, info);
                }
            }
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
        /// Автоматическое создание элемента коллекции.
        /// </summary>
        public DBFormItemBase CreateInstance(ViewMemberInfo info = null)
        {
            DBFormItemBase item = null;
            if (info == null)
                item = DBForm.CreateInstance(treeList:treeExist, memberExists:MemberExists);
            else if (MemberExists.FindInfo(info) == null)
                item = DBForm.CreateInstance(info, treeExist, MemberExists);
            return item;
        }

        /// <summary>
        /// Обновление визуального представление кнопок "Выше" и "Ниже".
        /// </summary>
        public void UpdateExistButtons()
        {
            ViewMemberInfo info = SelectedExistInfo;
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

                ViewMemberInfo tmInfo = treeExist.GetDataRecordByNode(node) as ViewMemberInfo;
                if (tmInfo != null && tmInfo.Item != null)
                {
                    DBFormItemBases dbItems = DBForm.Items;
                    if (0 <= index + inc && index + inc < dbItems.Count)
                    {
                        DBFormItemBases dbBuffs = new DBFormItemBases();
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
        /// Очистка записи.
        /// </summary>
        public void UnuseInitTable(ViewMemberInfo tmInfo)
        {
            ViewMemberInfo  infoInit = MemberInits.FindInfo(tmInfo);
            if (infoInit != null)
                infoInit.IsUsed = false;
        }
        #endregion
                
        #region Metods Handler
        private void DBFormDesignerForm_Load(object sender, EventArgs e)
        {
            treeExist.ExpandAll();
            treeExist.FocusedNode = treeExist.Nodes.FirstNode;
        }

        private void treeInit_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            ViewMemberInfo info = treeInit.GetDataRecordByNode(e.Node) as ViewMemberInfo;
            if (info != null)
                e.NodeImageIndex = info.ImageIndex;
        }

        private void treeExist_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            ViewMemberInfo info = treeExist.GetDataRecordByNode(e.Node) as ViewMemberInfo;
            if (info != null)
                e.NodeImageIndex = info.ImageIndex;
        }

        private void btnAddInit_Click(object sender, EventArgs e)
        {
            foreach (TreeListNode node in treeInit.Selection)
            {
                ViewMemberInfo info = treeInit.GetDataRecordByNode(node) as ViewMemberInfo;
                CreateInstance(info);
            }
        }

        private void treeExist_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            ViewMemberInfo info = treeExist.GetDataRecordByNode(e.Node) as ViewMemberInfo;
            SelectedExistInfo = info;
            if (info != null && info.Item != null)
            {
                proExist.SelectedObject = info.Item;
                UpdateExistButtons();
            }
        }

        private void treeExist_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            proExist.Refresh();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CreateInstance();
        }

        private void treeInit_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeListNode node = GetTreeListNode(treeInit, e.Location);
            if (node != null)
            {
                ViewMemberInfo info = treeInit.GetDataRecordByNode(node) as ViewMemberInfo;
                CreateInstance(info);
            }
        }

        private void DBFormDesignerForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void DBFormDesignerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DBForm.CountOpenDesigner++;
            DBForm.RefreshDesignCode();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ViewMemberInfos deletings = new ViewMemberInfos();
            foreach (TreeListNode node in treeExist.Selection)
            {
                ViewMemberInfo info = treeExist.GetDataRecordByNode(node) as ViewMemberInfo;
                if (info != null &&
                    info.Item != null)
                {
                    DBForm.DestroyInstance(info.Item);
                    
                    UnuseInitTable(info);
                    //EraseExistDate(vwInfo);

                    deletings.Add(info);
                }
            }

            // Удаление записей
            foreach (ViewMemberInfo infoInter in deletings)
                MemberExists.Remove(infoInter);
        }

        private void treeExist_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
            if (e.Column.FieldName == "ViewType")
            {
                //e.RepositoryItem.ed
            }
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
