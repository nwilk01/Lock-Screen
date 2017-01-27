using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parallax28340;
using System.Windows.Threading;

namespace System_Lock
{
    delegate void UI_Interface();

    public partial class Lock : Form
    {
        Parallax28340Device RFID = new Parallax28340Device();

        public Lock()
        {
            InitializeComponent();

            RFID.Init(DisplayRFID, DisplayStatus);
        }

        public void DisplayRFID(string RFID)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            (UI_Interface)delegate ()
            {
                /*if (TextBoxRFID.Text.Length > 0)
                {
                    TextBoxRFID.AppendText(Environment.NewLine);
                    TextBoxRFID.ScrollToEnd();
                }*/

                textBox2.Text= RFID;

                //PlaySound();
            });
        }

        public void DisplayStatus(Boolean connected)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            (UI_Interface)delegate ()
            {
                if (connected)
                {
                    textBox1.BackColor = Color.Green;
                    textBox1.Text = "Connected";
                }
                else
                {
                    textBox1.BackColor = Color.Red;
                    textBox1.Text = "Not Connected";
                }
            });
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
