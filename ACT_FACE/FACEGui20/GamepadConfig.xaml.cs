using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Act.Face.FACEGui20
{
    /// <summary>
    /// Interaction logic for GamepadConfig.xaml
    /// </summary>
    public partial class GamepadConfig : Window
    {
        FACEGui20Win parent;

        List<string> motorListName = new List<string>();

        public class Item
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Item(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        List<string> exp = new List<string>()
        {
            "None" , "Anger" , "Disgust" , "Fear", "Happiness", "Neutral" , "Sadness", "Surprise" 
        };
        public GamepadConfig(FACEGui20Win p)
        {
            InitializeComponent();

            parent = p;


            List<Item> itemsMotors = new List<Item>();
            itemsMotors.Add(new Item(" ", "55"));

            foreach (Act.Lib.ServoMotor s in parent.currentSmState)
                itemsMotors.Add(new Item(s.Name, s.Channel.ToString()));

            cbLeftTrigger.ItemsSource = itemsMotors;
            cbLeftTrigger.DisplayMemberPath = "Name";
            cbLeftTrigger.SelectedValuePath = "Value";

            cbRightTrigger.ItemsSource = itemsMotors;
            cbRightTrigger.DisplayMemberPath = "Name";
            cbRightTrigger.SelectedValuePath = "Value";


            cbX.ItemsSource = exp;
            cbA.ItemsSource = exp;
            cbY.ItemsSource = exp;
            cbB.ItemsSource = exp;
            cbRightShoulder.ItemsSource = exp;
            cbRightThumb.ItemsSource = exp;

            cbLeftTrigger.SelectedValue = parent.configGamepad.LeftTrigger;
            cbRightTrigger.SelectedValue = parent.configGamepad.RightTrigger;

            cbX.Text = parent.configGamepad.X;
            cbY.SelectedValue = parent.configGamepad.Y;
            cbA.SelectedValue = parent.configGamepad.A;
            cbB.SelectedValue = parent.configGamepad.B;

            cbDPadDown.SelectedValue = parent.configGamepad.DPadDown;
            cbDPadLeft.SelectedValue = parent.configGamepad.DPadLeft;
            cbDPadRight.SelectedValue = parent.configGamepad.DPadRight;
            cbDPadUp.SelectedValue = parent.configGamepad.DPadUp;

            cbLeftShoulder.SelectedValue = parent.configGamepad.LeftShoulder;
            cbLeftThumb.SelectedValue = parent.configGamepad.LeftThumb;
            cbLeftTrigger.SelectedValue = parent.configGamepad.LeftTrigger;
            cbRightShoulder.SelectedValue = parent.configGamepad.RightShoulder;
            cbRightThumb.SelectedValue = parent.configGamepad.RightThumb;
            cbRightTrigger.SelectedValue = parent.configGamepad.RightTrigger;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            parent.configGamepad.A = cbA.Text;
            parent.configGamepad.X = cbX.Text;
            parent.configGamepad.Y = cbY.Text;
            parent.configGamepad.B = cbB.Text;
            parent.configGamepad.DPadDown = cbDPadDown.Text;
            parent.configGamepad.DPadLeft = cbDPadLeft.Text;
            parent.configGamepad.DPadRight = cbDPadRight.Text;
            parent.configGamepad.DPadUp = cbDPadUp.Text;

            parent.configGamepad.LeftShoulder = cbLeftShoulder.Text;
            parent.configGamepad.LeftThumb = cbLeftThumb.Text;
            parent.configGamepad.LeftTrigger = Convert.ToInt32(cbLeftTrigger.SelectedValue);
            parent.configGamepad.RightShoulder =cbRightShoulder.Text;
            parent.configGamepad.RightThumb = cbRightThumb.Text;
            parent.configGamepad.RightTrigger = Convert.ToInt32(cbRightTrigger.SelectedValue);


            string json = JsonConvert.SerializeObject(parent.configGamepad);

            //write string to file
            System.IO.File.WriteAllText("configGamepad.json", json);

            //using (StreamWriter file = File.CreateText("configGamepad.json"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    //serialize object directly into file stream
            //    serializer.Serialize(file, parent.configGamepad);
            //}
          
          
           

            try
            {
                using (Stream fStream = new FileStream("configGamepad.json", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter xmlWriter = new StreamWriter(fStream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(xmlWriter, parent.configGamepad);

                    }

                }
                MessageBox.Show("The config has been updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during update the config");
            }
        }
    }
}
