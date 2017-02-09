using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace RFID_Reader
{
    public partial class CC : Form
    {
        StringBuilder never = new StringBuilder();
        public CC()
        {
            InitializeComponent();
            textBox1.Focus();
            //Cursor.Hide();
            textBox1.BackColor = this.BackColor;
            textBox1.ForeColor = this.BackColor;



            textBox1.TextChanged += txtbox_TextChanged;
        }
        private void txtbox_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text[textBox1.TextLength - 1] == '?')
            {
                parser(never.ToString());
                this.Close();

            }
            else
            {
                never.Append(textBox1.Text[textBox1.TextLength - 1].ToString());
            }
        }

        private void parser(string input)
        {
            string[] name;
            string num;
            string[] info;
            string[] buffer;
            info = input.Split('B');
            buffer = info[1].Split('^');
            name = buffer[1].Split('/');
            num = buffer[0];
        }
    }
}
