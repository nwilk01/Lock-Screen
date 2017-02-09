using System;
using System.Windows;
using System.Windows.Media;
using System.Media;
using System.IO;
using Parallax28340;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web;
using System.Text;
using System.Windows.Input;

namespace RFID_Reader
{
    delegate void UI_Interface();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Parallax28340Device RFID = new Parallax28340Device();
        Unlocked form = new Unlocked();

        SoundPlayer Mplayer         = new SoundPlayer();
        FileStream wavFile          = null;
        StringBuilder input = new StringBuilder();
        KeyConverter memer = new KeyConverter();
        List<string> memes = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            RFID.Init(DisplayRFID, DisplayStatus);

            try
            {
                wavFile = new FileStream("./Sounds/Beep.wav",
                                      FileMode.Open,
                                      FileAccess.Read);

            }
            catch (Exception e)
            {
                DisplayErrorMsg(e.Message);
                wavFile = null;
            }
        }


        public void DisplayErrorMsg(string ErrorMsg)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            (UI_Interface)delegate()
            {
                //TextBoxRFID.AppendText(ErrorMsg);
            });
        }

        public void DisplayRFID(string RFID)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            (UI_Interface)delegate()
            {
                /* if (TextBoxRFID.Text.Length > 0)
                 {
                     TextBoxRFID.AppendText(Environment.NewLine);
                     TextBoxRFID.ScrollToEnd();
                 }

                 TextBoxRFID.AppendText(RFID);
                 */
                XElement root = XElement.Load("Valid.xml");
                IEnumerable<string> RFIDTags = from tag in root.Elements("RFID_Tag") //(string)tag.Attribute("ID").Value
                                               where (string)tag.Attribute("ID") == RFID
                                               select (string)tag.Element("Name").Attribute("Person").Value + "\n" +
                                               (string)tag.Element("CC").Attribute("Num").Value + "\n$" +
                                               (string)tag.Element("Bill").Attribute("Total").Value;
                foreach (string tag in RFIDTags) {
                    PlaySound();
                    //form.Show();
                    
                    string[] info = tag.Split('\n');
                    if(info[0] == "" && info[1]== "" && info[2] =="$")
                    {
                        CC form = new CC();
                        form.Show();

                        //var message = string.Join(Environment.NewLine, memes);
                        //MessageBox.Show(message);
                    }
                    else
                    {
                        MessageBox.Show("Fail");
                    }
                    
                }


               
            });
        }

        public void DisplayStatus(Boolean connected)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            (UI_Interface)delegate()
            {
                if (connected)
                {
                    textBoxStatus.Background = Brushes.Green;
                    textBoxStatus.Text       = "Connected";
                }
                else
                {
                    textBoxStatus.Background = Brushes.Red;
                    textBoxStatus.Text       = "Not Connected";
                }
            });
        }


        public void PlaySound()
        {
            if (Properties.Settings.Default.BeepEnabled)
            {
                if (wavFile != null)
                {
                    Mplayer.Stream = wavFile;
                    Mplayer.Play();
                }
            }
        }

        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RFID.End();
        }

        private void ClearButtonPressed(object sender, RoutedEventArgs e)
        {
            //TextBoxRFID.Clear();

        }

        private void checkBoxBeepEnabled_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        private void CC(object sender, KeyEventArgs e)
        {
            string yolo;
            Key k = (Key)e.Key;
            yolo = memer.ConvertToString(k);
            input.Append(yolo);
            memes.Add(yolo); 
                
        }
    }
}
