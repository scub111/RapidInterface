using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using System.ComponentModel.Design;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.Collections.ObjectModel;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.Utils;
using System.IO;

namespace RapidInterface
{
    [DesignerAttribute(typeof(DBFormDesigner))]
    public partial class DBForm : UserControl
    {
        #region Constructos
        public DBForm()
        {
            InitializeComponent();

            _Items = new DBFormItemBases();

            NotifyIconDestroyed = false;

            Load += DBForm_Load;
        }
        #endregion
        
        #region Properties
        /// <summary>
        /// Форма, на которой размещен компонент.
        /// </summary>
        [Category("Visible components")]
        public Form OwnerForm { get; set; }

        /// <summary>
        /// Сервис типов.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITypeDiscoveryService TypeDiscoveryService { get; set; }

        [Category("Visible components")]
        public DockManager BaseDockManager { get; set; }

        [Category("Visible components")]
        public DockPanel BaseDockPanel { get; set; }

        [Category("Visible components")]
        public ControlContainer BaseControlContainer { get; set; }

        [Category("Visible components")]
        public LayoutControlEx BaseLayoutControl { get; set; }

        [Category("Visible components")]
        public LayoutControlGroup BaseLayoutGroup { get; set; }

        [Category("Visible components")]
        public LayoutControlItem FilterLayoutItem { get; set; }

        [Category("Visible components")]
        public LayoutControlItem BarLayoutItem { get; set; }

        [Category("Visible components")]
        public LayoutControlItem FilterClearLayoutItem { get; set; }

        [Category("Visible components")]
        public TextEdit FilterTextEdit { get; set; }

        [Category("Visible components")]
        public SimpleButton FilterClearButton { get; set; }

        [Category("Visible components")]
        public NavBarControl BaseNavBarControl { get; set; }

        [Category("Visible components")]
        public NavBarGroup BaseNavBarGroup { get; set; }

        [Category("Visible components")]
        public DocumentManager BaseDocumentManager { get; set; }

        [Category("Visible components")]
        public TabbedView BaseTabbedView { get; set; }

        DBFormItemBases _Items;
        /// <summary>
        /// Коллекция визиальных элементов интерфейса.
        /// </summary>
        [Editor(typeof(DBFormItemCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Invisible components")]
        public DBFormItemBases Items
        {
            get { return _Items; }
        }

        /// <summary>
        /// Путь к папке, где располагаются в иконки.
        /// </summary>
        [Category("Appearance")]
        public string ImagePath { get; set; }

        /// <summary>
        /// Компонент коллекции иконок.
        /// </summary>
        [Category("Visible components")]
        public ImageCollection Icons { get; set; }

        /// <summary>
        /// Количество раз открытия дизайнера.
        /// </summary>
        public int CountOpenDesigner { get; set; }

        /// <summary>
        /// Прошлая активная форма.
        /// </summary>
        private DBViewBase ViewActiveLast { get; set; }

        /// <summary>
        /// Активный элемент.
        /// </summary>
        private DBFormItemBase ItemActive { get; set; }

        /// <summary>
        /// Удален компонент иконки в трее.
        /// </summary>
        private bool NotifyIconDestroyed;

        /// <summary>
        /// Компонент иконки в трее.
        /// </summary>
        [Category("Visible components")]
        public NotifyIcon NotifyIcon { get; set; }

        /// <summary>
        /// Необходимость иконки в трее.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public bool NotifyIconNeed 
        {
            get
            {
                if (NotifyIcon != null && !NotifyIconDestroyed)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                    NotifyIcon = (NotifyIcon)HostComponent.CreateComponent(host, typeof(NotifyIcon), "notifyIcon");
                    NotifyIcon.Icon = OwnerForm.Icon;
                    NotifyIcon.Text = OwnerForm.Text;
                    NotifyIcon.Visible = true;

                    NotifyIconDestroyed = false;
                }
                else
                {
                    IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                    HostComponent.DestroyComponent(host, NotifyIcon);

                    NotifyIconDestroyed = true;
                }
            }
        }

        /// <summary>
        /// Состояние главной формы перед минимизацией.
        /// </summary>
        private FormWindowState OwnerFormWindowNormal { get; set; }
        #endregion

        #region Events
        public event EventHandler ChangedItems;
        #endregion
        
        #region Metods common
        /// <summary>
        /// Инициализация компонента и создание группы других компонентов.
        /// </summary>
        public void InitializeVisibleComponents(Form form)
        {
            OwnerForm = form;

            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("InitializeDBForm");

            // Создание компонентов.
            BaseDockManager = (DockManager)HostComponent.CreateComponent(host, typeof(DockManager), "baseDockManager");
            BaseDockPanel = (DockPanel)HostComponent.CreateComponent(host, typeof(DockPanel), "baseDockPanel");
            BaseControlContainer = (ControlContainer)HostComponent.CreateComponent(host, typeof(ControlContainer), "baseControlContainer");

            BaseLayoutControl = (LayoutControlEx)HostComponent.CreateComponent(host, typeof(LayoutControlEx), "baseLayoutControl");
            BaseLayoutGroup = (LayoutControlGroup)HostComponent.CreateComponent(host, typeof(LayoutControlGroup), "baseLayoutGroup");
            FilterLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), "filterLayoutItem");
            BarLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), "barLayoutItem");
            FilterClearLayoutItem = (LayoutControlItem)HostComponent.CreateComponent(host, typeof(LayoutControlItem), "filterClearLayoutItem");

            FilterTextEdit = (TextEdit)HostComponent.CreateComponent(host, typeof(TextEdit), "filterTextEdit");
            FilterClearButton = (SimpleButton)HostComponent.CreateComponent(host, typeof(SimpleButton), "filterClearButton");

            BaseNavBarControl = (NavBarControl)HostComponent.CreateComponent(host, typeof(NavBarControl), "baseNavBarControl");
            BaseNavBarGroup = (NavBarGroup)HostComponent.CreateComponent(host, typeof(NavBarGroup), "baseNavBarGroup");

            BaseDocumentManager = (DocumentManager)HostComponent.CreateComponent(host, typeof(DocumentManager), "baseDocumentManager");
            BaseTabbedView = (TabbedView)HostComponent.CreateComponent(host, typeof(TabbedView), "baseTabbedView");

            Icons = (ImageCollection)HostComponent.CreateComponent(host, typeof(ImageCollection), "icons");


            ((ISupportInitialize)(BaseDockManager)).BeginInit();
            BaseDockPanel.SuspendLayout();
            BaseControlContainer.SuspendLayout();
            ((ISupportInitialize)(BaseLayoutControl)).BeginInit();
            BaseLayoutControl.SuspendLayout();
            ((ISupportInitialize)(BaseLayoutGroup)).BeginInit();
            ((ISupportInitialize)(FilterTextEdit.Properties)).BeginInit();
            ((ISupportInitialize)(FilterLayoutItem)).BeginInit();
            ((ISupportInitialize)(BaseNavBarControl)).BeginInit();
            ((ISupportInitialize)(BarLayoutItem)).BeginInit();
            ((ISupportInitialize)(FilterClearLayoutItem)).BeginInit();
            ((ISupportInitialize)(BaseDocumentManager)).BeginInit();
            ((ISupportInitialize)(BaseTabbedView)).BeginInit();
            ((ISupportInitialize)(Icons)).BeginInit();

            // 
            // BaseDockManager
            // 
            BaseDockManager.Form = OwnerForm;
            BaseDockManager.RootPanels.AddRange(new DockPanel[] {
            BaseDockPanel});
            BaseDockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // BaseDockPanel
            // 
            BaseDockPanel.Controls.Add(BaseControlContainer);
            BaseDockPanel.Dock = DockingStyle.Left;
            BaseDockPanel.Location = new Point(0, 0);
            BaseDockPanel.OriginalSize = new Size(200, 200);
            BaseDockPanel.Size = new Size(200, 537);
            BaseDockPanel.Text = "Навигация";
            // 
            // BaseControlContainer
            // 
            BaseControlContainer.Controls.Add(this);
            BaseControlContainer.Location = new Point(4, 23);
            BaseControlContainer.Size = new Size(192, 510);
            BaseControlContainer.TabIndex = 0;

            // 
            // BaseLayoutControl
            // 
            BaseLayoutControl.Controls.Add(FilterClearButton);
            BaseLayoutControl.Controls.Add(BaseNavBarControl);
            BaseLayoutControl.Controls.Add(FilterTextEdit);
            BaseLayoutControl.Dock = DockStyle.Fill;
            BaseLayoutControl.Location = new Point(0, 0);
            BaseLayoutControl.Root = BaseLayoutGroup;
            BaseLayoutControl.Size = new Size(192, 510);
            BaseLayoutControl.TabIndex = 0;
            // 
            // BaseLayoutGroup
            // 
            BaseLayoutGroup.CustomizationFormText = "Формы";
            BaseLayoutGroup.EnableIndentsWithoutBorders = DefaultBoolean.True;
            BaseLayoutGroup.GroupBordersVisible = false;
            BaseLayoutGroup.Items.AddRange(new BaseLayoutItem[] {
            FilterLayoutItem,
            BarLayoutItem,
            FilterClearLayoutItem});
            BaseLayoutGroup.Location = new Point(0, 0);
            BaseLayoutGroup.Size = new Size(192, 510);
            BaseLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            BaseLayoutGroup.TextVisible = false;
            // 
            // FilterClearLayoutItem
            // 
            FilterClearLayoutItem.Control = FilterClearButton;
            FilterClearLayoutItem.CustomizationFormText = "Очистка фильтра";
            FilterClearLayoutItem.Location = new Point(0, 0);
            FilterClearLayoutItem.Size = new Size(26, 26);
            FilterClearLayoutItem.TextSize = new Size(0, 0);
            FilterClearLayoutItem.TextToControlDistance = 0;
            FilterClearLayoutItem.TextVisible = false;
            // 
            // FilterLayoutItem
            // 
            FilterLayoutItem.Control = FilterTextEdit;
            FilterLayoutItem.CustomizationFormText = "Фильтр";
            FilterLayoutItem.Location = new Point(26, 0);
            FilterLayoutItem.Size = new Size(146, 26);
            FilterLayoutItem.TextSize = new Size(0, 0);
            FilterLayoutItem.TextToControlDistance = 0;
            FilterLayoutItem.TextVisible = false;
            // 
            // BarLayoutItem
            // 
            BarLayoutItem.Control = BaseNavBarControl;
            BarLayoutItem.CustomizationFormText = "Навигация";
            BarLayoutItem.Location = new Point(0, 26);
            BarLayoutItem.Size = new Size(172, 464);
            BarLayoutItem.TextSize = new Size(0, 0);
            BarLayoutItem.TextToControlDistance = 0;
            BarLayoutItem.TextVisible = false;

            // 
            // FilterClearButton
            // 
            FilterClearButton.Image = InitIcons.Images[0];
            // 
            // BaseNavBarControl
            // 
            BaseNavBarControl.Dock = DockStyle.Fill;
            //BaseNavBarControl.AllowSelectedLink = true;
            BaseNavBarControl.LinkSelectionMode = LinkSelectionModeType.OneInControl;
            BaseNavBarControl.ActiveGroup = BaseNavBarGroup;
            BaseNavBarControl.SmallImages = Icons;
            BaseNavBarControl.Groups.AddRange(new NavBarGroup[] {
            BaseNavBarGroup});
            // 
            // BaseNavBarGroup
            // 
            BaseNavBarGroup.Caption = "Формы";
            BaseNavBarGroup.Expanded = true;
            // 
            // BaseDocumentManager
            // 
            //documentManager.BarAndDockingController = barAndDockingController;
            BaseDocumentManager.MdiParent = OwnerForm;
            //documentManager.MenuManager = barManager;
            BaseDocumentManager.View = BaseTabbedView;
            BaseDocumentManager.Images = Icons;
            BaseDocumentManager.ViewCollection.AddRange(new BaseView[] {
            BaseTabbedView});
            // 
            // tabbedView
            // 
            //tabbedView.DocumentClosing += new DocumentCancelEventHandler(tabbedView_DocumentClosing);

            // 
            // this
            // 
            Dock = DockStyle.Fill;
            Controls.Add(BaseLayoutControl);

            // 
            // OwnerForm
            // 
            OwnerForm.IsMdiContainer = true;
            OwnerForm.Controls.Add(BaseDockPanel);

            ((ISupportInitialize)(BaseDockManager)).EndInit();
            BaseDockPanel.ResumeLayout(false);
            BaseControlContainer.ResumeLayout(false);
            ((ISupportInitialize)(BaseLayoutControl)).EndInit();
            BaseLayoutControl.ResumeLayout(false);
            ((ISupportInitialize)(BaseLayoutGroup)).EndInit();
            ((ISupportInitialize)(FilterTextEdit.Properties)).EndInit();
            ((ISupportInitialize)(FilterLayoutItem)).EndInit();
            ((ISupportInitialize)(BaseNavBarControl)).EndInit();
            ((ISupportInitialize)(BarLayoutItem)).EndInit();
            ((ISupportInitialize)(FilterClearLayoutItem)).EndInit();
            ((ISupportInitialize)(BaseDocumentManager)).EndInit();
            ((ISupportInitialize)(BaseTabbedView)).EndInit();
            ((ISupportInitialize)(Icons)).EndInit();

            transaction.Commit();
        }

        /// <summary>
        /// Удаление всех записей для связываения визуальных объектов.
        /// </summary>
        public void DestroyItems()
        {
            Collection<DBFormItemBase> itemsInt = new Collection<DBFormItemBase>();
            foreach (DBFormItemBase item in Items)
                itemsInt.Add(item);

            foreach (DBFormItemBase item in itemsInt)
                //HostComponent.DestroyComponent(host, item); 
                DestroyInstance(item);
        }
            
        /// <summary>
        /// Удаление компонента и других связанных с ним компонентов.
        /// </summary>
        public void DestroyVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

            DestroyItems();

            HostComponent.DestroyComponent(host, BaseLayoutControl);
            HostComponent.DestroyComponent(host, BaseLayoutGroup);
            HostComponent.DestroyComponent(host, FilterLayoutItem);
            HostComponent.DestroyComponent(host, BarLayoutItem);
            HostComponent.DestroyComponent(host, FilterClearLayoutItem);

            HostComponent.DestroyComponent(host, FilterTextEdit);
            HostComponent.DestroyComponent(host, FilterClearButton);

            HostComponent.DestroyComponent(host, BaseNavBarControl);
            HostComponent.DestroyComponent(host, BaseNavBarGroup);

            HostComponent.DestroyComponent(host, Icons);

            if (OwnerForm != null)
            {
                OwnerForm.IsMdiContainer = false;
                OwnerForm.Controls.Remove(BaseDockPanel);

                HostComponent.DestroyComponent(host, BaseDocumentManager);
                HostComponent.DestroyComponent(host, BaseTabbedView);
                HostComponent.DestroyComponent(host, BaseDockManager);
            }
        }
        
        /// <summary>
        /// Удаление самого себя.
        /// </summary>
        public void DestroyItself()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

            HostComponent.DestroyComponent(host, this);
        }

        /// <summary>
        /// Показы дизайнера компонента.
        /// </summary>
        public void ShowDesigner()
        {
            DBFormDesignerForm designerForm = new DBFormDesignerForm(this);
            DialogResult dialogResult;
            IUIService service = GetService(typeof(IUIService)) as IUIService;
            if (service != null)
                dialogResult = service.ShowDialog(designerForm);
            else
                dialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Режим разработки.
        /// </summary>
        public bool isDesignMode()
        {
            return DesignMode;
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
        /// Поиск записи в древовидном объекте.
        /// </summary>
        public static TreeListNode FindNode(TreeList tree, ViewMemberInfo info)
        {
            return tree.FindNodeByFieldValue("HashCode", info.HashCode);
        }

        /// <summary>
        /// Создание записи для уже созданных полей таблицы
        /// </summary>
        public static ViewMemberInfo PrintExistData(TreeList treeList, DBFormItemBase item, ViewMemberInfos infos)
        {
            ViewMemberInfo info = new ViewMemberInfo() 
            { 
                ViewType = item.ViewType, 
                Caption = item.Caption, 
                ImageIndex = item.ImageIndex, 
                ImageName = item.ImageName, 
                Item = item 
            };

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
        /// Создание всех элементов для данного типа поля таблицы БД.
        /// </summary>
        public void CreateControl(DBFormItemBase item, string name)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("AddControl");

            item.DBForm = this;
            item.BaseNavBarItem = (NavBarItem)HostComponent.CreateComponent(host, typeof(NavBarItem), string.Format("{0}NavBarItem", name));
            item.BaseNavBarItem.AppearancePressed.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            item.BaseNavBarItem.AppearancePressed.ForeColor = System.Drawing.Color.Red;
            item.BaseNavBarItem.AppearancePressed.Options.UseFont = true;
            item.BaseNavBarItem.AppearancePressed.Options.UseForeColor = true;

            BaseNavBarControl.Items.Add(item.BaseNavBarItem);
            BaseNavBarGroup.ItemLinks.Add(new NavBarItemLink(item.BaseNavBarItem));
            BaseNavBarGroup.SelectedLinkIndex = -1;
            item.UpdateAll();

            transaction.Commit();
        }

        /// <summary>
        /// Создание иконки записи.
        /// </summary>
        public void CreateIcon(DBFormItemBase item, ViewMemberInfo info)
        {
            string imageName = GetImageFullName(info.ImageName);
            if (!ImageEx.IsExist(Icons, info.ImageName) && File.Exists(imageName))
            {
                Image image = Image.FromFile(imageName);
                Icons.Images.Add(image, info.ImageName);
            }

            item.ImageIndex = ImageEx.GetImageIndex(Icons, info.ImageName);
        }
        
        /// <summary>
        /// Создание элемента коллекции.
        /// </summary>
        public DBFormItemBase CreateInstance(ViewMemberInfo info = null, TreeList treeList = null, ViewMemberInfos memberExists = null)
        {
            DBFormItemBase item = null;

            string name = "BaseView";
            if (info != null)
            {
                name = info.ViewTypeName;
                if (TypeEx.IsSubclassOf(info.ViewType, typeof(DBViewInterface)))
                    item = CreateComponent(typeof(DBFormItemView), name) as DBFormItemBase;
                else
                    item = CreateComponent(typeof(DBFormItemBase), name) as DBFormItemBase;
                info.IsUsed = true;
                item.ViewType = info.ViewType;
                item.Caption = info.Caption;
                if (item.Caption == "")
                    item.Caption = name;
                item.ImageName = info.ImageName;
            }
            else
                item = CreateComponent(typeof(DBFormItemBase), name) as DBFormItemBase;

            Items.Add(item);

            CreateControl(item, name);

            if (info != null)
                CreateIcon(item, info);

            if (treeList != null && memberExists != null)
                PrintExistData(treeList, item, memberExists);

            return item;
        }

        /// <summary>
        /// Удание элемента коллекции.
        /// </summary>
        public void DestroyControl(DBFormItemBase item)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("DeleteControl");

            HostComponent.DestroyComponent(host, item.BaseNavBarItem);

            transaction.Commit();
        }

        /// <summary>
        /// Удание записи.
        /// </summary>
        public void DestroyInstance(DBFormItemBase item)
        {
            DestroyControl(item);

            if (item.Owner != null)
                item.Owner.Remove(item);
            if (item is IDisposable)
                ((IDisposable)item).Dispose();

            if (item.ImageIndex != -1)
            {
                DeleteUnusedImage(item.ImageIndex);
                Items.CorrectImageIndex();
            }

            item = null;
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
            foreach (DBFormItemBase item in Items)
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
        public void OnChangedItems(object sender, EventArgs e)
        {
            if (ChangedItems != null)
                ChangedItems(sender, e);
        }

        /// <summary>
        /// Обновление кода дизайна.
        /// </summary>
        public void RefreshDesignCode()
        {
            if (Items.Count > 0)
            {
                string temp = Items[0].Caption;
                Items[0].Caption += "t";
                Items[0].Caption = temp;
            }
        }

        /// <summary>
        /// Поиск записи по ссылке.
        /// </summary>
        public DBFormItemBase FindItemByBarLink(NavBarItem barItem)
        {
            foreach (DBFormItemBase item in Items)
                if (item.BaseNavBarItem == barItem)
                    return item;
            return null;
        }

        /// <summary>
        /// Поиск записи по документу.
        /// </summary>
        public DBFormItemBase FindItemByDocument(BaseDocument document)
        {
            foreach (DBFormItemBase item in Items)
                if (item.Document == document)
                    return item;
            return null;
        }

        /// <summary>
        /// Поиск записи по типу таблицы.
        /// </summary>
        public DBFormItemBase FindItemByTableType(Type type)
        {
            foreach (DBFormItemBase item in Items)
            {
                if (item is DBFormItemView && 
                    ((DBFormItemView)item).TableType == type)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Поиск ссылок по искомому запросу в панели навигации
        /// </summary>
        void FilterItems(string searchString)
        {
            foreach (NavBarGroup group in BaseNavBarControl.Groups)
                group.Visible = IsMatchFilter(group, searchString);


            foreach (NavBarItem item in BaseNavBarControl.Items)
                item.Visible = item.Caption.ToLower().Contains(searchString);
        }

        /// <summary>
        /// Функция соответствия группы искомой строке.
        /// </summary>
        static bool IsMatchFilter(NavBarGroup group, string searchString)
        {
            foreach (NavBarItemLink link in group.ItemLinks)
                if (link.Caption.ToLower().Contains(searchString))
                    return true;

            return false;
        }

        /// <summary>
        /// Проверка формы на сохранения данных.
        /// </summary>
        void CheckAndActivateItem(DBFormItemBase item)
        {
            AllowSwitchMessage allowViewActiveLast = new AllowSwitchMessage(true, false);
            if (ViewActiveLast is DBViewInterface)
            {
                DBViewInterface view = ViewActiveLast as DBViewInterface;
                if (view.DBInterface != null)
                    allowViewActiveLast = view.DBInterface.GetAllowSwitch();
            }

            if (item.Document != null &&
                item.Document.Form != null)
            {
                if (allowViewActiveLast.IsMessage)
                {
                    item.Document.Form.Visible = false;
                    item.Document.Form.Visible = true;
                }
                else
                    if (!item.Document.Form.Visible)
                        item.Document.Form.Visible = true;
            }

            BaseNavBarControl.SelectedLink = item.BaseNavBarItem.Links[0];
            ItemActive = item;
        }

        void CreateItem(DBFormItemBase item)
        {
            if (item != null &&
                item.ViewType != null &&
                TypeEx.IsSubclassOf(item.ViewType, typeof(DBViewBase)))
            {
                if (item.Document == null)
                {
                    SplashScreenManager.ShowForm(null, typeof(WaitFormEx), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription(string.Format("Загружается форма \"{0}\"", item.Caption));

                    CheckAndActivateItem(item);

                    item.View = Activator.CreateInstance(item.ViewType) as DBViewBase;
                    if (item.View is DBViewInterface)
                        ((DBViewInterface)item.View).RightMouseDown += DBFormItem_RightMouseDown;

                    item.Document = BaseTabbedView.AddDocument(item.View);
                    if (item.Document != null)
                    {
                        item.Document.Caption = item.Caption;
                        item.Document.ImageIndex = item.ImageIndex;
                    }
                    if (!item.IsDocumentActivated)
                        item.IsDocumentActivated = true;

                    item.View.InvokeFormUpdate(this, null);
                    SplashScreenManager.CloseForm(false);
                }
                else
                {
                    SplashScreenManager.ShowForm(null, typeof(WaitFormEx), true, true, false);
                    SplashScreenManager.Default.SetWaitFormDescription(string.Format("Обновляется форма \"{0}\"", item.Caption));
                    CheckAndActivateItem(item);

                    BaseTabbedView.Controller.Activate(item.Document);

                    item.View.InvokeFormUpdate(this, null);

                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        /// <summary>
        /// Показать все активированные документы.
        /// </summary>
        public void ShowActivatedDocument()
        {
            foreach (DBFormItemBase item in Items)
            {
                if (item.Document != null &&
                    item.Document.Form != null)
                    item.Document.Form.Visible = true;
            }            
        }
        /// <summary>
        /// Проверка всех окон на возможность закрытия
        /// </summary>
        /// <returns></returns>
        public bool GetAllowSwitchAll()
        {
            AllowSwitchMessage allow = new AllowSwitchMessage(true, false);
            foreach (DBFormItemBase item in Items)
            {
                if (item.View is DBViewInterface)
                {
                    DBViewInterface view = item.View as DBViewInterface;
                    if (view.DBInterface != null)
                    {
                        allow = view.DBInterface.GetAllowSwitch();
                        if (allow.IsAllow)
                        {
                            if (item.Document != null &&
                                item.Document.Form != null)
                                item.Document.Form.Visible = false;
                        }
                        else
                            return false;
                    }
                    else
                        return true;
                }
            }    
            return true;
        }
        #endregion
 
        #region Events handler
        void DBForm_Load(object sender, EventArgs e)
        {
            SplashScreenManager.CloseForm(false);

            if (FilterClearButton != null)
                FilterClearButton.Click += btnFilterClear_Click;

            foreach (DBFormItemBase item in Items)
                if (item.BaseNavBarItem != null)
                    item.BaseNavBarItem.LinkClicked += Items_LinksClicked;
            
            if (BaseTabbedView != null)
            {
                BaseTabbedView.DocumentClosing += tabbedView_DocumentClosing;
                BaseTabbedView.DocumentActivated += BaseTabbedView_DocumentActivated;
                BaseTabbedView.DocumentDeactivated += BaseTabbedView_DocumentDeactivated;
            }

            if (FilterTextEdit != null)
                FilterTextEdit.EditValueChanged += FilterTextEdit_EditValueChanged;

            if (OwnerForm != null)
            {
                OwnerForm.FormClosing += OwnerForm_FormClosing;
                OwnerForm.TextChanged += OwnerForm_TextChanged;
                OwnerForm.Resize += new EventHandler(OwnerForm_Resize);
                OwnerForm.KeyPreview = true;
                OwnerForm.KeyDown += OwnerForm_KeyDown;
            }

            if (NotifyIcon != null)
            {
                NotifyIcon.MouseClick += NotifyIcon_MouseClick;
            }
        }

        void FilterTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            FilterItems(FilterTextEdit.Text.ToLower());
        }

        void btnFilterClear_Click(object sender, EventArgs e)
        {
            FilterTextEdit.Text = "";
        }

        void Items_LinksClicked(object sender, NavBarLinkEventArgs e)
        {
            NavBarItem barItem = sender as NavBarItem;
            DBFormItemBase item = FindItemByBarLink(barItem);
            CreateItem(item);
        }

        void DBFormItem_RightMouseDown(object sender, XPObjectEventArgs e)
        {
            DBFormItemBase item = FindItemByTableType(e.TableType);
            if (item != null)
            {
                CreateItem(item);
                if (item.View is DBViewInterface)
                {
                    DBViewInterface view = item.View as DBViewInterface;
                    view.FocusRecord(e.Record);
                }
            }
        }

        void tabbedView_DocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            TabbedView tabbedView = sender as TabbedView;
            AllowSwitchMessage allow = new AllowSwitchMessage(true, false);
            if (tabbedView != null)
            {                
                DBFormItemBase item = FindItemByDocument(tabbedView.ActiveDocument);
                if (item != null)
                {
                    if (item.View is DBViewInterface)
                    {
                        DBViewInterface view = item.View as DBViewInterface;
                        allow = view.DBInterface.GetAllowSwitch();
                    }
                }
                if (allow.IsAllow)
                    tabbedView.ActiveDocument.Form.Visible = false;
            }
            e.Cancel = true;
        }

        void BaseTabbedView_DocumentActivated(object sender, DocumentEventArgs e)
        {
            DBFormItemBase item = FindItemByDocument(e.Document);
            if (item != null)
                CheckAndActivateItem(item);
        }

        void BaseTabbedView_DocumentDeactivated(object sender, DocumentEventArgs e)
        {
            DBFormItemBase item = FindItemByDocument(e.Document);
            if (item != null)
                ViewActiveLast = item.View;
        }

        void OwnerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowActivatedDocument();
            e.Cancel = !GetAllowSwitchAll();
        }

        void OwnerForm_TextChanged(object sender, EventArgs e)
        {
            if (NotifyIcon != null)
                NotifyIcon.Text = OwnerForm.Text;
        }

        void OwnerForm_Resize(object sender, EventArgs e)
        {
            if (NotifyIconNeed)
                if (OwnerForm.WindowState == FormWindowState.Minimized)
                {
                    OwnerForm.Hide();
                }
        }


        void OwnerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                DBFormItemBase item = FindItemByDocument(BaseTabbedView.ActiveDocument);
                if (item != null)
                {
                    if (item.View is DBViewInterface)
                    {
                        DBViewInterface view = item.View as DBViewInterface;
                        view.DBInterface.SaveData();
                    }
                }
            }

            if (e.Control && e.KeyCode == Keys.L)
            {
                DBFormItemBase item = FindItemByDocument(BaseTabbedView.ActiveDocument);
                if (item != null)
                {
                    if (item.View is DBViewInterface)
                    {
                        DBViewInterface view = item.View as DBViewInterface;
                        view.DBInterface.LoadData();
                    }
                }
            }
        }

        /// <summary>
        /// Изменение состояние окна.
        /// </summary>
        void ChangeWindowState()
        {
            if (OwnerForm.WindowState != FormWindowState.Minimized)
            {
                OwnerFormWindowNormal = OwnerForm.WindowState;
                OwnerForm.WindowState = FormWindowState.Minimized;
            }
            else
            {
                OwnerForm.Show();
                OwnerForm.WindowState = OwnerFormWindowNormal;
            }
        }

        void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (NotifyIconNeed)
                if (OwnerForm != null)
                {
                    if (OwnerForm is XtraFormEx)
                    {
                        if (OwnerForm is XtraFormOnline)
                        {
                            if (((XtraFormOnline)OwnerForm).Online)
                                ChangeWindowState();
                        }
                        ((XtraFormEx)OwnerForm).InvokeTrayClick();
                    }
                    else
                    {
                        ChangeWindowState();
                    }

                }
        }


        #endregion
    }
}
