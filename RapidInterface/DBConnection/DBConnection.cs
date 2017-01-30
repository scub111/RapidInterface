using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using System.IO;
using System.ComponentModel;
using DevExpress.XtraSplashScreen;
using System.ComponentModel.Design;
using DevExpress.Xpo.DB;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;

namespace RapidInterface
{
    #region DBClass
    [DefaultEvent("InitDAL")]
    [DesignerAttribute(typeof(DBConnectionDesigner))]
    public class DBConnection : Component, ISupportInitialize
    {
        /// <summary>
        /// Типы БД.
        /// </summary>
        public enum SQLType
        {
            [Description("Access")]
            Access = 0,
            [Description("Access 2007")]
            Access2007 = 1,
            [Description("MSSql")]
            MSSql = 2,
            [Description("MSSql Compact Edition")]
            MSSqlCE = 3,
            [Description("MySQL")]
            MySQL = 4,
            [Description("Oracle")]
            Oracle = 5,
            [Description("Firebird")]
            Firebird = 6,
            [Description("PostgreSql")]
            PostgreSql = 7,
            [Description("SQLite")]
            SQLite = 8,
            [Description("VistaDB")]
            VistaDB = 9,
            [Description("PervasiveSql")]
            PervasiveSql = 10,
            [Description("DB2")]
            DB2 = 11,
            [Description("NexusDB")]
            NexusDB = 12     
        }

        /// <summary>
        /// Инициализации интерфейса.
        /// </summary>
        /// <param name="skin"></param>
        public static void InitSkin(string skin = "DevExpress Style")
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle(skin);

            SplashScreenManager.ShowForm(null, typeof(SplashScreenEx), true, true, false);
        }

        /// <summary>
        /// Обновить схему БД.
        /// </summary>
        public static void UpdateSchema()
        {
            try
            {
                SplashScreenManager.ShowForm(null, typeof(WaitFormEx), true, true, false);
                SplashScreenManager.Default.SetWaitFormDescription("Обновляется схема базы данных...");

                XpoDefault.Session.UpdateSchema();

                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);

                XtraMessageBox.Show(
                    ex.Message, ex.Source,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

            }
        }

        public DBConnection()
        {
            DataBaseType = SQLType.Access2007;
            DataBase = "DataBase.accdb";
            PasswordNeed = false;
            LoginFormNeed = false;
            Abort = false;
            AutoCreateOption = AutoCreateOption.DatabaseAndSchema;
            SecurityEx = new Security("user", "password");

            OwnerFormShowDelegate = new OwnerShowClose(OwnerFormShowMethod);

            OwnerFormInited = false;
        }

        /// <summary>
        /// Тип БД.
        /// </summary>
        [Category("Data")]
        [DefaultValue(SQLType.Access2007)]
        [TypeConverter(typeof(EnumTypeConverter))]        
        public SQLType DataBaseType { get; set; }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        [Category("Data")]
        public string Server { get; set; }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        [DefaultValue("DataBase.accdb")]
        [Category("Data")]
        public string DataBase { get; set; }

        /// <summary>
        /// Ручной инициализации компонента.
        /// </summary>
        [DefaultValue(false)]
        [Category("Data")]
        public bool CustomInit { get; set; }

        /// <summary>
        /// Необходимость ввода пароля.
        /// </summary>
        [DefaultValue(false)]
        [Category("Security")]
        public bool PasswordNeed { get; set; }

        [Category("Security")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Security SecurityEx { get; set; }

        /// <summary>
        /// Необходимость окна ввода пользователя и пароля.
        /// </summary>
        [DefaultValue(false)]
        [Category("Security")]
        public bool LoginFormNeed { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        [Category("Security")]
        public string User { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Category("Security")]
        public string Password { get; set; }

        /// <summary>
        /// Форма, на которой размещен компонент.
        /// </summary>
        public Form OwnerForm { get; set; }
        
        /// <summary>
        /// Инициализация формы.
        /// </summary>
        public bool OwnerFormInited { get; set; }

        /// <summary>
        /// Тип создания БД.
        /// </summary>
        [Category("Data")]
        public AutoCreateOption AutoCreateOption { get; set; }

        /// <summary>
        /// Событие инициализации соединения с БД.
        /// </summary>
        public event EventHandler InitDAL;

        /// <summary>
        /// Событие при подключении к БД.
        /// </summary>
        public event EventHandler Initializing;

        /// <summary>
        /// Флаг на нажатие кнокпи "Выход".
        /// </summary>
        private bool Abort { get; set; }

        public delegate void OwnerShowClose();
        public OwnerShowClose OwnerFormShowDelegate;

        /// <summary>
        /// Форма авторизации.
        /// </summary>
        LoginForm LoginForm { get; set; }

        /// <summary>
        /// Метод закрытия окна.
        /// </summary>
        void OwnerFormShowMethod()
        {
            OwnerForm.Show();
        }

        /// <summary>
        /// Вызов события инициализации соединения с БД.
        /// </summary>
        private void InvokeInitDAL(object sender, EventArgs e)
        {
            if (InitDAL != null)
                InitDAL(sender, e);
            else
            {
                if (LoginFormNeed)
                {
                    LoginForm loginForm = new LoginForm(this);
                    LoginForm = loginForm;

                    if (DataBaseType == SQLType.Access ||
                        DataBaseType == SQLType.Access2007 ||
                        DataBaseType == SQLType.MSSqlCE ||
                        DataBaseType == SQLType.SQLite ||
                        DataBaseType == SQLType.VistaDB)
                        loginForm.lciUser.Visibility = LayoutVisibility.Never;

                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        User = loginForm.UserTextEdit.Text;
                        Password = loginForm.PasswordTextEdit.Text;
                    }
                    else
                        Abort = true;
                }

                if (!Abort)
                    Connect();
            }
        }

        /// <summary>
        /// Подключение к БД.
        /// </summary>
        private void Connect()
        {
            switch (DataBaseType)
            {
                case SQLType.Access:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            AccessConnectionProvider.GetConnectionString(DataBase, User, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            AccessConnectionProvider.GetConnectionString(DataBase),
                            AutoCreateOption);
                    break;

                case SQLType.Access2007:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            AccessConnectionProvider.GetConnectionStringACE(DataBase, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            AccessConnectionProvider.GetConnectionStringACE(DataBase, ""),
                            AutoCreateOption);
                    break;

                case SQLType.MSSql:
                    if (PasswordNeed)
                    {
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            MSSqlConnectionProvider.GetConnectionString(Server, User, Password, DataBase),
                            AutoCreateOption);

                        //string connctionString = string.Format(@"Data Source={0};Initial Catalog=test;Persist Security Info=True;User ID=sa;Password=qwe+ASDFG", Server);
                        //XpoDefault.DataLayer = XpoDefault.GetDataLayer(connctionString, AutoCreateOption);
                    }
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            MSSqlConnectionProvider.GetConnectionString(Server, DataBase),
                            AutoCreateOption);
                    break;

                case SQLType.MSSqlCE:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            MSSqlCEConnectionProvider.GetConnectionString(DataBase, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            MSSqlCEConnectionProvider.GetConnectionString(DataBase),
                            AutoCreateOption);
                    break;

                case SQLType.MySQL:
                    XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                        MySqlConnectionProvider.GetConnectionString(Server, "", "", DataBase),
                        AutoCreateOption);
                    break;

                case SQLType.Oracle:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            OracleConnectionProvider.GetConnectionString(Server, User, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            OracleConnectionProvider.GetConnectionString(Server, "", ""),
                            AutoCreateOption);
                    break;

                case SQLType.Firebird:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            FirebirdConnectionProvider.GetConnectionString(Server, User, Password, DataBase),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            FirebirdConnectionProvider.GetConnectionString(Server, "", "", DataBase),
                            AutoCreateOption);
                    break;

                case SQLType.PostgreSql:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            PostgreSqlConnectionProvider.GetConnectionString(Server, User, Password, DataBase),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            PostgreSqlConnectionProvider.GetConnectionString(Server, "", "", DataBase),
                            AutoCreateOption);
                    break;

                case SQLType.SQLite:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            SQLiteConnectionProvider.GetConnectionString(DataBase, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            SQLiteConnectionProvider.GetConnectionString(DataBase),
                            AutoCreateOption);
                    break;   

                case SQLType.VistaDB:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            VistaDBConnectionProvider.GetConnectionString(DataBase, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            VistaDBConnectionProvider.GetConnectionString(DataBase),
                            AutoCreateOption);
                    break;   

                case SQLType.PervasiveSql:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            PervasiveSqlConnectionProvider.GetConnectionString(Server, User, Password, DataBase),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            PervasiveSqlConnectionProvider.GetConnectionString(Server, "", "", DataBase),
                            AutoCreateOption);
                    break;

                case SQLType.DB2:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            DB2ConnectionProvider.GetConnectionString(Server, DataBase, User, Password),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            DB2ConnectionProvider.GetConnectionString(Server, DataBase, "", ""),
                            AutoCreateOption);
                    break; 

                case SQLType.NexusDB:
                    if (PasswordNeed)
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            NexusDBConnectionProvider.GetConnectionString(Server, User, Password, DataBase),
                            AutoCreateOption);
                    else
                        XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                            NexusDBConnectionProvider.GetConnectionString(Server, "", "", DataBase),
                            AutoCreateOption);
                    break; 

                default:
                    XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                        AccessConnectionProvider.GetConnectionString(DataBase),
                        AutoCreateOption.DatabaseAndSchema);
                    break;
            }
        }

        /// <summary>
        /// Попытка подключения к БД.
        /// </summary>
        private bool TryConnect()
        {
            try
            {
                InvokeInitDAL(this, null);
                return true;
            }
            catch
            {
                using (DataBaseErrorForm dataErrorForm = new DataBaseErrorForm())
                {
                    if (dataErrorForm.ShowDialog() == DialogResult.OK)
                        TryConnect();
                    else
                        Abort = true;
                }
            }
            return false;
        }

        /// <summary>
        /// Локальный тип БД.
        /// </summary>
        /// <param name="DataBaseType"></param>
        /// <returns></returns>
        public static bool IsLocalDataBaseType(SQLType DataBaseType)
        {
            if (DataBaseType == SQLType.Access ||
                DataBaseType == SQLType.Access2007 ||
                DataBaseType == SQLType.MSSqlCE ||
                DataBaseType == SQLType.SQLite ||
                DataBaseType == SQLType.VistaDB)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Инициализация подключения к БД.
        /// </summary>
        public bool InitDBConnection()
        {
            if (IsLocalDataBaseType(DataBaseType))
                if (File.Exists(DataBase))
                    return TryConnect();
                else
                    using (DataBaseNewForm dataNewForm = new DataBaseNewForm(this))
                    {
                        if (dataNewForm.ShowDialog() == DialogResult.OK)
                        {
                            InvokeInitDAL(this, null);
                            return true;
                        }
                        else
                            Abort = true;
                    }
            else
                return TryConnect();

            return false;
        }

        /// <summary>
        /// Расширенная инициализация подключения к БД.
        /// </summary>
        public void InitDBConnectionEx()
        {
            if (Initializing != null)
                Initializing(this, null);

            if (IsLocalDataBaseType(DataBaseType) && !DataBase.Contains("\\"))
                DataBase = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DataBase;

            InitDBConnection();

            if (!OwnerFormInited)
            {
                if (OwnerForm != null)
                {
                    OwnerForm.Load += OwnerForm_Load;
                    OwnerForm.VisibleChanged += OwnerForm_VisibleChanged;
                }
            }
            else
                OwnerForm.Invoke(OwnerFormShowDelegate);
        }

        public void BeginInit()
        {

        }

        public void EndInit()
        {
            if (!DesignMode && !CustomInit)
            {
                InitDBConnectionEx();
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

        void OwnerForm_Load(object sender, EventArgs e)
        {
            OwnerFormInited = true;
        }

        private void OwnerForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Abort)
                OwnerForm.Close();
        }

        public void ShowLoginForm()
        {
            if (LoginForm != null)
            {
                //LoginForm.TopMost = true;
                LoginForm.Activate();
                //LoginForm.TopMost = false;
            }
        }
    }
    #endregion
}
