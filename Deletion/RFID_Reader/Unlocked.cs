using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RFID_Reader
{
    public partial class Unlocked : Form
    {
        public Unlocked()
        {
            InitializeComponent();
            StreamReader file = new StreamReader("temp.txt");
            StringBuilder data = new StringBuilder();
            data.Append(file.ReadLine());
            data.Append(", ");
            data.Append(file.ReadLine());
            label1.Text = "Hello " + data.ToString();
            file.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
