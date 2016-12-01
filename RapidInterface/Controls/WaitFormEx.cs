using System;
using System.Collections.Generic;
using DevExpress.XtraWaitForm;

namespace RapidInterface
{
    public partial class WaitFormEx : WaitForm
    {
        public WaitFormEx()
        {
            InitializeComponent();
            progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }

        private void progressPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}