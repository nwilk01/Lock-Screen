using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Num.Focus();
        }

        public void CC_TextChanged(object sender, TextChangedEventArgs e)
        {
            //https://www.dotnetperls.com/textbox-wpf
            //get control of raised event
            var textBox = sender as TextBox;
            string input = textBox.Text;
            if(input[input.Length-1]=='?')
            {
                parse(input);
                this.Close();
            }
        }

        private void parse(string Input)
        {
            //Parsing the Number
            string[] temp = Input.Split('^');
            string[] number = temp[0].Split('B');

            //Parsing the Name
            string[] fullname = temp[1].Split('^');
            string[] namesplit = fullname[0].Split('/');

            //Writing the info to a file
            System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt");
            file.WriteLine(namesplit[0]);
            file.WriteLine(namesplit[1]);
            file.WriteLine(number[1]);
            file.Close();
        }

    }
}
