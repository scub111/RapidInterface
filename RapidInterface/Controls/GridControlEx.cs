using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.Xpo;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using System.Drawing;


namespace RapidInterface
{
    public partial class GridControlEx: GridControl
    {
        public GridControlEx()
        {
            InitializeComponent();
            Init();
        }

        public GridControlEx(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            Init();
        }

        public virtual void Init()
        {
            SubMenuItems = new List<DXSubMenuItem>();
            ExportName = "ExportDocument";
        }

        public class GridViewColumnButtonMenu : GridViewMenu
        {
            public GridViewColumnButtonMenu(GridView view) : base(view) { }
            // Create menu items.
            // This method is automatically called by the menu's public Init method.
            protected override void CreateItems()
            {

                Items.Clear();
                DXSubMenuItem columnsItem = new DXSubMenuItem("Columns");
                //Items.Add(CreateMenuItem("Runtime Column Customization", GridMenuImages.Column.Images[3],
                //  "Customization", true));
                Items.Add(CreateMenuItem("Копировать все в буфер", Properties.Resources.Copy, "Copy all", true));
                Items.Add(CreateMenuItem("Вставить из буфера", Properties.Resources.Paste, "Paste", true));
                //Items.Add(CreateMenuItem("Вставить 1000 строк", null, "Paste 1000 rows", true));
                //Items.Add(CreateMenuItem("Вставить 10000 строк", null, "Paste 10000 rows", true));
                Items.Add(CreateMenuItem("Удалить все", Properties.Resources.DeleteAll, "Remove all", true));
                Items.Add(CreateMenuItem("Экспорт...", Properties.Resources.Export, "Export...", true));

                if (SubMenuItems != null)
                    for (int i = 0; i < SubMenuItems.Count; i++)
                        Items.Add(SubMenuItems[i]);
            }

            public delegate void OnMyClickEventHandler(object sender, EventArgs e);
            /// <summary>
            /// Вызывается при нажатие кнопки меню.
            /// </summary>
            public event OnMyClickEventHandler OnMyClick;

            /// <summary>
            /// Пункты меню.
            /// </summary>
            public List<DXSubMenuItem> SubMenuItems;

            protected override void OnMenuItemClick(object sender, EventArgs e)
            {
                if (RaiseClickEvent(sender, null)) return;
                DXMenuItem Item = sender as DXMenuItem;

                if (OnMyClick != null)
                    OnMyClick(sender, e);

                View.OptionsSelection.MultiSelect = true;
                string strTest = "As columns are created, each is typed with a standard SQL type, and clicking in the first column of the table sets that tablecolumn to be part of the primary key.  A point I noticed at this juncture is that modifying multi-column keys seems to run into some problems at points.";
                int iCount = strTest.Length;

                if (Item.Tag.ToString() == "Copy all")
                {
                    View.SelectAll();
                    string selectedCellsText = GetSelectedValues(View);
                    Clipboard.SetDataObject(selectedCellsText);
                    View.ClearSelection();
                }
                else if (Item.Tag.ToString() == "Paste")
                {
                    PasteValues(View, Clipboard.GetText());
                }
                else if (Item.Tag.ToString() == "Paste 1000 rows")
                {
                    PasteRows(View, strTest, 1000);
                }
                else if (Item.Tag.ToString() == "Paste 10000 rows")
                {
                    PasteRows(View, strTest, 10000);
                }
                else if (Item.Tag.ToString() == "Remove all")
                {
                    RemoveAll(View);
                }
                else if (Item.Tag.ToString() == "Export...")
                {
                    Export(View);
                }    
                View.OptionsSelection.MultiSelect = false;
            }

            private void RemoveAll(GridView gvView)
            {
                gvView.SelectAll();
                gvView.DeleteSelectedRows();
                gvView.ClearSelection();
            }

            private string GetSelectedValues(GridView gvView)
            {
                if (gvView.SelectedRowsCount == 0) return "";

                const string strCellDelimiter = "\t";
                const string strLineDelimiter = "\r\n";
                string strResult = "";

                // iterate cells and compose a tab delimited string of cell values
                for (int i = 0; i < gvView.SelectedRowsCount; i++)
                {
                    int row = gvView.GetSelectedRows()[i];
                    for (int j = 0; j < gvView.VisibleColumns.Count; j++)
                    {
                        strResult += gvView.GetRowCellDisplayText(row, gvView.VisibleColumns[j]);
                        if (j != gvView.VisibleColumns.Count - 1)
                            strResult += strCellDelimiter;
                        else
                            if (i != gvView.SelectedRowsCount - 1)
                                strResult += strLineDelimiter;
                    }
                }
                return strResult;
            }

            private void PasteValues(GridView gvView, string strTextIn)
            {
                RemoveAll(gvView);                
               
                string[] strArray = strTextIn.Split(new string[] { "\t", "\r\n" }, StringSplitOptions.None);
                int rowCount = strArray.Length / gvView.VisibleColumns.Count;
                int columnCount = gvView.VisibleColumns.Count;

                int i;
                for (i = 0; i < rowCount + 1; i++)
                    gvView.AddNewRow();

                for (i = 0; i < rowCount; i++)
                    for (int j = 0; j < columnCount; j++)
                        gvView.SetRowCellValue(i, gvView.VisibleColumns[j],
                            strArray[i * columnCount + j]);

                gvView.DeleteRow(gvView.RowCount - 1);
                gvView.DeleteRow(gvView.RowCount - 1);
            }


            private void PasteRows(GridView gvView, string strTextIn, int rowCount)
            {         
                gvView.BeginInit();

                RemoveAll(gvView);

                int i;
                for (i = 0; i < rowCount + 1; i++)
                    gvView.AddNewRow();

                for (i = 0; i < rowCount; i++)
                    for (int j = 0; j < gvView.VisibleColumns.Count; j++)
                        gvView.SetRowCellValue(i, gvView.VisibleColumns[j], string.Format("{0} / {1}: {2}", i, j, strTextIn));

                gvView.DeleteRow(gvView.RowCount - 1);
                gvView.DeleteRow(gvView.RowCount - 1);

                gvView.EndInit();
            }

            private void Export(GridView gvView)
            {
                if (!gvView.GridControl.IsPrintingAvailable)
                {
                    MessageBox.Show("The 'DevExpress.XtraPrinting' Library is not found", "Error");
                    return;
                }
                else
                {                    
                    // Create a PrintingSystem component.
                    PrintingSystem ps = new PrintingSystem();
                    ps.ExportOptions.PrintPreview.DefaultExportFormat = PrintingSystemCommand.ExportXls;
                    ps.ExportOptions.Text.TextExportMode = TextExportMode.Text;
                    ps.ExportOptions.Xls.TextExportMode = TextExportMode.Text;
                    ps.ExportOptions.Xlsx.TextExportMode = TextExportMode.Text;

                    if (gvView.GridControl is GridControlEx)
                        ps.Document.Name = ((GridControlEx)gvView.GridControl).ExportName;

                    // Create a link that will print a control.
                    PrintableComponentLink link = new PrintableComponentLink(ps);                    

                    // Specify the control to be printed.
                    link.Component = gvView.GridControl;

                    // Generate a report.
                    link.CreateDocument();

                    link.ShowPreview();                 

                    /*// Opens the Preview window.
                    string path = Application.StartupPath + "\\Report.xlt";
                    IExportProvider provider = new ExportXlsProvider(path);
                    BaseExportLink link = gvView.CreateExportLink(provider);
                    link.ExportCellsAsDisplayText = true;
                    gvView.GridControl.ShowPrintPreview(); 
                    gvView.GridControl*/
                }
            }
        }

        private void MyGridControl_MouseUp(object sender, MouseEventArgs e)
        {
            // Get a View at the current point.
            BaseView View = GetViewAt(e.Location);
            // Retrieve information on the current View element.
            BaseHitInfo baseHI = View.CalcHitInfo(e.Location);
            GridHitInfo gridHI = baseHI as GridHitInfo;            
            //Perform any necessary logic
            if (gridHI.InRow == true && gridHI.InRowCell == false && e.Button == MouseButtons.Right)
            {
                GridViewColumnButtonMenu Menu = new GridViewColumnButtonMenu(View as GridView);
                Menu.SubMenuItems = SubMenuItems;
                Menu.OnMyClick += new GridViewColumnButtonMenu.OnMyClickEventHandler(Menu_OnMyClick);
                Menu.Init(gridHI);
                Menu.Show(gridHI.HitPoint);
            }
        }

        public void Menu_OnMyClick(object sender, EventArgs e)
        {
            if (OnMyMenuItemClick != null)
                OnMyMenuItemClick(sender, e);
        }

        /// <summary>
        /// Список пунктов меню.
        /// </summary>
        List<DXSubMenuItem> SubMenuItems;

        /// <summary>
        /// Название экспортируемого документа.
        /// </summary>
        public string ExportName;

        public delegate void OnMyMenuItemClickEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Вызывается при нажатие кнопку меню.
        /// </summary>
        public event OnMyMenuItemClickEventHandler OnMyMenuItemClick;


        public void AddSubMenuItem(DXSubMenuItem Item)
        {
            SubMenuItems.Add(Item);
        }
    }
}
