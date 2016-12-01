using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NDde.Client;

namespace ClientWin
{
    public partial class MainForm : Form
    {
        private DdeClient client;

        public MainForm()
        {
            InitializeComponent();

            //client.Advise += client_Advise;
            //client.Disconnected += client_Disconnected;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

        }

        private void client_Advise(object sender, DdeAdviseEventArgs args)
        {
            //displayTextBox.Text = "OnAdvise: " + args.Text;
        }

        private void client_Disconnected(object sender, DdeDisconnectedEventArgs args)
        {
            displayTextBox.Text = 
                "OnDisconnected: " +
                "IsServerInitiated=" + args.IsServerInitiated.ToString() + " " +
                "IsDisposed=" + args.IsDisposed.ToString();
        }



        private void btnConnect_Click(object sender, EventArgs e)
        {
            client = new DdeClient(txtServer.Text, txtTopic.Text);
            try
            {
                // Connect to the server.  It must be running or an exception will be thrown.
                client.Connect();

                // Advise Loop
                //client.StartAdvise("myitem", 1, true, 60000);
                //client.StartAdvise("myitem2", 1, true, 60000);
                if (client.IsConnected)
                    btnConnect.Text = "Yes";

            }
            catch (Exception ex)
            {
                displayTextBox.Text = "MainForm_Load: " + ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                btnRead.Text = client.Request(txtItem.Text, 1000);
                displayTextBox.Text = client.Request(txtItem.Text, 1000);
            }
            catch (Exception ex)
            {
                displayTextBox.Text = "Parse: " + ex.Message;
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            double temp = 0;
            try
            {
                temp = double.Parse(client.Request(txtItem.Text, 1000));
                displayTextBox.Text = client.Request(txtItem.Text, 1000);
            }
            catch (Exception ex)
            {
                displayTextBox.Text = "Parse: " + ex.Message;
            }
            btnParse.Text = temp.ToString();
        }

    } // class

} // namespace