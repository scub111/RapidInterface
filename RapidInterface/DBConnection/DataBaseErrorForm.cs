using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace RapidInterface
{
    public partial class DataBaseErrorForm : XtraForm
    {
        public DataBaseErrorForm()
        {
            SplashScreenManager.CloseForm(false);
            InitializeComponent();
        }
    }
}