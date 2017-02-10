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
using System.Xml;

namespace RFID_Reader
{
    delegate void UI_Interface();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Parallax28340Device RFID = new Parallax28340Device();
        Unlocked unlock = new Unlocked();

        SoundPlayer Mplayer         = new SoundPlayer();
        FileStream wavFile          = null;

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
                XElement root = XElement.Load("Valid.xml");   //Load XML file 
                IEnumerable<string> RFIDTags = from tag in root.Elements("RFID_Tag")    //Linq to XML to find all tag #'s
                                               where (string)tag.Attribute("ID") == RFID
                                               select (string)tag.Element("Name").Attribute("Person").Value + "\n" +
                                               (string)tag.Element("CC").Attribute("Num").Value + "\n$" + // returns the name, CC, and bill associated with tags
                                               (string)tag.Element("Bill").Attribute("Total").Value;
                foreach (string tag in RFIDTags) { //Loop to check Tags
                    PlaySound(); // Beep
                    
                    string[] info = tag.Split('\n'); //parses returned information
                    if(info[0] == "" && info[1]== "" && info[2] =="$") //checks to see if unidentified tag
                    {
                        CC form = new CC();
                        var dialogResult = form.ShowDialog(); //creates an instance of the swipe card message
                        StreamReader file = new StreamReader("temp.txt"); //loads the information from CC form
                        StringBuilder data = new StringBuilder(); 
                        data.Append(file.ReadLine()); //read in last name
                        data.Append(", ");            //make it look perty
                        data.Append(file.ReadLine()); //read in first name
                        
                        /* * * * * * * * * * * * * * * * *
                        *   WRITE TO THE XML FILE HERE   *
                        * * * * * * * * * * * * * * * * * */
                        XmlDocument doc = new XmlDocument();
                        doc.Load("Valid.xml");
                        XmlNodeList nodes= doc.SelectNodes("Valid_List/RFID_Tag");
                        foreach(XmlNode ID in nodes)
                        {
                            if (ID.Attributes["ID"].Value == RFID)
                            {
                                XmlNode name = ID.SelectSingleNode("Name");
                                name.Attributes["Person"].Value = data.ToString();
                                XmlNode cc = ID.SelectSingleNode("CC");
                                cc.Attributes["Num"].Value = file.ReadLine();
                                XmlNode bill = ID.SelectSingleNode("Bill");
                                bill.Attributes["Total"].Value = "0.00";
                                doc.Save("Valid.xml");
                            }
                        }

                        file.Close();
                        var unlooooooked = unlock.ShowDialog(); //unlocks machine
                    }
                    else
                    {
                        var dialogResult = unlock.ShowDialog(); //unlocks if tag associated
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
       
    }
}
