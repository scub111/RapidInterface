namespace RapidInterface
{
    partial class SplashScreenEx
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenEx));
            marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(marqueeProgressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureEdit2.Properties)).BeginInit();
            SuspendLayout();
            // 
            // marqueeProgressBarControl1
            // 
            marqueeProgressBarControl1.EditValue = 0;
            marqueeProgressBarControl1.Location = new System.Drawing.Point(23, 231);
            marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            marqueeProgressBarControl1.Size = new System.Drawing.Size(404, 12);
            marqueeProgressBarControl1.TabIndex = 5;
            // 
            // labelControl2
            // 
            labelControl2.Location = new System.Drawing.Point(23, 206);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(47, 13);
            labelControl2.TabIndex = 7;
            labelControl2.Text = "Запуск...";
            // 
            // pictureEdit2
            // 
            pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            pictureEdit2.Location = new System.Drawing.Point(12, 12);
            pictureEdit2.Name = "pictureEdit2";
            pictureEdit2.Properties.AllowFocused = false;
            pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pictureEdit2.Properties.ShowMenu = false;
            pictureEdit2.Size = new System.Drawing.Size(426, 180);
            pictureEdit2.TabIndex = 9;
            // 
            // SplashScreen1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(450, 266);
            Controls.Add(pictureEdit2);
            Controls.Add(labelControl2);
            Controls.Add(marqueeProgressBarControl1);
            Name = "SplashScreen1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(marqueeProgressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureEdit2.Properties)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
    }
}
