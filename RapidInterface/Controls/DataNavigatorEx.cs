using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.Xpo;
using System.Windows.Forms;

namespace RapidInterface
{
    public partial class DataNavigatorEx : DevExpress.XtraEditors.DataNavigator
    {
        #region Constructors
        public DataNavigatorEx()
        {
            InitializeComponent();
        }

        public DataNavigatorEx(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Название таблицы.
        /// </summary>
        public string TableCaption { get; set; }

        /// <summary>
        /// Наличие ошибки.
        /// </summary>
        public bool Error { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// Событие на необходимость обновление данных.
        /// </summary>
        public event EventHandler DateBaseUpdating;

        /// <summary>
        /// Событие, возникающее перед сохранением данных.
        /// </summary>
        public event EventHandler DataBaseSaving;

        /// <summary>
        /// Событие, возникающее после сохранения данных.
        /// </summary>
        public event EventHandler DataBaseSaved;
        #endregion
        
        #region Event handlers
        public void DataNavigatorEx_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            Error = false;
            XPCollection xpcBase = DataSource as XPCollection;

            switch (e.Button.ButtonType)
            {
                case NavigatorButtonType.EndEdit:
                    {
                        try
                        {
                            if (DataBaseSaving != null)
                                DataBaseSaving(sender, EventArgs.Empty);

                            if (xpcBase.Session is UnitOfWork)
                                (xpcBase.Session as UnitOfWork).CommitChanges();
                            else
                                xpcBase.Session.Save(xpcBase);

                            if (DataBaseSaved != null)
                                DataBaseSaved(sender, EventArgs.Empty);
                        }
                        catch (System.Exception ex)
                        {
                            Error = true;
                            XtraMessageBox.Show(
                                ex.Message,
                                ex.Source,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);
                        }
                        finally
                        {
                            if (!Error)
                            {
                                if (sender == this)
                                    DataNavigatorEx_ButtonClick(null, new NavigatorButtonClickEventArgs(Buttons.CancelEdit));

                                XtraMessageBox.Show(
                                    string.Format("Данные таблицы \"{0}\" сохранены.", TableCaption),
                                    "Информация",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                            }
                            else
                            {
                                XtraMessageBox.Show(
                                        "Произошла ошибка сохранения.",
                                        "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button1);
                            }
                        }

                        break;
                    }
                case NavigatorButtonType.CancelEdit:
                    {
                        object currentRecord = xpcBase[Position];
                        try
                        {
                            if (DateBaseUpdating != null)
                                DateBaseUpdating(this, EventArgs.Empty);
                            else
                            {
                                xpcBase.Session.DropIdentityMap();
                                xpcBase.Reload();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show(
                                ex.InnerException.Message,
                                ex.InnerException.Source,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);
                        }

                        if (currentRecord is XPObject)
                            Position = XPObjectEx.FindRecordPossition(xpcBase, (XPObject)currentRecord);

                        break;
                    }
                case NavigatorButtonType.Remove:
                    {
                        if (Position == 0)
                        {
                            //e.Handled = true;
                            Buttons.EndEdit.Enabled = true;
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
