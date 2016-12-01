using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;

namespace RapidInterface
{
    public partial class RepositoryItemDateEditEx : RepositoryItemDateEdit
    {
        public RepositoryItemDateEditEx()
        {
            InitializeComponent();
        }

        private void RepositoryItemDateEditEx_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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
