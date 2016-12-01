using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace RapidInterface
{
    public partial class DataBaseNewForm : XtraForm
    {
        public DataBaseNewForm(DBConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            SplashScreenManager.CloseForm(false);
            InitializeComponent();
        }

        DBConnection dbConnection { get; set; }

        private void DataBaseNewForm_Load(object sender, EventArgs e)
        {
            memoEdit1.Text = string.Format("Будет создана новая БД с именем \"{0}\"", dbConnection.DataBase);
            memoEdit1.Select(0, 0);
        }
    }
}