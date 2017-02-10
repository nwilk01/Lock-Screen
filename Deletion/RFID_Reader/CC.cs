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



            textBox1.TextChanged += txtbox_TextChanged; //adds addition to event handler on form creation
        }
        private void txtbox_TextChanged(object sender, EventArgs e) //wrote to addition to base event handler
        {
             if (textBox1.Text[textBox1.TextLength - 1] == '?') //only takes needed info
            {
                parser(never.ToString()); // calls parser function
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
            string[] fname;
            string num;
            string[] info;
            string[] buffer;
            info = input.Split('B');
            buffer = info[1].Split('^');
            name = buffer[1].Split('/');
            fname = name[1].Split(' ');
            num = buffer[0];
            System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"); //write info to file to pass
            file.WriteLine(name[0]);
            file.WriteLine(fname[0]);
            file.WriteLine(num);
            file.Close();
        }
    }
}
