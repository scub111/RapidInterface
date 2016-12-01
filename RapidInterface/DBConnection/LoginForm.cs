using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace RapidInterface
{
    public partial class LoginForm : XtraForm
    {
        public LoginForm(DBConnection dbConnection)
        {
            SplashScreenManager.CloseForm(false);
            InitializeComponent();
        }

        DBConnection dbConnection { get; set; }
    }
}