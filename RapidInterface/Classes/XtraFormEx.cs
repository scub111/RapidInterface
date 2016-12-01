using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RapidInterface
{
    public class XtraFormEx : XtraForm
    {
        public XtraFormEx()
        {

        }

        /// <summary>
        /// Подключение к БД.
        /// </summary>
        [Category("Data")]
        public DBConnection DBConnection { get; set; }

        /// <summary>
        /// Событие на клик по трею.
        /// </summary>
        public event EventHandler TrayClick;

        /// <summary>
        /// Оповещние о событии клика по трею.
        /// </summary>
        public void InvokeTrayClick()
        {
            if (TrayClick != null)
                TrayClick(this, EventArgs.Empty);
        }
    }
}
