using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.Design;
using DevExpress.Xpo;

namespace RapidInterface
{
    [DesignerAttribute(typeof(DBViewInterfaceDesigner))]
    public partial class DBViewInterface : DBViewBase
    {
        #region Constructors
        public DBViewInterface()
        {
            InitializeComponent();
            Destroyed = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Основной объект компонента.
        /// </summary>
        public DBInterface DBInterface { get; set; }

        /// <summary>
        /// Удален компонент.
        /// </summary>
        private bool Destroyed;

        /// <summary>
        /// Инициализация компонента через свойство.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public bool _Initialized
        {
            get
            {
                if (DBInterface != null && !Destroyed)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    InitializeVisibleComponents();
                    Destroyed = false;
                }
                else
                {
                    DestroyVisibleComponents();
                    Destroyed = true;
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Событие для нажатия правой кнопки мыши по выпадающему списку.
        /// </summary>
        public event DBInterfaceItemXPObject.RightMouseDownEventHandler RightMouseDown;
        #endregion

        #region Metods common
        /// <summary>
        /// Инициализация компонента и создание группы других компонентов.
        /// </summary>
        public void InitializeVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("InitializeDBInterfaceView");

            // Create compnonents
            DBInterface = (DBInterface)HostComponent.CreateComponent(host, typeof(DBInterface), "_dbInterface");

            DBInterface.Dock = DockStyle.Fill;
            DBInterface.InitializeVisibleComponents();

            Controls.Add(DBInterface);

            transaction.Commit();
        }

        /// <summary>
        /// Удаление компонента и других связанных с ним компонентов.
        /// </summary>
        public void DestroyVisibleComponents()
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("DestroyVisibleComponent");

            DBInterface.DestroyVisibleComponents();

            HostComponent.DestroyComponent(host, DBInterface);

            transaction.Commit();
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
        /// Фукус на искомой записи
        /// </summary>
        public void FocusRecord(XPBaseObject record)
        {
            if (DBInterface != null)
                DBInterface.FocusRecord(record);
        }
        #endregion

        #region Event_handlers
        private void DBInterfaceView_Load(object sender, EventArgs e)
        {
            if (DBInterface != null)
                DBInterface.RightMouseDown += dbInterface_RightMouseDown;
        }

        private void DBInterfaceView_FormUpdate(object sender, EventArgs e)
        {
            if (DBInterface != null)
                DBInterface.NavigatorControl_DateBaseUpdating(sender, e);
        }

        void dbInterface_RightMouseDown(object sender, XPObjectEventArgs e)
        {
            InvokeRightMouseDown(sender, e);
        }
        #endregion
    }
}
