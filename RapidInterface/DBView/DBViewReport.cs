using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.Design;
using DevExpress.Xpo;
using DevExpress.XtraLayout;
using DevExpress.XtraPrinting.Control;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;

namespace RapidInterface
{
    [DesignerAttribute(typeof(DBViewInterfaceDesigner))]
    public partial class DBViewReport : DBViewBase
    {
        #region Constructors
        public DBViewReport()
        {
            InitializeComponent();
            IsDestroyed = false;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Инициализация компонента через свойство.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public bool _Initialized
        {
            get
            {
                if (printBarManager != null && !IsDestroyed)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    InitializeVisibleComponents();
                else
                    DestroyVisibleComponents();
            }
        }

        private bool IsDestroyed { get; set; }

        /// <summary>
        /// Компонент для интерфейса отчета.
        /// </summary>
        [Category("Visible components")]
        public PrintBarManager printBarManager { get; set; }

        /// <summary>
        /// Компонент для вывода отчетов.
        /// </summary>
        [Category("Visible components")]
        public PrintControl BasePrintControl { get; set; }

        #endregion

        #region Events
        #endregion

        #region Metods common
        /// <summary>
        /// Инициализация компонента и создание группы других компонентов.
        /// </summary>
        public void InitializeVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("DBViewReport");

            // Create compnonents

            printBarManager = (PrintBarManager)HostComponent.CreateComponent(host, typeof(PrintBarManager), "printBarManager");
            BasePrintControl = (PrintControl)HostComponent.CreateComponent(host, typeof(PrintControl), "basePrintControl");

            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());

            ComponentResourceManager resources = Activator.CreateInstance(typeof(ComponentResourceManager), this.GetType()) as ComponentResourceManager;

            PreviewBar previewBar1 = (PreviewBar)HostComponent.CreateComponent(host, typeof(PreviewBar), "previewBar");
            PrintPreviewBarItem printPreviewBarItem2 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem3 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem4 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem5 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem6 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem7 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem8 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem9 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem10 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem11 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem12 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem13 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem14 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem15 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            ZoomBarEditItem zoomBarEditItem1 = (ZoomBarEditItem)HostComponent.CreateComponent(host, typeof(ZoomBarEditItem), "zoomBarEditItem");
            PrintPreviewRepositoryItemComboBox printPreviewRepositoryItemComboBox1 = (PrintPreviewRepositoryItemComboBox)HostComponent.CreateComponent(host, typeof(PrintPreviewRepositoryItemComboBox), "printPreviewRepositoryItemComboBox1");
            PrintPreviewBarItem printPreviewBarItem16 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem17 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem18 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem19 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem20 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem21 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem22 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem23 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem24 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem25 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PrintPreviewBarItem printPreviewBarItem26 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            PreviewBar previewBar2 = (PreviewBar)HostComponent.CreateComponent(host, typeof(PreviewBar), "previewBar");
            PrintPreviewStaticItem printPreviewStaticItem1 = (PrintPreviewStaticItem)HostComponent.CreateComponent(host, typeof(PrintPreviewStaticItem), "printPreviewStaticItem");
            BarStaticItem barStaticItem1 = (BarStaticItem)HostComponent.CreateComponent(host, typeof(BarStaticItem), "barStaticItem");
            ProgressBarEditItem progressBarEditItem1 = (ProgressBarEditItem)HostComponent.CreateComponent(host, typeof(ProgressBarEditItem), "progressBarEditItem");
            RepositoryItemProgressBar repositoryItemProgressBar1 = (RepositoryItemProgressBar)HostComponent.CreateComponent(host, typeof(RepositoryItemProgressBar), "repositoryItemProgressBar");
            PrintPreviewBarItem printPreviewBarItem1 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewBarItem");
            BarButtonItem barButtonItem1 = (BarButtonItem)HostComponent.CreateComponent(host, typeof(BarButtonItem), "barButtonItem");
            PrintPreviewStaticItem printPreviewStaticItem2 = (PrintPreviewStaticItem)HostComponent.CreateComponent(host, typeof(PrintPreviewStaticItem), "printPreviewStaticItem");
            ZoomTrackBarEditItem zoomTrackBarEditItem1 = (ZoomTrackBarEditItem)HostComponent.CreateComponent(host, typeof(ZoomTrackBarEditItem), "zoomTrackBarEditItem");
            RepositoryItemZoomTrackBar repositoryItemZoomTrackBar1 = (RepositoryItemZoomTrackBar)HostComponent.CreateComponent(host, typeof(RepositoryItemZoomTrackBar), "repositoryItemZoomTrackBar");
            BarDockControl barDockControlTop = (BarDockControl)HostComponent.CreateComponent(host, typeof(BarDockControl), "barDockControlTop");
            BarDockControl barDockControlBottom = (BarDockControl)HostComponent.CreateComponent(host, typeof(BarDockControl), "barDockControlBottom");
            BarDockControl barDockControlLeft = (BarDockControl)HostComponent.CreateComponent(host, typeof(BarDockControl), "barDockControlLeft");
            BarDockControl barDockControlRight = (BarDockControl)HostComponent.CreateComponent(host, typeof(BarDockControl), "basePrintBarManager");
            PrintPreviewSubItem printPreviewSubItem1 = (PrintPreviewSubItem)HostComponent.CreateComponent(host, typeof(PrintPreviewSubItem), "printPreviewSubItem");
            PrintPreviewSubItem printPreviewSubItem2 = (PrintPreviewSubItem)HostComponent.CreateComponent(host, typeof(PrintPreviewSubItem), "printPreviewSubItem");
            PrintPreviewSubItem printPreviewSubItem4 = (PrintPreviewSubItem)HostComponent.CreateComponent(host, typeof(PrintPreviewSubItem), "printPreviewSubItem");
            PrintPreviewBarItem printPreviewBarItem27 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewSubItem");
            PrintPreviewBarItem printPreviewBarItem28 = (PrintPreviewBarItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarItem), "printPreviewSubItem");
            BarToolbarsListItem barToolbarsListItem1 = (BarToolbarsListItem)HostComponent.CreateComponent(host, typeof(BarToolbarsListItem), "printPreviewSubItem");
            PrintPreviewSubItem printPreviewSubItem3 = (PrintPreviewSubItem)HostComponent.CreateComponent(host, typeof(PrintPreviewSubItem), "printPreviewSubItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem1 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem2 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem3 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem4 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem5 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem6 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem7 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem8 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem9 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem10 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem11 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem12 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem13 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem14 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem15 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem16 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");
            PrintPreviewBarCheckItem printPreviewBarCheckItem17 = (PrintPreviewBarCheckItem)HostComponent.CreateComponent(host, typeof(PrintPreviewBarCheckItem), "printPreviewBarCheckItem");

            SuspendLayout();
            // 
            // BasePrintBarManager
            // 
            printBarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            previewBar1,
            previewBar2});
            printBarManager.DockControls.Add(barDockControlTop);
            printBarManager.DockControls.Add(barDockControlBottom);
            printBarManager.DockControls.Add(barDockControlLeft);
            printBarManager.DockControls.Add(barDockControlRight);
            printBarManager.Form = this;
            printBarManager.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject(string.Format("{0}.ImageStream", printBarManager.Site.Name))));
            printBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            printPreviewStaticItem1,
            barStaticItem1,
            progressBarEditItem1,
            printPreviewBarItem1,
            barButtonItem1,
            printPreviewStaticItem2,
            zoomTrackBarEditItem1,
            printPreviewBarItem2,
            printPreviewBarItem3,
            printPreviewBarItem4,
            printPreviewBarItem5,
            printPreviewBarItem6,
            printPreviewBarItem7,
            printPreviewBarItem8,
            printPreviewBarItem9,
            printPreviewBarItem10,
            printPreviewBarItem11,
            printPreviewBarItem12,
            printPreviewBarItem13,
            printPreviewBarItem14,
            printPreviewBarItem15,
            zoomBarEditItem1,
            printPreviewBarItem16,
            printPreviewBarItem17,
            printPreviewBarItem18,
            printPreviewBarItem19,
            printPreviewBarItem20,
            printPreviewBarItem21,
            printPreviewBarItem22,
            printPreviewBarItem23,
            printPreviewBarItem24,
            printPreviewBarItem25,
            printPreviewBarItem26,
            printPreviewSubItem1,
            printPreviewSubItem2,
            printPreviewSubItem3,
            printPreviewSubItem4,
            printPreviewBarItem27,
            printPreviewBarItem28,
            barToolbarsListItem1,
            printPreviewBarCheckItem1,
            printPreviewBarCheckItem2,
            printPreviewBarCheckItem3,
            printPreviewBarCheckItem4,
            printPreviewBarCheckItem5,
            printPreviewBarCheckItem6,
            printPreviewBarCheckItem7,
            printPreviewBarCheckItem8,
            printPreviewBarCheckItem9,
            printPreviewBarCheckItem10,
            printPreviewBarCheckItem11,
            printPreviewBarCheckItem12,
            printPreviewBarCheckItem13,
            printPreviewBarCheckItem14,
            printPreviewBarCheckItem15,
            printPreviewBarCheckItem16,
            printPreviewBarCheckItem17});
            printBarManager.MaxItemId = 57;
            printBarManager.PreviewBar = previewBar1;
            printBarManager.PrintControl = BasePrintControl;
            printBarManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            repositoryItemProgressBar1,
            repositoryItemZoomTrackBar1,
            printPreviewRepositoryItemComboBox1});
            printBarManager.StatusBar = previewBar2;
            printBarManager.TransparentEditors = true;
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            barDockControlTop.Location = new System.Drawing.Point(0, 0);
            barDockControlTop.Size = new System.Drawing.Size(989, 31);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            barDockControlBottom.Location = new System.Drawing.Point(0, 596);
            barDockControlBottom.Size = new System.Drawing.Size(989, 28);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            barDockControlLeft.Size = new System.Drawing.Size(0, 565);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            barDockControlRight.Location = new System.Drawing.Point(989, 31);
            barDockControlRight.Size = new System.Drawing.Size(0, 565);
            // 
            // previewBar1
            // 
            previewBar1.BarName = "Toolbar";
            previewBar1.DockCol = 0;
            previewBar1.DockRow = 1;
            previewBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            previewBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem2),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem3),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem4),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem5, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem6, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem7),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem8, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem9),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem10),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem11),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem12),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem13, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem14),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem15, true),
            new DevExpress.XtraBars.LinkPersistInfo(zoomBarEditItem1),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem16),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem17, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem18),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem19),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem20),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem21, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem22),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem23),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem24, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem25),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem26, true)});
            previewBar1.Text = "Toolbar";
            // 
            // previewBar2
            // 
            previewBar2.BarName = "Status Bar";
            previewBar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            previewBar2.DockCol = 0;
            previewBar2.DockRow = 0;
            previewBar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            previewBar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(barStaticItem1, true),
            new DevExpress.XtraBars.LinkPersistInfo(progressBarEditItem1),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem1),
            new DevExpress.XtraBars.LinkPersistInfo(barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewStaticItem2, true),
            new DevExpress.XtraBars.LinkPersistInfo(zoomTrackBarEditItem1)});
            previewBar2.OptionsBar.AllowQuickCustomization = false;
            previewBar2.OptionsBar.DrawDragBorder = false;
            previewBar2.OptionsBar.UseWholeRow = true;
            previewBar2.Text = "Status Bar";
            // 
            // printPreviewStaticItem1
            // 
            printPreviewStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            printPreviewStaticItem1.Caption = "Nothing";
            printPreviewStaticItem1.Id = 0;
            printPreviewStaticItem1.LeftIndent = 1;
            printPreviewStaticItem1.RightIndent = 1;
            printPreviewStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            printPreviewStaticItem1.Type = "PageOfPages";
            // 
            // barStaticItem1
            // 
            barStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            barStaticItem1.Id = 1;
            barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.OnlyInRuntime;
            // 
            // repositoryItemProgressBar1
            // 
            repositoryItemProgressBar1.UseParentBackground = true;
            // 
            // progressBarEditItem1
            // 
            progressBarEditItem1.Edit = repositoryItemProgressBar1;
            progressBarEditItem1.EditHeight = 12;
            progressBarEditItem1.Id = 2;
            progressBarEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            progressBarEditItem1.Width = 150;
            // 
            // printPreviewBarItem1
            // 
            printPreviewBarItem1.Caption = "Stop";
            printPreviewBarItem1.Command = DevExpress.XtraPrinting.PrintingSystemCommand.StopPageBuilding;
            printPreviewBarItem1.Enabled = false;
            printPreviewBarItem1.Hint = "Stop";
            printPreviewBarItem1.Id = 3;
            printPreviewBarItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barButtonItem1
            // 
            barButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            barButtonItem1.Enabled = false;
            barButtonItem1.Id = 4;
            barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.OnlyInRuntime;
            // 
            // printPreviewStaticItem2
            // 
            printPreviewStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            printPreviewStaticItem2.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            printPreviewStaticItem2.Caption = "100%";
            printPreviewStaticItem2.Id = 5;
            printPreviewStaticItem2.TextAlignment = System.Drawing.StringAlignment.Far;
            printPreviewStaticItem2.Type = "ZoomFactor";
            printPreviewStaticItem2.Width = 40;
            // 
            // repositoryItemZoomTrackBar1
            // 
            repositoryItemZoomTrackBar1.Alignment = DevExpress.Utils.VertAlignment.Center;
            repositoryItemZoomTrackBar1.AllowFocused = false;
            repositoryItemZoomTrackBar1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            repositoryItemZoomTrackBar1.Maximum = 180;
            repositoryItemZoomTrackBar1.ScrollThumbStyle = DevExpress.XtraEditors.Repository.ScrollThumbStyle.ArrowDownRight;
            repositoryItemZoomTrackBar1.UseParentBackground = true;
            // 
            // zoomTrackBarEditItem1
            // 
            zoomTrackBarEditItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            zoomTrackBarEditItem1.Edit = repositoryItemZoomTrackBar1;
            zoomTrackBarEditItem1.EditValue = 90;
            zoomTrackBarEditItem1.Enabled = false;
            zoomTrackBarEditItem1.Id = 6;
            zoomTrackBarEditItem1.Range = new int[] {10, 500};
            zoomTrackBarEditItem1.Width = 140;
            // 
            // printPreviewBarItem2
            // 
            printPreviewBarItem2.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            printPreviewBarItem2.Caption = "Document Map";
            printPreviewBarItem2.Command = DevExpress.XtraPrinting.PrintingSystemCommand.DocumentMap;
            printPreviewBarItem2.Enabled = false;
            printPreviewBarItem2.Hint = "Document Map";
            printPreviewBarItem2.Id = 7;
            printPreviewBarItem2.ImageIndex = 19;
            // 
            // printPreviewBarItem3
            // 
            printPreviewBarItem3.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            printPreviewBarItem3.Caption = "Parameters";
            printPreviewBarItem3.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Parameters;
            printPreviewBarItem3.Enabled = false;
            printPreviewBarItem3.Hint = "Parameters";
            printPreviewBarItem3.Id = 8;
            printPreviewBarItem3.ImageIndex = 22;
            // 
            // printPreviewBarItem4
            // 
            printPreviewBarItem4.Caption = "Search";
            printPreviewBarItem4.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Find;
            printPreviewBarItem4.Enabled = false;
            printPreviewBarItem4.Hint = "Search";
            printPreviewBarItem4.Id = 9;
            printPreviewBarItem4.ImageIndex = 20;
            // 
            // printPreviewBarItem5
            // 
            printPreviewBarItem5.Caption = "Customize";
            printPreviewBarItem5.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Customize;
            printPreviewBarItem5.Enabled = false;
            printPreviewBarItem5.Hint = "Customize";
            printPreviewBarItem5.Id = 10;
            printPreviewBarItem5.ImageIndex = 14;
            // 
            // printPreviewBarItem6
            // 
            printPreviewBarItem6.Caption = "Open";
            printPreviewBarItem6.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Open;
            printPreviewBarItem6.Enabled = false;
            printPreviewBarItem6.Hint = "Open a document";
            printPreviewBarItem6.Id = 11;
            printPreviewBarItem6.ImageIndex = 23;
            // 
            // printPreviewBarItem7
            // 
            printPreviewBarItem7.Caption = "Save";
            printPreviewBarItem7.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Save;
            printPreviewBarItem7.Enabled = false;
            printPreviewBarItem7.Hint = "Save the document";
            printPreviewBarItem7.Id = 12;
            printPreviewBarItem7.ImageIndex = 24;
            // 
            // printPreviewBarItem8
            // 
            printPreviewBarItem8.Caption = "&Print...";
            printPreviewBarItem8.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Print;
            printPreviewBarItem8.Enabled = false;
            printPreviewBarItem8.Hint = "Print";
            printPreviewBarItem8.Id = 13;
            printPreviewBarItem8.ImageIndex = 0;
            // 
            // printPreviewBarItem9
            // 
            printPreviewBarItem9.Caption = "P&rint";
            printPreviewBarItem9.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PrintDirect;
            printPreviewBarItem9.Enabled = false;
            printPreviewBarItem9.Hint = "Quick Print";
            printPreviewBarItem9.Id = 14;
            printPreviewBarItem9.ImageIndex = 1;
            // 
            // printPreviewBarItem10
            // 
            printPreviewBarItem10.Caption = "Page Set&up...";
            printPreviewBarItem10.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PageSetup;
            printPreviewBarItem10.Enabled = false;
            printPreviewBarItem10.Hint = "Page Setup";
            printPreviewBarItem10.Id = 15;
            printPreviewBarItem10.ImageIndex = 2;
            // 
            // printPreviewBarItem11
            // 
            printPreviewBarItem11.Caption = "Header And Footer";
            printPreviewBarItem11.Command = DevExpress.XtraPrinting.PrintingSystemCommand.EditPageHF;
            printPreviewBarItem11.Enabled = false;
            printPreviewBarItem11.Hint = "Header And Footer";
            printPreviewBarItem11.Id = 16;
            printPreviewBarItem11.ImageIndex = 15;
            // 
            // printPreviewBarItem12
            // 
            printPreviewBarItem12.ActAsDropDown = true;
            printPreviewBarItem12.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            printPreviewBarItem12.Caption = "Scale";
            printPreviewBarItem12.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Scale;
            printPreviewBarItem12.Enabled = false;
            printPreviewBarItem12.Hint = "Scale";
            printPreviewBarItem12.Id = 17;
            printPreviewBarItem12.ImageIndex = 25;
            // 
            // printPreviewBarItem13
            // 
            printPreviewBarItem13.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            printPreviewBarItem13.Caption = "Hand Tool";
            printPreviewBarItem13.Command = DevExpress.XtraPrinting.PrintingSystemCommand.HandTool;
            printPreviewBarItem13.Enabled = false;
            printPreviewBarItem13.Hint = "Hand Tool";
            printPreviewBarItem13.Id = 18;
            printPreviewBarItem13.ImageIndex = 16;
            // 
            // printPreviewBarItem14
            // 
            printPreviewBarItem14.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            printPreviewBarItem14.Caption = "Magnifier";
            printPreviewBarItem14.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Magnifier;
            printPreviewBarItem14.Enabled = false;
            printPreviewBarItem14.Hint = "Magnifier";
            printPreviewBarItem14.Id = 19;
            printPreviewBarItem14.ImageIndex = 3;
            // 
            // printPreviewBarItem15
            // 
            printPreviewBarItem15.Caption = "Zoom Out";
            printPreviewBarItem15.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ZoomOut;
            printPreviewBarItem15.Enabled = false;
            printPreviewBarItem15.Hint = "Zoom Out";
            printPreviewBarItem15.Id = 20;
            printPreviewBarItem15.ImageIndex = 5;
            // 
            // zoomBarEditItem1
            // 
            zoomBarEditItem1.Caption = "Zoom";
            zoomBarEditItem1.Edit = printPreviewRepositoryItemComboBox1;
            zoomBarEditItem1.EditValue = "100%";
            zoomBarEditItem1.Enabled = false;
            zoomBarEditItem1.Hint = "Zoom";
            zoomBarEditItem1.Id = 21;
            zoomBarEditItem1.Width = 70;
            // 
            // printPreviewRepositoryItemComboBox1
            // 
            printPreviewRepositoryItemComboBox1.AutoComplete = false;
            printPreviewRepositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            printPreviewRepositoryItemComboBox1.DropDownRows = 11;
            printPreviewRepositoryItemComboBox1.UseParentBackground = true;
            // 
            // printPreviewBarItem16
            // 
            printPreviewBarItem16.Caption = "Zoom In";
            printPreviewBarItem16.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ZoomIn;
            printPreviewBarItem16.Enabled = false;
            printPreviewBarItem16.Hint = "Zoom In";
            printPreviewBarItem16.Id = 22;
            printPreviewBarItem16.ImageIndex = 4;
            // 
            // printPreviewBarItem17
            // 
            printPreviewBarItem17.Caption = "First Page";
            printPreviewBarItem17.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ShowFirstPage;
            printPreviewBarItem17.Enabled = false;
            printPreviewBarItem17.Hint = "First Page";
            printPreviewBarItem17.Id = 23;
            printPreviewBarItem17.ImageIndex = 7;
            // 
            // printPreviewBarItem18
            // 
            printPreviewBarItem18.Caption = "Previous Page";
            printPreviewBarItem18.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ShowPrevPage;
            printPreviewBarItem18.Enabled = false;
            printPreviewBarItem18.Hint = "Previous Page";
            printPreviewBarItem18.Id = 24;
            printPreviewBarItem18.ImageIndex = 8;
            // 
            // printPreviewBarItem19
            // 
            printPreviewBarItem19.Caption = "Next Page";
            printPreviewBarItem19.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ShowNextPage;
            printPreviewBarItem19.Enabled = false;
            printPreviewBarItem19.Hint = "Next Page";
            printPreviewBarItem19.Id = 25;
            printPreviewBarItem19.ImageIndex = 9;
            // 
            // printPreviewBarItem20
            // 
            printPreviewBarItem20.Caption = "Last Page";
            printPreviewBarItem20.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ShowLastPage;
            printPreviewBarItem20.Enabled = false;
            printPreviewBarItem20.Hint = "Last Page";
            printPreviewBarItem20.Id = 26;
            printPreviewBarItem20.ImageIndex = 10;
            // 
            // printPreviewBarItem21
            // 
            printPreviewBarItem21.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            printPreviewBarItem21.Caption = "Multiple Pages";
            printPreviewBarItem21.Command = DevExpress.XtraPrinting.PrintingSystemCommand.MultiplePages;
            printPreviewBarItem21.Enabled = false;
            printPreviewBarItem21.Hint = "Multiple Pages";
            printPreviewBarItem21.Id = 27;
            printPreviewBarItem21.ImageIndex = 11;
            // 
            // printPreviewBarItem22
            // 
            printPreviewBarItem22.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            printPreviewBarItem22.Caption = "&Color...";
            printPreviewBarItem22.Command = DevExpress.XtraPrinting.PrintingSystemCommand.FillBackground;
            printPreviewBarItem22.Enabled = false;
            printPreviewBarItem22.Hint = "Background";
            printPreviewBarItem22.Id = 28;
            printPreviewBarItem22.ImageIndex = 12;
            // 
            // printPreviewBarItem23
            // 
            printPreviewBarItem23.Caption = "&Watermark...";
            printPreviewBarItem23.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Watermark;
            printPreviewBarItem23.Enabled = false;
            printPreviewBarItem23.Hint = "Watermark";
            printPreviewBarItem23.Id = 29;
            printPreviewBarItem23.ImageIndex = 21;
            // 
            // printPreviewBarItem24
            // 
            printPreviewBarItem24.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            printPreviewBarItem24.Caption = "Export Document...";
            printPreviewBarItem24.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportFile;
            printPreviewBarItem24.Enabled = false;
            printPreviewBarItem24.Hint = "Export Document...";
            printPreviewBarItem24.Id = 30;
            printPreviewBarItem24.ImageIndex = 18;
            // 
            // printPreviewBarItem25
            // 
            printPreviewBarItem25.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            printPreviewBarItem25.Caption = "Send via E-Mail...";
            printPreviewBarItem25.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendFile;
            printPreviewBarItem25.Enabled = false;
            printPreviewBarItem25.Hint = "Send via E-Mail...";
            printPreviewBarItem25.Id = 31;
            printPreviewBarItem25.ImageIndex = 17;
            // 
            // printPreviewBarItem26
            // 
            printPreviewBarItem26.Caption = "E&xit";
            printPreviewBarItem26.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ClosePreview;
            printPreviewBarItem26.Enabled = false;
            printPreviewBarItem26.Hint = "Close Preview";
            printPreviewBarItem26.Id = 32;
            printPreviewBarItem26.ImageIndex = 13;
            // 
            // printPreviewSubItem1
            // 
            printPreviewSubItem1.Caption = "&File";
            printPreviewSubItem1.Command = DevExpress.XtraPrinting.PrintingSystemCommand.File;
            printPreviewSubItem1.Id = 33;
            printPreviewSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem10),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem8),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem9),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem24, true),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem25),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem26, true)});
            // 
            // printPreviewSubItem2
            // 
            printPreviewSubItem2.Caption = "&View";
            printPreviewSubItem2.Command = DevExpress.XtraPrinting.PrintingSystemCommand.View;
            printPreviewSubItem2.Id = 34;
            printPreviewSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewSubItem4, true),
            new DevExpress.XtraBars.LinkPersistInfo(barToolbarsListItem1, true)});
            // 
            // printPreviewSubItem3
            // 
            printPreviewSubItem3.Caption = "&Background";
            printPreviewSubItem3.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Background;
            printPreviewSubItem3.Id = 35;
            printPreviewSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem22),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem23)});
            // 
            // printPreviewSubItem4
            // 
            printPreviewSubItem4.Caption = "&Page Layout";
            printPreviewSubItem4.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PageLayout;
            printPreviewSubItem4.Id = 36;
            printPreviewSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem27),
            new DevExpress.XtraBars.LinkPersistInfo(printPreviewBarItem28)});
            // 
            // printPreviewBarItem27
            // 
            printPreviewBarItem27.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            printPreviewBarItem27.Caption = "&Facing";
            printPreviewBarItem27.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutFacing;
            printPreviewBarItem27.Enabled = false;
            printPreviewBarItem27.GroupIndex = 100;
            printPreviewBarItem27.Id = 37;
            // 
            // printPreviewBarItem28
            // 
            printPreviewBarItem28.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            printPreviewBarItem28.Caption = "&Continuous";
            printPreviewBarItem28.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutContinuous;
            printPreviewBarItem28.Down = true;
            printPreviewBarItem28.Enabled = false;
            printPreviewBarItem28.GroupIndex = 100;
            printPreviewBarItem28.Id = 38;
            // 
            // barToolbarsListItem1
            // 
            barToolbarsListItem1.Caption = "Bars";
            barToolbarsListItem1.Id = 39;
            // 
            // printPreviewBarCheckItem1
            // 
            printPreviewBarCheckItem1.Caption = "PDF File";
            printPreviewBarCheckItem1.Checked = true;
            printPreviewBarCheckItem1.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportPdf;
            printPreviewBarCheckItem1.Enabled = false;
            printPreviewBarCheckItem1.GroupIndex = 2;
            printPreviewBarCheckItem1.Hint = "PDF File";
            printPreviewBarCheckItem1.Id = 40;
            // 
            // printPreviewBarCheckItem2
            // 
            printPreviewBarCheckItem2.Caption = "HTML File";
            printPreviewBarCheckItem2.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportHtm;
            printPreviewBarCheckItem2.Enabled = false;
            printPreviewBarCheckItem2.GroupIndex = 2;
            printPreviewBarCheckItem2.Hint = "HTML File";
            printPreviewBarCheckItem2.Id = 41;
            // 
            // printPreviewBarCheckItem3
            // 
            printPreviewBarCheckItem3.Caption = "MHT File";
            printPreviewBarCheckItem3.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportMht;
            printPreviewBarCheckItem3.Enabled = false;
            printPreviewBarCheckItem3.GroupIndex = 2;
            printPreviewBarCheckItem3.Hint = "MHT File";
            printPreviewBarCheckItem3.Id = 42;
            // 
            // printPreviewBarCheckItem4
            // 
            printPreviewBarCheckItem4.Caption = "RTF File";
            printPreviewBarCheckItem4.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportRtf;
            printPreviewBarCheckItem4.Enabled = false;
            printPreviewBarCheckItem4.GroupIndex = 2;
            printPreviewBarCheckItem4.Hint = "RTF File";
            printPreviewBarCheckItem4.Id = 43;
            // 
            // printPreviewBarCheckItem5
            // 
            printPreviewBarCheckItem5.Caption = "XLS File";
            printPreviewBarCheckItem5.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportXls;
            printPreviewBarCheckItem5.Enabled = false;
            printPreviewBarCheckItem5.GroupIndex = 2;
            printPreviewBarCheckItem5.Hint = "XLS File";
            printPreviewBarCheckItem5.Id = 44;
            // 
            // printPreviewBarCheckItem6
            // 
            printPreviewBarCheckItem6.Caption = "XLSX File";
            printPreviewBarCheckItem6.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportXlsx;
            printPreviewBarCheckItem6.Enabled = false;
            printPreviewBarCheckItem6.GroupIndex = 2;
            printPreviewBarCheckItem6.Hint = "XLSX File";
            printPreviewBarCheckItem6.Id = 45;
            // 
            // printPreviewBarCheckItem7
            // 
            printPreviewBarCheckItem7.Caption = "CSV File";
            printPreviewBarCheckItem7.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportCsv;
            printPreviewBarCheckItem7.Enabled = false;
            printPreviewBarCheckItem7.GroupIndex = 2;
            printPreviewBarCheckItem7.Hint = "CSV File";
            printPreviewBarCheckItem7.Id = 46;
            // 
            // printPreviewBarCheckItem8
            // 
            printPreviewBarCheckItem8.Caption = "Text File";
            printPreviewBarCheckItem8.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportTxt;
            printPreviewBarCheckItem8.Enabled = false;
            printPreviewBarCheckItem8.GroupIndex = 2;
            printPreviewBarCheckItem8.Hint = "Text File";
            printPreviewBarCheckItem8.Id = 47;
            // 
            // printPreviewBarCheckItem9
            // 
            printPreviewBarCheckItem9.Caption = "Image File";
            printPreviewBarCheckItem9.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportGraphic;
            printPreviewBarCheckItem9.Enabled = false;
            printPreviewBarCheckItem9.GroupIndex = 2;
            printPreviewBarCheckItem9.Hint = "Image File";
            printPreviewBarCheckItem9.Id = 48;
            // 
            // printPreviewBarCheckItem10
            // 
            printPreviewBarCheckItem10.Caption = "PDF File";
            printPreviewBarCheckItem10.Checked = true;
            printPreviewBarCheckItem10.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendPdf;
            printPreviewBarCheckItem10.Enabled = false;
            printPreviewBarCheckItem10.GroupIndex = 1;
            printPreviewBarCheckItem10.Hint = "PDF File";
            printPreviewBarCheckItem10.Id = 49;
            // 
            // printPreviewBarCheckItem11
            // 
            printPreviewBarCheckItem11.Caption = "MHT File";
            printPreviewBarCheckItem11.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendMht;
            printPreviewBarCheckItem11.Enabled = false;
            printPreviewBarCheckItem11.GroupIndex = 1;
            printPreviewBarCheckItem11.Hint = "MHT File";
            printPreviewBarCheckItem11.Id = 50;
            // 
            // printPreviewBarCheckItem12
            // 
            printPreviewBarCheckItem12.Caption = "RTF File";
            printPreviewBarCheckItem12.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendRtf;
            printPreviewBarCheckItem12.Enabled = false;
            printPreviewBarCheckItem12.GroupIndex = 1;
            printPreviewBarCheckItem12.Hint = "RTF File";
            printPreviewBarCheckItem12.Id = 51;
            // 
            // printPreviewBarCheckItem13
            // 
            printPreviewBarCheckItem13.Caption = "XLS File";
            printPreviewBarCheckItem13.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendXls;
            printPreviewBarCheckItem13.Enabled = false;
            printPreviewBarCheckItem13.GroupIndex = 1;
            printPreviewBarCheckItem13.Hint = "XLS File";
            printPreviewBarCheckItem13.Id = 52;
            // 
            // printPreviewBarCheckItem14
            // 
            printPreviewBarCheckItem14.Caption = "XLSX File";
            printPreviewBarCheckItem14.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendXlsx;
            printPreviewBarCheckItem14.Enabled = false;
            printPreviewBarCheckItem14.GroupIndex = 1;
            printPreviewBarCheckItem14.Hint = "XLSX File";
            printPreviewBarCheckItem14.Id = 53;
            // 
            // printPreviewBarCheckItem15
            // 
            printPreviewBarCheckItem15.Caption = "CSV File";
            printPreviewBarCheckItem15.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendCsv;
            printPreviewBarCheckItem15.Enabled = false;
            printPreviewBarCheckItem15.GroupIndex = 1;
            printPreviewBarCheckItem15.Hint = "CSV File";
            printPreviewBarCheckItem15.Id = 54;
            // 
            // printPreviewBarCheckItem16
            // 
            printPreviewBarCheckItem16.Caption = "Text File";
            printPreviewBarCheckItem16.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendTxt;
            printPreviewBarCheckItem16.Enabled = false;
            printPreviewBarCheckItem16.GroupIndex = 1;
            printPreviewBarCheckItem16.Hint = "Text File";
            printPreviewBarCheckItem16.Id = 55;
            // 
            // printPreviewBarCheckItem17
            // 
            printPreviewBarCheckItem17.Caption = "Image File";
            printPreviewBarCheckItem17.Command = DevExpress.XtraPrinting.PrintingSystemCommand.SendGraphic;
            printPreviewBarCheckItem17.Enabled = false;
            printPreviewBarCheckItem17.GroupIndex = 1;
            printPreviewBarCheckItem17.Hint = "Image File";
            printPreviewBarCheckItem17.Id = 56;
            // 
            // printControl1
            // 
            BasePrintControl.BackColor = System.Drawing.Color.Empty;
            BasePrintControl.Dock = System.Windows.Forms.DockStyle.Fill;
            BasePrintControl.ForeColor = System.Drawing.Color.Empty;
            BasePrintControl.IsMetric = true;
            BasePrintControl.Location = new System.Drawing.Point(0, 31);
            BasePrintControl.Size = new System.Drawing.Size(989, 565);
            BasePrintControl.TabIndex = 4;
            BasePrintControl.TooltipFont = new System.Drawing.Font("Tahoma", 8.25F);
            // 
            // this
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BasePrintControl);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            ((System.ComponentModel.ISupportInitialize)(printBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemZoomTrackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(printPreviewRepositoryItemComboBox1)).EndInit();
            ResumeLayout(false);

            transaction.Commit();

            IsDestroyed = false;
        }

        /// <summary>
        /// Удаление компонента и других связанных с ним компонентов.
        /// </summary>
        public void DestroyVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("DestroyVisibleComponent");

            HostComponent.DestroyComponent(host, BasePrintControl);
            HostComponent.DestroyComponent(host, printBarManager);

            transaction.Commit();

            IsDestroyed = true;
        }

        #endregion

        #region Event_handlers
        private void DBInterfaceView_Load(object sender, EventArgs e)
        {

        }

        private void DBInterfaceView_FormUpdate(object sender, EventArgs e)
        {

        }
        #endregion


    }
}
