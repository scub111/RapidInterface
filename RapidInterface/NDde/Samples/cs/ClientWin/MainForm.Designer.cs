namespace ClientWin
{
    partial class MainForm
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
            this.btnRead = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtTopic = new System.Windows.Forms.TextBox();
            this.txtItem = new System.Windows.Forms.TextBox();
            this.displayTextBox = new System.Windows.Forms.TextBox();
            this.btnParse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(26, 110);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(285, 23);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "Прочитать";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(248, 27);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(114, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Подключиться...";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(26, 12);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(100, 20);
            this.txtServer.TabIndex = 3;
            this.txtServer.Text = "View";
            // 
            // txtTopic
            // 
            this.txtTopic.Location = new System.Drawing.Point(26, 39);
            this.txtTopic.Name = "txtTopic";
            this.txtTopic.Size = new System.Drawing.Size(100, 20);
            this.txtTopic.TabIndex = 4;
            this.txtTopic.Text = "Tagname";
            // 
            // txtItem
            // 
            this.txtItem.Location = new System.Drawing.Point(26, 84);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(285, 20);
            this.txtItem.TabIndex = 5;
            this.txtItem.Text = "K_BF20";
            // 
            // displayTextBox
            // 
            this.displayTextBox.Location = new System.Drawing.Point(26, 246);
            this.displayTextBox.Name = "displayTextBox";
            this.displayTextBox.Size = new System.Drawing.Size(395, 20);
            this.displayTextBox.TabIndex = 6;
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(26, 157);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(285, 23);
            this.btnParse.TabIndex = 1;
            this.btnParse.Text = "Прочитать";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 277);
            this.Controls.Add(this.displayTextBox);
            this.Controls.Add(this.txtItem);
            this.Controls.Add(this.txtTopic);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.btnRead);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "DDE Sample Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtTopic;
        private System.Windows.Forms.TextBox txtItem;
        private System.Windows.Forms.TextBox displayTextBox;
        private System.Windows.Forms.Button btnParse;

    }
}

