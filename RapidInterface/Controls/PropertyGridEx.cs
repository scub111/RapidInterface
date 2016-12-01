using System;

namespace RapidInterface.Controls
{
    internal class PropertyGridEx : System.Windows.Forms.PropertyGrid
    {
        private System.ComponentModel.Container components;

        public PropertyGridEx()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        public void ShowEvents(bool show)
        {
            ShowEventsButton(show);
        }

        public bool DrawFlat
        {
            get { return DrawFlatToolbar; }
            set { DrawFlatToolbar = value; }
        }

        /*
        protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Value == null)
            {
                (((Component)SelectedObject).Site).Events.Remove(e.OldValue);
            }
        }
         */
    }
}
