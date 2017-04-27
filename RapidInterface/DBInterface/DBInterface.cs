using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars.Docking;
using System.Drawing.Design;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using DevExpress.XtraEditors.Repository;
using System.Windows.Forms.Design;
using DevExpress.XtraGrid;
using DevExpress.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors.Controls;
using DevExpress.Data;
using Usable;

namespace RapidInterface
{
    [DesignerAttribute(typeof(DBInterfaceDesigner))]
    public partial class DBInterface : UserControl
    {
        #region Constructors
        public DBInterface()
        {
            InitializeComponent();

            //Dock = DockStyle.Fill;

            Items = new DBInterfaceItemBases(rewrite: true);
            ItemsSeq = new DBInterfaceItemBases();
            ItemDestroys = new DBInterfaceItemBases();
            XPCollections = new XPCollectionContainer();

            _ReadOnlyBackColor = Color.Gainsboro;
            MasterRowInfos = new List<MasterRowInfo>();
            IsFirstLoad = true;
            ProgramDesign = false;
        }
        #endregion

        /// <summary>
        /// Тип XPCollection.
        /// </summary>
        public enum XPCollectionType
        {
            [Description("Simple")]
            Simple,
            [Description("Server Mode only first")]
            ServerModeOnlyFirst,
            [Description("Server Mode all")]
            ServerModeAll,
            [Description("Different")]
            Different
        }

        #region Properties
        /// <summary>
        /// Основной объект XPCollection.
        /// </summary>
        [Category("Visible components")]
        public Component BaseXPCollecton { get; set; }

        /// <summary>
        /// Основной объект UnitOfWork.
        /// </summary>
        [Category("Visible components")]
        public UnitOfWork BaseUnitOfWork { get; set; }

        /// <summary>
        /// Менеджер плавающих панелей.
        /// </summary>
        [Category("Visible components")]
        public DockManager DockManager { get; set; }

        /// <summary>
        /// Панель формы.
        /// </summary>
        [Category("Visible components")]
        public DockPanel FormDockPanel { get; set; }

        /// <summary>
        /// Контейнер для панели формы.
        /// </summary>
        [Category("Visible components")]
        public ControlContainer FormDockContainer { get; set; }

        /// <summary>
        /// Панель таблицы.
        /// </summary>
        [Category("Visible components")]
        public DockPanel TableDockPanel { get; set; }

        /// <summary>
        /// Контейнер для панели таблицы.
        /// </summary>
        [Category("Visible components")]
        public ControlContainer TableDockContainer { get; set; }

        /// <summary>
        /// Панель, объединяющая панели формы и таблицы.
        /// </summary>
        [Category("Visible components")]
        public DockPanel DockPanelMerge { get; set; }

        /// <summary>
        /// Разметка формы.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlEx FormLayoutControl { get; set; }

        /// <summary>
        /// Группа разметки формы.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlGroup FormLayoutGroup { get; set; }

        /// <summary>
        /// Группа разметки основных данных.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlGroup FormDataLayoutGroup { get; set; }

        /// <summary>
        /// Компонент разметки таблицы.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlEx TableLayoutControl { get; set; }

        /// <summary>
        /// Группа разметки таблицы.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlGroup TableLayoutGroup { get; set; }

        /// <summary>
        /// Ячейка разметки панели навигации.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlItem FormNavigatorLayoutItem { get; set; }

        /// <summary>
        /// Компонент навигации формы.
        /// </summary>
        [Category("Visible components")]
        public DataNavigatorEx FormNavigatorControl { get; set; }

        /// <summary>
        /// Объект таблицы.
        /// </summary>
        [Category("Visible components")]
        public GridControlEx TableGridControl { get; set; }

        /// <summary>
        /// Вид объекта таблицы.
        /// </summary>
        [Category("Visible components")]
        public BandedGridView TableGridView { get; set; }

        /// <summary>
        /// Связка колонок таблицы.
        /// </summary>
        [Category("Visible components")]
        public GridBand TableGridBand { get; set; }

        /// <summary>
        /// Ячейка разметки таблицы.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlItem TableGridLayoutItem { get; set; }

        /// <summary>
        /// Ячейка разметки панели навигации.
        /// </summary>
        [Category("Visible components")]
        public LayoutControlItem TableNavigatorLayoutItem { get; set; }

        /// <summary>
        /// Компонент навигации формы.
        /// </summary>
        [Category("Visible components")]
        public DataNavigatorEx TableNavigatorControl { get; set; }

        /// <summary>
        /// Компонент коллекции иконок.
        /// </summary>
        [Category("Visible components")]
        public ImageCollection Icons { get; set; }

        /// <summary>
        /// Путь к папке, где располагаются в иконки.
        /// </summary>
        [Category("Appearance")]
        public string ImagePath { get; set; }

        /// <summary>
        /// Коллекция визиальных элементов интерфейса.
        /// </summary>
        [Editor(typeof(DBInterfaceItemCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Invisible components")]
        public DBInterfaceItemBases Items { get; set; }

        /// <summary>
        /// Коллекция визиальных элементов интерфейса в последовательном порядке.
        /// </summary>
        [Editor(typeof(DBInterfaceItemCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Invisible components")]
        public DBInterfaceItemBases ItemsSeq {get; set;}

        /// <summary>
        /// Коллекция удаленных элементов интерфейса.
        /// </summary>
        [Browsable(false)]
        public DBInterfaceItemBases ItemDestroys {get; set;}

        /// <summary>
        /// Коллекция XPCollection, с которыми работает основной компонент.
        /// </summary>
        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Invisible components")]
        public XPCollectionContainer XPCollections { get; set; }

        /// <summary>
        /// Компонент навигации.
        /// </summary>
        [Category("Reference components")]
        public DataNavigatorEx NavigatorControl
        {
            get
            {
                if (TableNavigatorControl != null)
                    return TableNavigatorControl;
                else
                    return FormNavigatorControl;

            }
        }

        /// <summary>
        /// Название таблицы.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        public string TableCaption
        {
            get
            {
                if (FormNavigatorControl != null)
                    return FormNavigatorControl.TableCaption;
                else
                    return "";
            }
            set
            {
                if (FormNavigatorControl != null)
                    FormNavigatorControl.TableCaption = value;

                if (TableNavigatorControl != null)
                    TableNavigatorControl.TableCaption = value;
            }
        }

        /// <summary>
        /// Текущая позиция формы.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Data")]
        public int RecordPosition
        {
            get
            {
                if (FormNavigatorControl != null && 
                    TableNavigatorControl != null &&
                    FormNavigatorControl.Position == TableNavigatorControl.Position)
                    return FormNavigatorControl.Position;
                else
                    return 0;
            }
            set
            {
                if (FormNavigatorControl != null &&
                    TableNavigatorControl != null)
                    FormNavigatorControl.Position = TableNavigatorControl.Position = value;
            }
        }

        /// <summary>
        /// Список раскрытых записей.
        /// </summary>
        private List<MasterRowInfo> MasterRowInfos { get; set; }

        /// <summary>
        /// Количество раз открытия дизайнера.
        /// </summary>
        [Category("Data")]
        public int CountOpenDesigner { get; set; }

        Type _TableType;
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue("")]
        [TypeConverter(typeof(XPSourceConverter<XPBaseObject>))]
        [Category("Data")]
        public Type TableType 
        {
            get
            {
                return _TableType;
            }
            set
            {
                if (TableType != null)
                    UpdateTypeXPCollection(value);
                else
                {
                    if (BaseXPCollecton is XPCollection)
                        ((XPCollection)BaseXPCollecton).ObjectType = value;
                    else if (BaseXPCollecton is XPServerCollectionSource)
                        ((XPCollection)BaseXPCollecton).ObjectType = value;
                }
                TableCaption = DBAttribute.GetCaption(value);
                _TableType = value;
            }
        }

        /// <summary>
        /// Сервис типов.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITypeDiscoveryService TypeDiscoveryService { get; set; }

        /// <summary>
        /// Задание цвета заголовка группам разметки на вкладке "Форма".
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color GroupsBackColor
        {
            get
            {
                Color color = new Color();
                int count = 0;
                if (FormLayoutGroup != null)
                {
                    foreach (object obj in FormLayoutGroup.Items)
                    {
                        if (obj is LayoutControlGroup)
                        { 
                            CalculateSimularColor(((LayoutControlGroup)obj).AppearanceGroup.BackColor, ref color, ref count);
                            if (color == Color.Empty) break;
                        }
                    }
                    return color;
                }
                else
                    return Color.Empty;
            }
            set
            {
                if (FormLayoutGroup != null)
                    foreach (object obj in FormLayoutGroup.Items)
                        if (obj is LayoutControlGroup)
                            ((LayoutControlGroup)obj).AppearanceGroup.BackColor = value;
            }
        }

        Color _ReadOnlyBackColor;
        /// <summary>
        /// Цвет "Только для чтения".
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color ReadOnlyBackColor
        {
            get
            {
                Color color = _ReadOnlyBackColor;
                int count = 0;
                foreach (DBInterfaceItemBase item in ItemsSeq)
                {
                    if (item.ReadOnly)
                    {
                        if (item.FormGridColumn != null)
                            CalculateSimularColor(item.FormGridColumn.AppearanceCell.BackColor, ref color, ref count);
                        if (item.TableGridColumn != null)
                            CalculateSimularColor(item.TableGridColumn.AppearanceCell.BackColor, ref color, ref count);

                        if (color == Color.Empty) break;
                    }
                }
                return color;
            }
            set
            {
                _ReadOnlyBackColor = value;
                foreach (DBInterfaceItemBase item in ItemsSeq)
                {
                    if (item.ReadOnly)
                    {
                        if (item.FormGridColumn != null)
                            item.FormGridColumn.AppearanceCell.BackColor = value;
                        if (item.TableGridColumn != null)
                            item.TableGridColumn.AppearanceCell.BackColor = value;
                    }
                }
            }
        }

        /// <summary>
        /// Тип коллекции данных.
        /// </summary>
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(EnumTypeConverter))]
        [DefaultValue("Simple")]
        public XPCollectionType CollectionType 
        {
            get
            {
                int countXPCollection = 0;
                bool isFirstXPCollection = false;
                int countSVCollection = 0;
                for (int i = 0; i < XPCollections.Count; i++)
                {
                    Component comp = XPCollections[i];

                    if (comp is XPCollection)
                    {
                        countXPCollection++;

                        if (i == 0)
                            isFirstXPCollection = true;
                    }
                    if (comp is XPServerCollectionSource)
                        countSVCollection++;
                }

                if (countSVCollection == 0)
                {
                    return XPCollectionType.Simple;
                }
                else
                {
                    if (countXPCollection == 1 && isFirstXPCollection)
                        return XPCollectionType.ServerModeOnlyFirst;
                    if (countXPCollection == 0)
                        return XPCollectionType.ServerModeAll;
                }

                return XPCollectionType.Different;
            }
            set
            {
                if (CollectionType == value) return;

                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

                Collection<Component> deletings = new Collection<Component>();
                string name;
                Type tableType = null;

                switch (value)
                {
                    case XPCollectionType.Simple:
                        foreach (Component comp in XPCollections)
                        {
                            if (comp is XPServerCollectionSource)
                            {
                                if (comp.Site != null)
                                {
                                    name = comp.Site.Name;

                                    bool isBaseXPCollection = false;                                    
                                    if (comp == BaseXPCollecton)
                                    {
                                        isBaseXPCollection = true;
                                        tableType = TableType;
                                    }


                                    HostComponent.DestroyComponent(host, comp);
                                    XPCollection compTemp = (XPCollection)HostComponent.CreateComponent(host, typeof(XPCollection), name);

                                    if (isBaseXPCollection)
                                    {
                                        BaseXPCollecton = compTemp as Component;
                                        XPCollection xpCollection = compTemp;
                                        XPCollectionContainer.InitXPCollection(xpCollection, tableType, BaseUnitOfWork);

                                        // Добавление в список.
                                        XPCollections.Insert(0, BaseXPCollecton);

                                        // Привязка к интерфейсу.
                                        SetBaseXPCollection(BaseXPCollecton, BaseUnitOfWork);
                                    }
                                }
                            }
                        }
                        break;
                    case XPCollectionType.ServerModeOnlyFirst:
                        if (BaseXPCollecton is XPCollection)
                        {
                            if (BaseXPCollecton.Site != null)
                            {
                                name = BaseXPCollecton.Site.Name;
                                tableType = TableType;
                                HostComponent.DestroyComponent(host, BaseXPCollecton);
                                BaseXPCollecton = (Component)HostComponent.CreateComponent(host, typeof(XPServerCollectionSource), name);

                                XPServerCollectionSource svCollection = BaseXPCollecton as XPServerCollectionSource;
                                XPCollectionContainer.InitSVCollection(svCollection, tableType, BaseUnitOfWork);

                                // Привязка к интерфейсу.
                                SetBaseXPCollection(BaseXPCollecton, BaseUnitOfWork);
                            }
                        }

                        break;

                    case XPCollectionType.ServerModeAll:
                        foreach (Component comp in XPCollections)
                        {
                            if (comp is XPCollection)
                            {
                                if (comp.Site != null)
                                {
                                    name = comp.Site.Name;

                                    bool isBaseXPCollection = false;
                                    if (comp == BaseXPCollecton)
                                    {
                                        isBaseXPCollection = true;
                                        tableType = TableType;
                                    }

                                    HostComponent.DestroyComponent(host, comp);
                                    XPServerCollectionSource compTemp = (XPServerCollectionSource)HostComponent.CreateComponent(host, typeof(XPServerCollectionSource), name);

                                    if (isBaseXPCollection)
                                    {
                                        BaseXPCollecton = compTemp as Component;
                                        XPServerCollectionSource svCollection = compTemp;
                                        XPCollectionContainer.InitSVCollection(svCollection, tableType, BaseUnitOfWork);

                                        // Привязка к интерфейсу.
                                        SetBaseXPCollection(BaseXPCollecton, BaseUnitOfWork);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Необходимость автогенерации коллекций данных.
        /// </summary>
        [Category("Data")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool XPCollectionNeed
        {
            get
            {
                //return _XPCollectionNeed;
                if (XPCollections != null && XPCollections.Count > 0)
                    return true;
                else
                    return false;
            }
            set
            {
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                if (value)
                {
                    BaseXPCollecton = (XPCollection)HostComponent.CreateComponent(host, typeof(XPCollection), "baseXPCollecton");
                    BaseUnitOfWork = (UnitOfWork)HostComponent.CreateComponent(host, typeof(UnitOfWork), "baseUnitOfWork");

                    XPCollectionContainer.InitXPCollection((XPCollection)BaseXPCollecton, TableType, BaseUnitOfWork);

                    // Привязка к интерфейсу.
                    SetBaseXPCollection(BaseXPCollecton, BaseUnitOfWork);

                    foreach (DBInterfaceItemBase item in ItemsSeq)
                        if (item is DBInterfaceItemXPObject)
                            CreateXPCollection(host, (DBInterfaceItemXPObject)item);
                }
                else
                {
                    HostComponent.DestroyComponent(host, BaseUnitOfWork);

                    foreach (Component comp in XPCollections)
                        HostComponent.DestroyComponent(host, comp);
                    XPCollections.Clear();
                }
            }
        }

        /// <summary>
        /// Текущая запись.
        /// </summary>
        private XPBaseObject RecordCurrent { get; set; }

        /// <summary>
        /// Флаг перетаскивания панелей.
        /// </summary>
        private bool Docking { get; set; }

        /// <summary>
        /// Состояние визуальных элементов до скрытия вида.
        /// </summary>
        //public InterfaceViewState ViewState { get; set; }

        /// <summary>
        /// Первая загрузка.
        /// </summary>
        private bool IsFirstLoad { get; set; }

        /// <summary>
        /// Программное редактирование интерфейса.
        /// </summary>
        public bool ProgramDesign { get; set; }

        /// <summary>
        /// Режим разработки.
        /// </summary>
        public bool IsDesignModeEx
        {
            get
            {
                if (!ProgramDesign)
                    return DesignMode;
                else
                    return true;
            }
        }

        #endregion

        #region Events
        public event EventHandler ItemsChanged;

        /// <summary>
        /// Событие для нажатия правой кнопки мыши по выпадающему списку.
        /// </summary>
        public event DBInterfaceItemXPObject.RightMouseDownEventHandler RightMouseDown;

        /// <summary>
        /// Событие, возникающее перед сохранением данных.
        /// </summary>
        public event EventHandler DataBaseSaving;

        /// <summary>
        /// Событие, возникающее после сохранения данных.
        /// </summary>
        public event EventHandler DataBaseSaved;

        /// <summary>
        /// Событие на отключение от сервера.
        /// </summary>
        public event EventHandler<CurrentObjectEventArgs> CurrentObjectChanged = delegate { };

        #endregion

        #region Metods common
        /// <summary>
        /// При изменении свойств.
        /// </summary>        
        private void OnChangeProperties()
        {
            Invalidate();
        }

        /// <summary>
        /// Подсчет схожих цветов.
        /// </summary>
        public static void CalculateSimularColor(Color colorAnalis, ref Color colorTemp, ref int count)
        {
            count++;
            if (count == 1)
                colorTemp = colorAnalis;

            if (colorTemp != colorAnalis)
                colorTemp = Color.Empty;
        }

        /// <summary>
        /// Инициализация компонента и создание группы других компонентов.
        /// </summary>
        public void InitializeVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("InitializeDBInterface");

            // Создание компонентов.
            BaseXPCollecton = (XPCollection)HostComponent.CreateComponent(host, typeof(XPCollection), "baseXPCollecton");
            BaseUnitOfWork = (UnitOfWork)HostComponent.CreateComponent(host, typeof(UnitOfWork), "baseUnitOfWork");
            DockManager = (DockManager)HostComponent.CreateComponent(host, typeof(DockManager), "dockManager");
            FormDockPanel = (DockPanel)HostComponent.CreateComponent(host, typeof(DockPanel), "formDockPanel");
            FormDockContainer = (ControlContainer)HostComponent.CreateComponent(host, typeof(ControlContainer), "formDockContainer");
            TableDockPanel = (DockPanel)HostComponent.CreateComponent(host, typeof(DockPanel), "tableDockPanel");
            TableDockContainer = (ControlContainer)HostComponent.CreateComponent(host, typeof(ControlContainer), "tableDockContainer");
            DockPanelMerge = (DockPanel)HostComponent.CreateComponent(host, typeof(DockPanel), "dockPanelMerge");

            FormLayoutControl = (LayoutControlEx)HostComponent.CreateComponent(host, typeof(LayoutControlEx), "formLayoutControl");
            FormLayoutGroup = (LayoutControlGroup)HostComponent.CreateComponent(host, typeof(LayoutControlGroup), "formLayoutGroup");
            FormDataLayoutGroup = (LayoutControlGroup)HostComponent.CreateComponent(host, typeof(LayoutControlGroup), "formDataLayoutGroup");
            FormNavigatorLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), "formNavigatorLayoutItem");
            FormNavigatorControl = (DataNavigatorEx)HostComponent.CreateComponent(host, typeof(DataNavigatorEx), "formNavigatorControl");

            TableLayoutControl = (LayoutControlEx)HostComponent.CreateComponent(host, typeof(LayoutControlEx), "tableLayoutControl");
            TableLayoutGroup = (LayoutControlGroup)HostComponent.CreateComponent(host, typeof(LayoutControlGroup), "tableLayoutGroup");
            TableGridControl = (GridControlEx)HostComponent.CreateComponent(host, typeof(GridControlEx), "tableGridControl");
            TableGridView = (BandedGridView)HostComponent.CreateComponent(host, typeof(BandedGridView), "tableGridView");
            TableGridBand = (GridBand)HostComponent.CreateComponent(host, typeof(GridBand), "tableGridBand");
            TableNavigatorLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), "tableNavigatorLayoutItem");
            TableNavigatorControl = (DataNavigatorEx)HostComponent.CreateComponent(host, typeof(DataNavigatorEx), "tableNavigatorControl");
            TableGridLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), "tableGridLayoutItem");
            Icons = (ImageCollection)HostComponent.CreateComponent(host, typeof(ImageCollection), "icons");

            // Initial actions
            FormDockPanel.SuspendLayout();
            FormDockContainer.SuspendLayout();
            TableDockPanel.SuspendLayout();
            TableDockContainer.SuspendLayout();
            DockPanelMerge.SuspendLayout();
            ((ISupportInitialize)(BaseXPCollecton)).BeginInit();
            ((ISupportInitialize)(BaseUnitOfWork)).BeginInit();
            ((ISupportInitialize)(DockManager)).BeginInit();
            ((ISupportInitialize)(FormLayoutControl)).BeginInit();
            ((ISupportInitialize)(FormLayoutGroup)).BeginInit();
            ((ISupportInitialize)(FormDataLayoutGroup)).BeginInit();
            ((ISupportInitialize)(FormNavigatorLayoutItem)).BeginInit();
            ((ISupportInitialize)(TableLayoutControl)).BeginInit();
            ((ISupportInitialize)(TableLayoutGroup)).BeginInit();
            ((ISupportInitialize)(TableGridControl)).BeginInit();
            ((ISupportInitialize)(TableGridView)).BeginInit();
            ((ISupportInitialize)(TableGridLayoutItem)).BeginInit();
            ((ISupportInitialize)(TableNavigatorLayoutItem)).BeginInit();
            ((ISupportInitialize)(Icons)).BeginInit();

            // 
            // BaseXPCollecton
            //

            ((XPCollection)BaseXPCollecton).Session = BaseUnitOfWork;
            ((XPCollection)BaseXPCollecton).DeleteObjectOnRemove = true;
            // 
            // DockManager
            // 
            DockManager.Form = this;
            DockManager.RootPanels.AddRange(new DockPanel[] {
            DockPanelMerge});
            // 
            // FormDockPanel
            // 
            FormDockPanel.Controls.Add(FormDockContainer);
            FormDockPanel.Dock = DockingStyle.Left;
            FormDockPanel.Location = new Point(0, 0);
            FormDockPanel.Size = new Size(400, 400);
            FormDockPanel.FloatSize = new Size(600, 600);
            FormDockPanel.Text = "Форма";
            // 
            // FormDockContainer
            // 
            FormDockContainer.Controls.Add(FormLayoutControl);
            FormDockContainer.Location = new Point(4, 23);
            FormDockContainer.Size = new Size(192, 317);
            // 
            // TableDockPanel
            // 
            TableDockPanel.Controls.Add(TableDockContainer);
            TableDockPanel.Dock = DockingStyle.Left;
            TableDockPanel.Location = new Point(200, 0);
            TableDockPanel.Size = new Size(400, 400);
            TableDockPanel.FloatSize = new Size(600, 600);
            TableDockPanel.Text = "Таблица";
            // 
            // TableDockContainer
            // 
            TableDockContainer.Controls.Add(TableLayoutControl);
            TableDockContainer.Location = new Point(4, 23);
            TableDockContainer.Size = new Size(192, 317);
            // 
            // DockPanelMerge
            // 
            DockPanelMerge.ActiveChild = FormDockPanel;
            DockPanelMerge.Controls.Add(FormDockPanel);
            DockPanelMerge.Controls.Add(TableDockPanel);
            DockPanelMerge.Dock = DockingStyle.Fill;
            DockPanelMerge.FloatVertical = true;
            DockPanelMerge.Location = new Point(0, 0);
            DockPanelMerge.OriginalSize = new Size(501, 200);
            DockPanelMerge.Size = new Size(624, 366);
            DockPanelMerge.Tabbed = true;
            DockPanelMerge.TabsPosition = TabsPosition.Left;
            // 
            // FormLayoutControl
            //
            FormLayoutControl.Controls.Add(FormNavigatorControl);
            FormLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            FormLayoutControl.Root = FormLayoutGroup;
            FormLayoutControl.Images = Icons;
            // 
            // FormLayoutGroup
            //
            FormLayoutGroup.CustomizationFormText = "Основная группа формы";
            FormLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            FormLayoutGroup.GroupBordersVisible = false;
            FormLayoutGroup.TextVisible = false;
            FormLayoutGroup.Items.AddRange(new BaseLayoutItem[] { FormNavigatorLayoutItem, FormDataLayoutGroup });
            // 
            // FormDataLayoutGroup
            //
            FormDataLayoutGroup.CustomizationFormText = "Основные данные";
            FormDataLayoutGroup.Text = "Основные данные";
            // 
            // FormNavigatorLayoutItem
            //
            FormNavigatorLayoutItem.Control = FormNavigatorControl;
            FormNavigatorLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            FormNavigatorLayoutItem.CustomizationFormText = "Панель навигации";
            FormNavigatorLayoutItem.Size = new System.Drawing.Size(1, 1);
            FormNavigatorLayoutItem.TextVisible = false;
            // 
            // FormNavigatorControl
            // 
            FormNavigatorControl.DataSource = BaseXPCollecton;
            FormNavigatorControl.ShowToolTips = true;
            // 
            // TableLayoutControl
            //
            TableLayoutControl.Controls.Add(TableGridControl);
            TableLayoutControl.Controls.Add(TableNavigatorControl);
            TableLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutControl.Root = TableLayoutGroup;
            TableLayoutControl.Images = Icons;
            // 
            // TableLayoutGroup
            //
            TableLayoutGroup.CustomizationFormText = "Основная группа";
            TableLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            TableLayoutGroup.GroupBordersVisible = false;
            TableLayoutGroup.TextVisible = false;
            TableLayoutGroup.Items.AddRange(new BaseLayoutItem[] { TableNavigatorLayoutItem, TableGridLayoutItem });
            // 
            // TableNavigatorLayoutItem
            //
            TableNavigatorLayoutItem.Control = TableNavigatorControl;
            TableNavigatorLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            TableNavigatorLayoutItem.CustomizationFormText = "Панель навигации";
            TableNavigatorLayoutItem.Size = new Size(1, 1);
            TableNavigatorLayoutItem.TextVisible = false;
            // 
            // TableNavigatorControl
            // 
            TableNavigatorControl.DataSource = BaseXPCollecton;
            TableNavigatorControl.ShowToolTips = true;
            // 
            // TableGridLayoutItem
            //
            TableGridLayoutItem.Control = TableGridControl;
            TableGridLayoutItem.CustomizationFormText = "Таблица";
            TableGridLayoutItem.Size = new Size(1, 1);
            TableGridLayoutItem.TextVisible = false;
            // 
            // Icons
            //
            Icons.Changed += Icons_Changed;
            // 
            // TableGridControl
            // 
            TableGridControl.DataSource = BaseXPCollecton;
            TableGridControl.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            TableGridControl.EmbeddedNavigator.Buttons.Edit.Visible = false;
            TableGridControl.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            TableGridControl.MainView = TableGridView;
            // 
            // TableGridView
            // 
            TableGridView.Bands.AddRange(new GridBand[] { TableGridBand });
            TableGridView.GridControl = TableGridControl;
            TableGridView.OptionsBehavior.AutoPopulateColumns = false;
            TableGridView.OptionsView.ShowAutoFilterRow = true;
            TableGridView.OptionsView.ShowGroupPanel = false;
            TableGridView.OptionsDetail.AllowExpandEmptyDetails = true;
            TableGridView.Images = Icons;
            TableGridView.Appearance.FocusedRow.BackColor = Color.FromArgb(60, 0, 0, 240);
            TableGridView.Appearance.FocusedRow.Options.UseBackColor = true;
            // 
            // TableGridBand
            // 
            TableGridBand.Caption = "Основные данные";
            // 
            // this
            // 
            Controls.Add(DockPanelMerge);
            XPCollections.Add(BaseXPCollecton);
            GroupsBackColor = Color.Gainsboro;
            ReadOnlyBackColor = Color.Gainsboro;

            // Finish actions
            FormDockPanel.ResumeLayout(false);
            FormDockContainer.ResumeLayout(false);
            TableDockPanel.ResumeLayout(false);
            TableDockContainer.ResumeLayout(false);
            DockPanelMerge.ResumeLayout(false);
            ((ISupportInitialize)(BaseXPCollecton)).EndInit();
            ((ISupportInitialize)(BaseUnitOfWork)).EndInit();
            ((ISupportInitialize)(DockManager)).EndInit();
            ((ISupportInitialize)(FormLayoutControl)).EndInit();
            ((ISupportInitialize)(FormLayoutGroup)).EndInit();
            ((ISupportInitialize)(FormDataLayoutGroup)).EndInit();
            ((ISupportInitialize)(FormNavigatorLayoutItem)).EndInit();
            ((ISupportInitialize)(TableLayoutControl)).EndInit();
            ((ISupportInitialize)(TableLayoutGroup)).EndInit();
            ((ISupportInitialize)(TableGridControl)).EndInit();
            ((ISupportInitialize)(TableGridView)).EndInit();
            ((ISupportInitialize)(TableGridLayoutItem)).EndInit();
            ((ISupportInitialize)(TableNavigatorLayoutItem)).EndInit();
            ((ISupportInitialize)(Icons)).EndInit();

            transaction.Commit();
        }

        /// <summary>
        /// Перезагрузка всех объектов XPCollection с обновлением таблицы.
        /// </summary>
        public void ReloadTablesEx()
        {
            // Очистка кэша для овновления данных.
            if (BaseXPCollecton is XPCollection)
                ((XPCollection)BaseXPCollecton).Session.DropIdentityMap();

            XPCollections.Reload();

            // Развенуть таблицу.
            if (TableGridView != null)
                ExpandDatails(TableGridView, MasterRowInfos);
        }

        /// <summary>
        /// Удаление всех записей для связываения визуальных объектов.
        /// </summary>
        public void DestroyItems()
        {
            Collection<DBInterfaceItemBase> itemsInt = new Collection<DBInterfaceItemBase>();
            foreach (DBInterfaceItemBase item in Items)
                itemsInt.Add(item);

            foreach (DBInterfaceItemBase item in itemsInt)
                DestroyInstance(item);

            InvokeItemsChanged(this, null);
        }

        /// <summary>
        /// Удаление компонента и других связанных с ним компонентов.
        /// </summary>
        public void DestroyVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("DestroyVisibleComponent");

            DestroyItems();

            HostComponent.DestroyComponent(host, BaseXPCollecton);
            HostComponent.DestroyComponent(host, BaseUnitOfWork);
            HostComponent.DestroyComponent(host, Icons);   

            foreach (XPCollection xpcItem in XPCollections)
                HostComponent.DestroyComponent(host, xpcItem);

             transaction.Commit();
        }

        /// <summary>
        /// Вычисление новых координат положения элеметов разметки.
        /// </summary>
        private static Point GetNewLocation(LayoutControlGroup group)
        {
            return new Point(24 * (group.Items.Count + 1), 24 * (group.Items.Count + 1));
        }

        /// <summary>
        /// Выравнивание длины столбцов.
        /// </summary>
        private void FixColumnWidth()
        {
            if (TableGridBand != null)
                foreach (BandedGridColumn column in TableGridBand.Columns)
                    column.Width = TableGridBand.Width / TableGridBand.Columns.Count;
        }

        /// <summary>
        /// Создание визуальных элементов.
        /// </summary>
        private Control CreateItemVisibleComponents(DBInterfaceItemBase item, IDesignerHost host, string postfix = "", Type typeComponent = null)
        {
            Control formEdit = null;

            string name = item.ControlName;

            if (item.Parent == null)
            {
                item.FormLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), string.Format("{0}LayoutItem", name));
                ((ISupportInitialize)(item.FormLayoutItem)).BeginInit();
                item.FormLayoutItem.CustomizationFormText = item.Caption;
                item.FormLayoutItem.Text = item.Caption + ":";
                item.FormLayoutItem.Size = new Size(1, 1);
                item.FormLayoutItem.Location = GetNewLocation(FormDataLayoutGroup);
                ((ISupportInitialize)(item.FormLayoutItem)).EndInit();

                if (!(item is DBInterfaceItemXPCollection))
                {
                    FormDataLayoutGroup.Items.AddRange(new BaseLayoutItem[] { item.FormLayoutItem });
                    item.TableGridColumn = (BandedGridColumn)HostComponent.CreateComponent(host, typeof(BandedGridColumn), string.Format("{0}GridColumn", name));
                    item.TableGridColumn.VisibleIndex = TableGridView.Columns.Count;
                    TableGridView.Columns.AddRange(new BandedGridColumn[] { (BandedGridColumn)item.TableGridColumn });
                    TableGridBand.Columns.Add((BandedGridColumn)item.TableGridColumn);
                    FixColumnWidth();
                }
                else
                {
                    DBInterfaceItemXPCollection xpcItem = (DBInterfaceItemXPCollection)item;
                    xpcItem.FormLayoutGroup = (LayoutControlGroup)HostComponent.CreateComponent(host, typeof(LayoutControlGroup), string.Format("{0}LayoutGroup", name));
                    xpcItem.FormLayoutGroup.CustomizationFormText = item.Caption;
                    xpcItem.FormLayoutGroup.Text = item.Caption;
                    xpcItem.FormLayoutGroup.Items.AddRange(new BaseLayoutItem[] { item.FormLayoutItem });
                    xpcItem.FormLayoutGroup.AppearanceGroup.BackColor = GroupsBackColor;
                    xpcItem.FormLayoutGroup.Location = new Point(0, FormLayoutGroup.Height);
                    FormLayoutGroup.Items.AddRange(new BaseLayoutItem[] { xpcItem.FormLayoutGroup });
                }

                formEdit = (Control)HostComponent.CreateComponent(host, typeComponent, string.Format("{0}{1}", name, postfix));

                item.FormEdit = formEdit;

                FormLayoutControl.Controls.Add(formEdit);

                item.FormLayoutItem.Control = formEdit;
                if (item is DBInterfaceItemBoolean || item is DBInterfaceItemXPCollection)
                    item.FormLayoutItem.TextVisible = false;

                if (item is DBInterfaceItemDateTime)
                {
                    DateEditEx dateEdit = item.FormEdit as DateEditEx;
                    if (dateEdit != null)
                    {
                        dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
                        dateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });
                        dateEdit.StyleController = FormLayoutControl;
                    }
                }
            }
            else
            {
                if (item.Parent is DBInterfaceItemXPComplex && !(item is DBInterfaceItemXPCollection))
                {
                    DBInterfaceItemXPComplex xpcItemParent = item.Parent as DBInterfaceItemXPComplex;

                    if (xpcItemParent.FormGridView != null)
                    {
                        item.FormGridColumn = (GridColumn)HostComponent.CreateComponent(host, typeof(GridColumn), string.Format("{0}GridColumn", name));
                        item.FormGridColumn.FieldName = item.FieldName;
                        item.FormGridColumn.Caption = item.Caption;
                        item.FormGridColumn.VisibleIndex = TableGridView.Columns.Count;

                        xpcItemParent.FormGridView.Columns.AddRange(new GridColumn[] { item.FormGridColumn });
                    }

                    if (xpcItemParent.TableGridView != null)
                    {
                        item.TableGridColumn = (GridColumn)HostComponent.CreateComponent(host, typeof(GridColumn), string.Format("{0}GridColumn", name));
                        item.TableGridColumn.FieldName = item.FieldName;
                        item.TableGridColumn.Caption = item.Caption;
                        item.TableGridColumn.VisibleIndex = TableGridView.Columns.Count;

                        xpcItemParent.TableGridView.Columns.AddRange(new GridColumn[] { item.TableGridColumn });
                    }
                }
            }

            if (item is DBInterfaceItemDateTime)
            {
                DBInterfaceItemDateTime dtItem = item as DBInterfaceItemDateTime;
                dtItem.RepositoryItemDateEdit = (RepositoryItemDateEditEx)HostComponent.CreateComponent(host, typeof(RepositoryItemDateEditEx), string.Format("{0}RepositoryItemDateEdit", name));
                dtItem.RepositoryItemDateEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
                dtItem.RepositoryItemDateEdit.VistaTimeProperties.Buttons.AddRange(new EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });

                if (item.FormGridColumn != null)
                {
                    DBInterfaceItemXPComplex xpcItemTop = (DBInterfaceItemXPComplex)item.GetTopParent();
                    if (xpcItemTop is DBInterfaceItemXPCollection)
                    {
                        GridControlEx gridForm = (GridControlEx)xpcItemTop.FormEdit;
                        gridForm.RepositoryItems.AddRange(new RepositoryItem[] { dtItem.RepositoryItemDateEdit });
                    }
                    if (xpcItemTop is DBInterfaceItemXPObject)
                    {
                        GridLookUpEdit gluEdit = (GridLookUpEdit)xpcItemTop.FormEdit;
                        gluEdit.Properties.RepositoryItems.AddRange(new RepositoryItem[] { dtItem.RepositoryItemDateEdit });
                    }
                    item.FormGridColumn.ColumnEdit = dtItem.RepositoryItemDateEdit;
                }

                if (item.TableGridColumn != null)
                {
                    TableGridControl.RepositoryItems.AddRange(new RepositoryItem[] { dtItem.RepositoryItemDateEdit });
                    dtItem.TableGridColumn.ColumnEdit = dtItem.RepositoryItemDateEdit;
                }
            }

            return formEdit;
        }

        /// <summary>
        /// Создание объета связки визуальных объектов.
        /// </summary>
        public IComponent CreateComponent(Type type, string name)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            return HostComponent.CreateComponent(host, type, name);
        }

        /// <summary>
        /// Удаление объект связки визуальных обеъктов.
        /// </summary>
        public void DestroyItem(DBInterfaceItemBase dbItem)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            HostComponent.DestroyComponent(host, dbItem); 
        }

        /// <summary>
        /// Поиск записи в древовидном объекте.
        /// </summary>
        public static TreeListNode FindNode(TreeList tree, TableMemberInfo info)
        {
            return tree.FindNodeByFieldValue("HashCode", info.HashCode);
        }

        /// <summary>
        /// Создание записи для уже созданных полей таблицы
        /// </summary>
        public static TableMemberInfo PrintExistData(TreeList treeList, DBInterfaceItemBase item, TableMemberInfos infos)
        {
            TableMemberInfo info = new TableMemberInfo() 
            { 
                FieldName = item.FieldName, 
                TypeName = item.TypeName, 
                Caption = item.Caption, 
                ImageIndex = item.ImageIndex, 
                Item = item 
            };

            if (item is DBInterfaceItemXPObject)
                info.IsXPBaseObject = true;
            if (item is DBInterfaceItemXPCollection)
                info.IsXPCollection = true;

            infos.Add(info);

            TreeListNode node = FindNode(treeList, info);
            if (node != null)
            {
                treeList.FocusedNode = node;
                treeList.Selection.Clear();
                treeList.Selection.Add(treeList.FocusedNode);
            }
            return info;
        }

        /// <summary>
        /// Создание иконки записи.
        /// </summary>
        public void CreateIcon(DBInterfaceItemBase item, TableMemberInfo info)
        {
            string name = GetImageFullName(info.ImageName);
            if (!ImageEx.IsExist(Icons, info.ImageName) && File.Exists(name))
            {
                Image image = Image.FromFile(name);
                Icons.Images.Add(image, info.ImageName);
            }

            item.ImageIndex = ImageEx.GetImageIndex(Icons, info.ImageName);
        }

        /// <summary>
        /// Создание элемента коллекции.
        /// </summary>
        public DBInterfaceItemBase CreateInstance(Type type, TableMemberInfo info = null, TableMemberInfo infoSelect = null, TreeList treeList = null, TableMemberInfos MemberExists = null)
        {
            DBInterfaceItemBase item = null;

            if (type == typeof(string) || type == typeof(DBInterfaceItemString))
                item = CreateComponent(typeof(DBInterfaceItemString), "itemString") as DBInterfaceItemString;
            else if (type == typeof(DateTime) || type == typeof(DBInterfaceItemDateTime))
                item = CreateComponent(typeof(DBInterfaceItemDateTime), "itemDateTime") as DBInterfaceItemDateTime;
            else if (type == typeof(short) || type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(object) || type == typeof(DBInterfaceItemNumeric))
                item = CreateComponent(typeof(DBInterfaceItemNumeric), "itemNumeric") as DBInterfaceItemNumeric;
            else if (type == typeof(bool) || type == typeof(DBInterfaceItemBoolean))
                item = CreateComponent(typeof(DBInterfaceItemBoolean), "itemBoolean") as DBInterfaceItemBoolean;
            else if (PropertyInfoEx.isXPBaseObject(type) || type == typeof(DBInterfaceItemXPObject))
            {
                item = CreateComponent(typeof(DBInterfaceItemXPObject), "itemXPObject") as DBInterfaceItemXPObject;
                ((DBInterfaceItemXPObject)item).TableType = type;
                ((DBInterfaceItemXPObject)item).GridViewCaption = type.Name;
            }
            else if (PropertyInfoEx.isXPCollection(type) || type == typeof(DBInterfaceItemXPCollection))
            {
                item = CreateComponent(typeof(DBInterfaceItemXPCollection), "itemXPCollection") as DBInterfaceItemXPCollection;
                if (info != null && info.PropertyTypeCollection != null)
                    ((DBInterfaceItemXPCollection)item).GridViewCaption = info.PropertyTypeCollection.Name;
            }

            if (item != null)
            {
                if (info != null)
                    item.ControlName = info.FieldName;

                if (infoSelect != null)
                {
                    if (infoSelect.Item is DBInterfaceItemXPComplex)
                    {
                        DBInterfaceItemXPComplex xpcItem = (DBInterfaceItemXPComplex)infoSelect.Item;
                        xpcItem.Items.Add(item);
                        if (treeList != null)
                            PrintExistData(treeList, item, infoSelect.Items);
                    }
                    else
                    {
                        infoSelect.Item.Owner.Add(item);
                        if (infoSelect.Parent != null)
                        {
                            if (treeList != null)
                                PrintExistData(treeList, item, infoSelect.Parent.Items);
                        }
                        else
                            if (treeList != null && MemberExists != null)
                                PrintExistData(treeList, item, MemberExists);
                    }
                }
                else
                {
                    Items.Add(item);
                    if (treeList != null && MemberExists != null)
                        PrintExistData(treeList, item, MemberExists);
                }

                CreateControl(item);
                ItemsSeq.Add(item);

                if (info != null)
                {
                    item.FieldName = info.FieldName;
                    item.Caption = info.Caption;
                    if (info.Caption == "")
                        item.Caption = item.FieldName;
                    info.IsUsed = true;
                    if (item is DBInterfaceItemXPCollection)
                        ((DBInterfaceItemXPCollection)item).DataMember = info.PropertyTypeCollection.Name;

                    CreateIcon(item, info);
                }
            }
            return item;
        }

        /// <summary>
        /// Создание всех элементов для данного типа поля таблицы БД.
        /// </summary>
        public void CreateControl(DBInterfaceItemBase item)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("AddControl");

            item.DBInterface = this;

            ((ISupportInitialize)(FormLayoutControl)).BeginInit();
            ((ISupportInitialize)(FormLayoutGroup)).BeginInit();

            if (item is DBInterfaceItemString)
            {
                CreateItemVisibleComponents(item, host, "TextEdit", typeof(TextEdit));
            }
            else if (item is DBInterfaceItemDateTime)
            {
                CreateItemVisibleComponents(item, host, "DateEditEx", typeof(DateEditEx));
            }
            else if (item is DBInterfaceItemNumeric)
            {
                CreateItemVisibleComponents(item, host, "SpinEdit", typeof(SpinEdit));
            }
            else if (item is DBInterfaceItemBoolean)
            {
                CreateItemVisibleComponents(item, host, "CheckEdit", typeof(CheckEdit));
            }
            else if (item is DBInterfaceItemXPObject)
            {
                DBInterfaceItemXPObject xpoItem = item as DBInterfaceItemXPObject;
                if (item.Parent == null)
                {
                    CreateItemVisibleComponents(item, host, "GridLookUpEdit", typeof(GridLookUpEdit));

                    // Form
                    xpoItem.FormGridView = (GridView)HostComponent.CreateComponent(host, typeof(GridView), string.Format("{0}GridView", xpoItem.GridViewCaption));
                    xpoItem.FormGridView.OptionsBehavior.AutoPopulateColumns = false;
                    xpoItem.FormGridView.OptionsView.ShowAutoFilterRow = true;
                    xpoItem.FormGridView.OptionsView.ShowGroupPanel = false;
                    xpoItem.FormGridView.OptionsDetail.AllowExpandEmptyDetails = true;
                    xpoItem.FormGridView.Images = Icons;
                    ((GridLookUpEdit)xpoItem.FormEdit).Properties.View = xpoItem.FormGridView;
                    ((GridLookUpEdit)xpoItem.FormEdit).Properties.NullText = "";
                }
                else
                    if (item.Parent is DBInterfaceItemXPComplex)
                    {
                        CreateItemVisibleComponents(item, host);
                        xpoItem.FormEdit = (RepositoryItemGridLookUpEdit)HostComponent.CreateComponent(host, typeof(RepositoryItemGridLookUpEdit), string.Format("{0}RepGridLookUpEdit", xpoItem.TableType.Name));
                        xpoItem.FormGridView = (GridView)HostComponent.CreateComponent(host, typeof(GridView), string.Format("{0}GridView", xpoItem.GridViewCaption));
                        xpoItem.FormGridView.OptionsBehavior.AutoPopulateColumns = false;
                        xpoItem.FormGridView.OptionsView.ShowAutoFilterRow = true;
                        xpoItem.FormGridView.OptionsView.ShowGroupPanel = false;
                        xpoItem.FormGridView.OptionsDetail.AllowExpandEmptyDetails = true;
                        xpoItem.FormGridView.Images = Icons;
                        ((RepositoryItemGridLookUpEdit)xpoItem.FormEdit).View = xpoItem.FormGridView;
                        ((RepositoryItemGridLookUpEdit)xpoItem.FormEdit).NullText = "";

                        xpoItem.FormGridColumn.ColumnEdit = (RepositoryItemGridLookUpEdit)xpoItem.FormEdit;

                        DBInterfaceItemXPCollection xpcItemTop = (DBInterfaceItemXPCollection)item.GetTopParent();
                        GridControlEx gridForm = (GridControlEx)xpcItemTop.FormEdit;
                        gridForm.RepositoryItems.Add((RepositoryItemGridLookUpEdit)xpoItem.FormEdit);
                    }

                // Table
                xpoItem.TableRepositoryEdit = (RepositoryItemGridLookUpEdit)HostComponent.CreateComponent(host, typeof(RepositoryItemGridLookUpEdit), string.Format("{0}RepGridLookUpEdit", xpoItem.TableType.Name));
                xpoItem.TableGridView = (GridView)HostComponent.CreateComponent(host, typeof(GridView), string.Format("{0}GridView", xpoItem.GridViewCaption));
                xpoItem.TableGridView.OptionsBehavior.AutoPopulateColumns = false;
                xpoItem.TableGridView.OptionsView.ShowAutoFilterRow = true;
                xpoItem.TableGridView.OptionsView.ShowGroupPanel = false;
                xpoItem.TableGridView.OptionsDetail.AllowExpandEmptyDetails = true;
                xpoItem.TableGridView.Images = Icons;
                xpoItem.TableRepositoryEdit.View = xpoItem.TableGridView;
                xpoItem.TableRepositoryEdit.NullText = "";

                xpoItem.TableGridColumn.ColumnEdit = xpoItem.TableRepositoryEdit;

                TableGridControl.RepositoryItems.Add(xpoItem.TableRepositoryEdit);

                // Common
                xpoItem.ValueMember = "This";
                xpoItem.DisplayMember = "DisplayMember";
                //xpoItem.DisplayMember = DBAttribute.GetDisplayMember(xpoItem.TableType);

                if (XPCollectionNeed)
                    CreateXPCollection(host, xpoItem);
            }
            else if (item is DBInterfaceItemXPCollection)
            {
                DBInterfaceItemXPCollection xpcItem = item as DBInterfaceItemXPCollection;

                CreateItemVisibleComponents(item, host, "GridControl", typeof(GridControlEx));

                // Form
                xpcItem.FormGridView = (GridView)HostComponent.CreateComponent(host, typeof(GridView), string.Format("{0}GridView", xpcItem.GridViewCaption));
                xpcItem.FormGridView.OptionsBehavior.AutoPopulateColumns = false;
                //gridViewBase.OptionsView.ShowAutoFilterRow = true;
                xpcItem.FormGridView.OptionsView.ShowGroupPanel = false;
                xpcItem.FormGridView.OptionsDetail.AllowExpandEmptyDetails = true;
                xpcItem.FormGridView.Images = Icons;
                DBInterfaceItemXPCollection xpcItemTop = (DBInterfaceItemXPCollection)xpcItem.GetTopParent();
                GridControlEx gridForm = (GridControlEx)xpcItemTop.FormEdit;
                if (item.Parent == null)
                {
                    gridForm.MainView = xpcItem.FormGridView;
                    gridForm.DataSource = BaseXPCollecton;
                }
                else if (item.Parent is DBInterfaceItemXPCollection)
                {
                    xpcItem.FormLevelNode = new GridLevelNode();
                    xpcItem.FormLevelNode.LevelTemplate = xpcItem.FormGridView;

                    if (item.Parent.Parent == null)
                    {
                        gridForm.LevelTree.Nodes.Add(xpcItem.FormLevelNode);
                    }
                    else if (item.Parent.Parent is DBInterfaceItemXPCollection)
                    {
                        DBInterfaceItemXPCollection xpcItemParent = (DBInterfaceItemXPCollection)item.Parent;
                        xpcItemParent.FormLevelNode.Nodes.Add(xpcItem.FormLevelNode);
                    }

                    gridForm.ViewCollection.Add(xpcItem.FormGridView);
                }

                // Table
                xpcItem.TableGridView = (GridView)HostComponent.CreateComponent(host, typeof(GridView), string.Format("{0}GridView", xpcItem.GridViewCaption));
                xpcItem.TableGridView.OptionsBehavior.AutoPopulateColumns = false;
                xpcItem.TableGridView.OptionsView.ShowGroupPanel = false;
                xpcItem.TableGridView.OptionsDetail.AllowExpandEmptyDetails = true;
                xpcItem.TableGridView.Images = Icons;
                xpcItem.TableLevelNode = new GridLevelNode();
                xpcItem.TableLevelNode.LevelTemplate = xpcItem.TableGridView;

                if (item.Parent == null)
                {
                    TableGridControl.LevelTree.Nodes.Add(xpcItem.TableLevelNode);
                }
                else if (item.Parent is DBInterfaceItemXPCollection)
                {
                    DBInterfaceItemXPCollection xpcItemParent = (DBInterfaceItemXPCollection)item.Parent;
                    xpcItemParent.TableLevelNode.Nodes.Add(xpcItem.TableLevelNode);
                }

                TableGridControl.ViewCollection.Add(xpcItem.TableGridView);
            }

            ((ISupportInitialize)(FormLayoutGroup)).EndInit();
            ((ISupportInitialize)(FormLayoutControl)).EndInit();

            transaction.Commit();
        }

        /// <summary>
        /// Поиск подходящей или создание новой коллекции данных.
        /// </summary>
        private XPCollection CreateXPCollection(IDesignerHost host, DBInterfaceItemXPObject item)
        {
            XPCollection collection = XPCollections.IsExistXPCollection(item.TableType);
            if (collection == null)
            {
                collection = (XPCollection)HostComponent.CreateComponent(host, typeof(XPCollection), string.Format("{0}XPCollection", item.TableType.Name));
                XPCollectionContainer.InitXPCollection(collection, item.TableType, BaseUnitOfWork);
            }
            item.DataSource = collection;
            XPCollections.Add(collection);

            return collection;
        }

        /// <summary>
        /// Рекурсивное удалание дочерних элементов.
        /// </summary>
        private void DestroyChildControl(DBInterfaceItemXPComplex item, IDesignerHost host, int level)
        {
            foreach (DBInterfaceItemBase itemIter in item.Items)
            {
                DestroyControl(itemIter);
                if (itemIter is DBInterfaceItemXPComplex)
                {
                    DBInterfaceItemXPComplex xpItemIter = (DBInterfaceItemXPComplex)itemIter;
                    DestroyChildControl(xpItemIter, host, level + 1);
                }
            }
        }

        /// <summary>
        /// Очистка записи коллекции.
        /// </summary>
        private void EraseItem(DBInterfaceItemBase item)
        {
            if (item.Owner != null)
                item.Owner.Remove(item);
            else
                Items.Remove(item);
            ItemsSeq.Remove(item);
            DestroyItem(item);
        }

        /// <summary>
        /// Очистка всех записей коллекции.
        /// </summary>
        private void EraseItems(DBInterfaceItemBases itemDestroys)
        {
            foreach (DBInterfaceItemBase item in itemDestroys)
                EraseItem(item);
        }

        /// <summary>
        /// Удание элемента.
        /// </summary>
        private void DestroyControl(DBInterfaceItemBase item)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("DeleteControl");

            HostComponent.DestroyComponent(host, item.FormEdit);
            HostComponent.DestroyComponent(host, item.FormLayoutItem);
            HostComponent.DestroyComponent(host, item.FormGridColumn);
            HostComponent.DestroyComponent(host, item.TableGridColumn);

            if (item is DBInterfaceItemDateTime)
            {
                DBInterfaceItemDateTime dtItem = (DBInterfaceItemDateTime)item;
                HostComponent.DestroyComponent(host, dtItem.RepositoryItemDateEdit);
            }
            else if (item is DBInterfaceItemXPObject)
            {
                DBInterfaceItemXPObject xpoItem = (DBInterfaceItemXPObject)item;
                HostComponent.DestroyComponent(host, xpoItem.FormGridView);
                HostComponent.DestroyComponent(host, xpoItem.TableGridView);
                HostComponent.DestroyComponent(host, xpoItem.TableRepositoryEdit);
            }
            else if (item is DBInterfaceItemXPCollection)
            {
                DBInterfaceItemXPCollection xpcItem = (DBInterfaceItemXPCollection)item;
                HostComponent.DestroyComponent(host, xpcItem.FormGridView);
                HostComponent.DestroyComponent(host, xpcItem.TableGridView);
                HostComponent.DestroyComponent(host, xpcItem.FormLayoutGroup);

                if (xpcItem.FormLevelNode != null)
                    xpcItem.FormLevelNode.Dispose();

                if (xpcItem.TableLevelNode != null)
                    xpcItem.TableLevelNode.Dispose();
            }

            if (item is DBInterfaceItemXPComplex)
                DestroyChildControl((DBInterfaceItemXPComplex)item, host, 0);

            ItemDestroys.Add(item);

            if (item.Parent == null)
                FixColumnWidth();

            transaction.Commit();
        }

        /// <summary>
        /// Удание записи.
        /// </summary>
        public void DestroyInstance(DBInterfaceItemBase item)
        {
            ItemDestroys.Clear();

            DestroyControl(item);

            EraseItems(ItemDestroys);

            if (item.Owner != null)
                item.Owner.Remove(item);
            if (item is IDisposable)
                ((IDisposable)item).Dispose();

            if (item is DBInterfaceItemXPObject)
                DeleteUnuseXPCollection();

            if (item.ImageIndex != -1)
            {
                DeleteUnusedImage(item.ImageIndex);
                ItemsSeq.CorrectImageIndex();
            }

            item = null;
        }

        /// <summary>
        /// Удаление неиспользуемых XPCollection.
        /// </summary>
        public void DeleteUnuseXPCollection()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            XPCollectionContainer unuses = XPCollections.FindUnuseCollections(ItemsSeq, BaseXPCollecton);
            foreach (XPCollection unuse in unuses)
            {
                XPCollections.Remove(unuse);
                HostComponent.DestroyComponent(host, (Component)unuse);
            }
        }

        /// <summary>
        /// Удаление неиспользуемых иконок из коллекции.
        /// </summary>
        private void DeleteUnusedImage(int index)
        {
            if (index != -1 && !IsUsedImageIndex(index))
                Icons.Images.RemoveAt(index);
        }

        /// <summary>
        /// Используется ли индекс иконки кем-нибудь из элементов.
        /// </summary>
        private bool IsUsedImageIndex(int index)
        {
            foreach (DBInterfaceItemBase item in ItemsSeq)
                if (item.ImageIndex == index)
                    return true;
            return false;
        }

        /// <summary>
        /// Возврат полного пути к иконке.
        /// </summary>
        public string GetImageFullName(string name)
        {
            return String.Format("{0}\\{1}", ImagePath, name);
        }

        /// <summary>
        /// Обновление дизайнера.
        /// </summary>
        public void InvokeItemsChanged(object sender, EventArgs e)
        {
            if (ItemsChanged != null)
                ItemsChanged(sender, e);
        }

        /// <summary>
        /// Обновление кода дизайна.
        /// </summary>
        public void RefreshDesignCode()
        {
            if (ItemsSeq.Count > 0)
            {
                string temp = ItemsSeq[0].Caption;
                ItemsSeq[0].Caption += "t";
                ItemsSeq[0].Caption = temp;
            }
        }

        /// <summary>
        /// Обновние типа таблицы за счет пересоздания xpcBase.
        /// </summary>
        private void UpdateTypeXPCollection(Type newType)
        {
            if (TableType != newType)
            {
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

                // Удалиние компонента.
                HostComponent.DestroyComponent(host, BaseXPCollecton);

                // Удаление всех элементов.
                DestroyItems();

                if (XPCollectionNeed)
                {
                    if (BaseXPCollecton is XPCollection)
                    {
                        XPCollection xpCollection = BaseXPCollecton as XPCollection;
                        // Создание нового компонента.
                        xpCollection = (XPCollection)HostComponent.CreateComponent(host, typeof(XPCollection), "baseXPCollecton");
                        XPCollectionContainer.InitXPCollection(xpCollection, newType, BaseUnitOfWork);
                    }
                    else if (BaseXPCollecton is XPServerCollectionSource)
                    {
                        XPServerCollectionSource svCollection = BaseXPCollecton as XPServerCollectionSource;
                        // Создание нового компонента.
                        svCollection = (XPServerCollectionSource)HostComponent.CreateComponent(host, typeof(XPServerCollectionSource), "baseServerCollecton");
                        XPCollectionContainer.InitSVCollection(svCollection, newType, BaseUnitOfWork);
                    }

                    // Добавление в список.
                    XPCollections.Insert(0, BaseXPCollecton);

                    // Привязка к интерфейсу
                    SetBaseXPCollection(BaseXPCollecton, BaseUnitOfWork);
                }
            }
        }

        /// <summary>
        /// Показы дизайнера компонента.
        /// </summary>
        public void ShowDesigner()
        {
            DBInterfaceDesignerForm designerForm = new DBInterfaceDesignerForm(this);
            DialogResult result;
            IUIService service = GetService(typeof(IUIService)) as IUIService;
            if (service != null)
                result = service.ShowDialog(designerForm);
            else
                result = DialogResult.OK;
        }

        public void AddFindPanelToEverything()
        {
            int count = 0;
            if (TableGridView != null)
            {
                TableGridView.OptionsFind.AlwaysVisible = true;
                count++;
            }

            foreach (var item in ItemsSeq)
            {
                DBInterfaceItemXPComplex itemComplex = item as DBInterfaceItemXPComplex;
                if (itemComplex != null)
                {
                    itemComplex.TableGridView.OptionsFind.AlwaysVisible = true;
                    count++;
                }
            }

            XtraMessageBox.Show(
                $"Панель поиска была доблена в {count} GridControl",
                "Информация",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Вызов события при нажатии правой кнопки мышки по выпадающему списку.
        /// </summary>
        public void InvokeRightMouseDown(object sender, XPObjectEventArgs e)
        {
            if (RightMouseDown != null)
                RightMouseDown(sender, e);
        }

        /// <summary>
        /// Проверка наличия изменений в данных.
        /// </summary>
        public AllowSwitchMessage GetAllowSwitch()
        {
            AllowSwitchMessage allowResult = new AllowSwitchMessage(true, false);

            // Необходимо переводить фокус элементов, где происходит ввод данных, на какой-нибудь объект,
            // иначе не будут видны изменения в таблице БД.
            DataNavigatorEx navigator = null;

            if (FormNavigatorControl != null)
            {
                FormNavigatorControl.Focus();
                navigator = FormNavigatorControl;
            }
            else if (TableNavigatorControl != null)
            {
                TableNavigatorControl.Focus();
                navigator = TableNavigatorControl;
            }

            if (BaseXPCollecton is XPCollection)
            {
                XPCollection xpCollection = BaseXPCollecton as XPCollection;
                int recordCount = xpCollection.Count;
                if (recordCount > 0 && navigator != null)
                {
                    int saveCount = (xpCollection.Session.GetObjectsToSave()).Count;
                    int deleteCount = (xpCollection.Session.GetObjectsToDelete()).Count;
                    if (saveCount > 0 || deleteCount > 0)
                    {
                        DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(
                            string.Format("Хотите сохранить измениния в таблице \"{0}\" ?", TableCaption),
                            "Найдены изменения",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        allowResult.IsMessage = true;
                        if (result == DialogResult.Yes)
                        {
                            navigator.DataNavigatorEx_ButtonClick(null, new NavigatorButtonClickEventArgs(navigator.Buttons.EndEdit));
                            allowResult.IsAllow = !navigator.Error;
                        }
                        else
                        {
                            ReloadTablesEx();
                        }
                    }
                }
            }
            return allowResult;
        }

        /// <summary>
        /// Сохранение данных в БД.
        /// </summary>
        public void SaveData()
        {
            DataNavigatorEx dataNavigator = null;

            if (DockPanelMerge.ActiveChild == FormDockPanel)
            {
                FormNavigatorControl.Focus();
                dataNavigator = FormNavigatorControl;
            }
            else
            {
                TableNavigatorControl.Focus();
                dataNavigator = TableNavigatorControl;
            }

            if (dataNavigator != null)
                dataNavigator.DataNavigatorEx_ButtonClick(null, new NavigatorButtonClickEventArgs(dataNavigator.Buttons.EndEdit));
        }

        /// <summary>
        /// Загрузка данных из БД.
        /// </summary>
        public void LoadData()
        {
            DataNavigatorEx dataNavigator = null;

            if (DockPanelMerge.ActiveChild == FormDockPanel)
            {
                FormNavigatorControl.Focus();
                dataNavigator = FormNavigatorControl;
            }
            else
            {
                TableNavigatorControl.Focus();
                dataNavigator = TableNavigatorControl;
            }

            if (dataNavigator != null)
                dataNavigator.DataNavigatorEx_ButtonClick(null, new NavigatorButtonClickEventArgs(dataNavigator.Buttons.CancelEdit));
        }

        /// <summary>
        /// Привязывание элементов интерфейса к новому основному источнику данных.
        /// </summary>
        public void SetBaseXPCollection(Component collection, UnitOfWork unit = null)
        {
            BaseXPCollecton = collection;
            if (unit != null)
                BaseUnitOfWork = unit;
            FormNavigatorControl.DataSource = collection;
            TableNavigatorControl.DataSource = collection;
            TableGridControl.DataSource = collection;
            Items.SetDataBinding(collection);
            XPCollections.Insert(0, collection);
        }

        /// <summary>
        /// Привязывание элементов интерфейса к нового основному источнику данных.
        /// </summary>
        public void SetBaseXPCollection(XPCollectionWithUnit collectionWithUnit)
        {
            SetBaseXPCollection(collectionWithUnit.Collection, collectionWithUnit.UnitOfWork);
        }

        /// <summary>
        /// Привязывание элементов интерфейса к нового основному источнику данных.
        /// </summary>
        public void SetXPCollection(XPCollection collection)
        {
            foreach (DBInterfaceItemBase item in ItemsSeq)
                if (item is DBInterfaceItemXPObject)
                {
                    DBInterfaceItemXPObject xpoItem = item as DBInterfaceItemXPObject;

                    if (xpoItem.TableType == collection.ObjectType)
                    {
                        xpoItem.DataSource = collection;
                        XPCollections.Add(collection);
                    }
                }
        }

        /// <summary>
        /// Привзка всех возможных коллекций данных к элементам интерфейса.
        /// </summary>
        public void SetXPCollectionSmart(XPCollectionWithUnits collections)
        {
            XPCollectionWithUnit baseCollection = null;
            foreach (XPCollectionWithUnit collection in collections)
                if (TableType == collection.Collection.ObjectType)
                {
                    SetBaseXPCollection(collection);
                    baseCollection = collection;
                    break;
                }

            if (baseCollection != null)
                foreach (DBInterfaceItemBase item in ItemsSeq)
                    if (item is DBInterfaceItemXPObject)
                    {
                        DBInterfaceItemXPObject xpoItem = (DBInterfaceItemXPObject)item;
                        XPCollection xpCollection = XPCollections.IsExistXPCollection(xpoItem.TableType);
                        if (xpCollection == null)
                            xpCollection = new XPCollection(baseCollection.UnitOfWork, xpoItem.TableType);

                        xpoItem.DataSource = xpCollection;

                        if (xpoItem.FormEdit != null && xpoItem.FormEdit is BaseEdit)
                        {
                            BaseEdit baseEdit = xpoItem.FormEdit as BaseEdit;
                            if (baseEdit.DataBindings.Count > 0)
                                baseEdit.DataBindings.Clear();
                            baseEdit.DataBindings.Add(new Binding("EditValue", BaseXPCollecton, xpoItem.FieldNameEnd, true));
                        }
                        XPCollections.Add(xpCollection);
                    }
                    else if (item is DBInterfaceItemXPCollection)
                    {
                        DBInterfaceItemXPCollection xpcItem = (DBInterfaceItemXPCollection)item;
                        xpcItem.DataSource = baseCollection.Collection;
                    }

        }

        #region Position
        /// <summary>
        /// Возвращение текущего объекта формы.
        /// </summary>
        /// <returns></returns>
        public XPBaseObject GetCurrentObject()
        {
            if (BaseXPCollecton is XPCollection)
            {
                XPCollection collection = BaseXPCollecton as XPCollection;
                if (collection.Count > 0)
                    return collection[RecordPosition] as XPBaseObject;
                return
                    null;
            }
            else if (BaseXPCollecton is XPServerCollectionSource)
            {
                XPServerCollectionSource collection = BaseXPCollecton as XPServerCollectionSource;
                IListServer list = (collection as IListSource).GetList() as IListServer;
                if (list != null && 
                    list.Count > 0)
                    return list[RecordPosition] as XPBaseObject;
                return
                    null;
            }
            else
                return null;
        }
        
        /// <summary>
        /// Сохранить текущую позицию объкта.
        /// </summary>
        public void SaveRecordPosition()
        {
            RecordCurrent = GetCurrentObject();
        }

        /// <summary>
        /// Задание сохраненной позиции.
        /// </summary>
        public void LoadRecordPosition()
        {
            if (RecordCurrent != null && 
                !(GetCurrentObject()).Equals(RecordCurrent))
                RecordPosition = XPObjectEx.FindRecordPossition(BaseXPCollecton, RecordCurrent);
        }

        /// <summary>
        /// Фукус на искомой записи
        /// </summary>
        public void FocusRecord(XPBaseObject record)
        {
            RecordPosition = XPObjectEx.FindRecordPossition(BaseXPCollecton, record);
        }
        #endregion

        #region Expanding rows
        /// <summary>
        /// Развернуть все записи.
        /// </summary>
        public void ExpandAllRows(GridView view)
        {
            view.BeginUpdate();
            for (int handle = 0; handle < view.DataRowCount; handle++)
                view.SetMasterRowExpanded(handle, true);
        }

        /// <summary>
        /// Получить спискок развернутых записей.
        /// </summary>
        public void GetListOfExpandDatails(GridView view, List<MasterRowInfo> rowInfos)
        {
            rowInfos.Clear();
            for (int handle = 0; handle < view.DataRowCount; handle++)
                if (view.GetMasterRowExpanded(handle))
                    rowInfos.Add(new MasterRowInfo(handle, view.GetVisibleDetailRelationIndex(handle)));
        }

        /// <summary>
        /// Свернуть все раскрытые записи.
        /// </summary>
        public void CollapseDatails(GridView view, List<MasterRowInfo> rowInfos)
        {
            rowInfos.Clear();
            for (int handle = 0; handle < view.DataRowCount; handle++)
            {
                if (view.GetMasterRowExpanded(handle))
                {
                    rowInfos.Add(new MasterRowInfo(handle, view.GetVisibleDetailRelationIndex(handle)));
                    view.SetMasterRowExpanded(handle, false);
                }
            }
        }

        /// <summary>
        /// Развернуть определенные записи из списка.
        /// </summary>
        public void ExpandDatails(GridView view, List<MasterRowInfo> rowInfos)
        {
            for (int i = 0; i < rowInfos.Count; i++)
                view.SetMasterRowExpandedEx(rowInfos[i].RowHandle, rowInfos[i].RelationIndex, true);
        }

        #endregion

        #endregion

        #region Events handler
        private void DBInterface_Load(object sender, EventArgs e)
        {
            SaveRecordPosition();
            if (DockManager != null)
            {
                DockManager.Docking += DockManager_Docking;
                DockManager.EndDocking += DockManager_EndDocking;
            }
            if (DockPanelMerge != null)
            {
                if (!IsDesignModeEx && IsFirstLoad)
                    if (DockPanelMerge.ActiveChild != TableDockPanel)
                        DockPanelMerge.ActiveChild = TableDockPanel;
            }
            if (FormNavigatorControl != null)
                FormNavigatorControl.PositionChanged += FormNavigatorControl_PositionChanged;
            if (TableNavigatorControl != null)
            {
                TableNavigatorControl.PositionChanged += TableNavigatorControl_PositionChanged;
                if (!IsDesignModeEx && IsFirstLoad)
                    if (BaseXPCollecton is XPCollection)
                        TableNavigatorControl.Position = ((XPCollection)BaseXPCollecton).Count - 1;
            }
            if (FormNavigatorControl != null)
            {
                FormNavigatorControl.DateBaseUpdating += NavigatorControl_DateBaseUpdating;
                FormNavigatorControl.DataBaseSaving += NavigatorControl_DataBaseSaving;
                FormNavigatorControl.DataBaseSaved += NavigatorControl_DataBaseSaved;
            }
            if (TableNavigatorControl != null)
            {
                TableNavigatorControl.DateBaseUpdating += NavigatorControl_DateBaseUpdating;
                TableNavigatorControl.DataBaseSaving += NavigatorControl_DataBaseSaving;
                TableNavigatorControl.DataBaseSaved += NavigatorControl_DataBaseSaved;
            }

            IsFirstLoad = false;

            if (NavigatorControl != null)
            {
                NavigatorControl.PositionChanged += NavigatorControl_PositionChanged;
            }
        }

        public void NavigatorControl_DateBaseUpdating(object sender, EventArgs e)
        {
            // Свернуть таблицу.
            if (TableGridView != null)
            {
                TableGridView.ActiveFilterString = "";
                CollapseDatails(TableGridView, MasterRowInfos);
            }

            ReloadTablesEx();
        }

        void NavigatorControl_DataBaseSaving(object sender, EventArgs e)
        {           
            // Свернуть таблицу.
            if (TableGridView != null)
            {
                TableGridView.ActiveFilterString = "";
                CollapseDatails(TableGridView, MasterRowInfos);
            }

            if (DataBaseSaving != null)
                DataBaseSaving(this, EventArgs.Empty);
        }

        void NavigatorControl_DataBaseSaved(object sender, EventArgs e)
        {
            if (DataBaseSaved != null)
                DataBaseSaved(this, EventArgs.Empty);
        }

        void DockManager_Docking(object sender, DockingEventArgs e)
        {
            if (!Docking)
            {
                SaveRecordPosition();
                Docking = true;
            }
        }

        void DockManager_EndDocking(object sender, EndDockingEventArgs e)
        {
            LoadRecordPosition();
            Docking = false;
        }

        void FormNavigatorControl_PositionChanged(object sender, EventArgs e)
        {
            if (FormNavigatorControl != null && 
                TableNavigatorControl != null &&
                TableNavigatorControl.Position != FormNavigatorControl.Position)
                TableNavigatorControl.Position = FormNavigatorControl.Position;
        }

        void TableNavigatorControl_PositionChanged(object sender, EventArgs e)
        {
            if (FormNavigatorControl != null &&
                TableNavigatorControl != null &&
                TableNavigatorControl.Position != FormNavigatorControl.Position) 
                FormNavigatorControl.Position = TableNavigatorControl.Position;
        }
        void Icons_Changed(object sender, EventArgs e)
        {
            ItemsSeq.CorrectImageIndex();
        }

        private void NavigatorControl_PositionChanged(object sender, EventArgs e)
        {
            XPBaseObject currentObject = GetCurrentObject();
            CurrentObjectChanged(this, new CurrentObjectEventArgs(currentObject));
        }
        #endregion
    }
}
