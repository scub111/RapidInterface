namespace RapidInterface
{
    partial class DBForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBForm));
            InitIcons = new DevExpress.Utils.ImageCollection(components);
            ((System.ComponentModel.ISupportInitialize)(InitIcons)).BeginInit();
            SuspendLayout();
            // 
            // InitIcons
            // 
            InitIcons.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("InitIcons.ImageStream")));
            InitIcons.Images.SetKeyName(0, "Filter.png");
            // 
            // DBForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Name = "DBForm";
            Size = new System.Drawing.Size(526, 359);
            ((System.ComponentModel.ISupportInitialize)(InitIcons)).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ImageCollection InitIcons;
    }
}
