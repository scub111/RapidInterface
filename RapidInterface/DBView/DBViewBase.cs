using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RapidInterface
{
    public partial class DBViewBase : UserControl
    {
        public DBViewBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Название формы.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Собыите на обновление формы.
        /// </summary>
        public event EventHandler FormUpdate;

        /// <summary>
        /// Событие на изменение языка интерфейса.
        /// </summary>
        public static event EventHandler LanguageUIChanged;

        /// <summary>
        /// Вызов события обновления формы.
        /// </summary>
        public void InvokeFormUpdate(object sender, EventArgs e)
        {
            if (FormUpdate != null)
                FormUpdate(sender, e);
        }

        /// <summary>
        /// Вызов события изменения языка интерфейса.
        /// </summary>
        public static void InvokeLanguageUIChanged()
        {
            if (LanguageUIChanged != null)
                LanguageUIChanged(null, EventArgs.Empty);
        }
    }
}
