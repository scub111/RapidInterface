using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;

namespace RapidInterface
{
    public partial class DateEditEx : DateEdit
    {
        public DateEditEx()
        {
            InitializeComponent();
        }

        private void DateEditEx_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 'T' ||
                e.KeyChar == 't' ||
                e.KeyChar == 'Е' ||
                e.KeyChar == 'е')
            {
                (sender as DateEdit).EditValue = DateTime.Now;
                e.Handled = true;
            }
        }
    }
}
