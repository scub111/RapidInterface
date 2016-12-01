using System;
using System.Collections.Generic;
using DevExpress.XtraSplashScreen;

namespace RapidInterface
{
    public partial class SplashScreenEx : SplashScreen
    {
        public SplashScreenEx()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}