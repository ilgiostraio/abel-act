using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Globalization;
using System.IO.Ports;
using Act.Lib.Robot;
using Act.Lib;
using Act.Lib.Control;
using Act.Lib.ServoController;

namespace Act.Face.FACEGui20
{
    /// <summary>
    /// Interaction logic for FACEConfigWin.xaml
    /// </summary>
    public partial class MotorConfig : Window
    {
      

        private RobotMotion defaultConfig;
        private Type selectedEnum;        
        private string filenameConfig = "";
        private SettingsDialog setDialog;
        private int time;

        ServoMotor currentServoMotor = null;

        //Stores the value of the ProgressBar
        //private int currChannel;
        private int currAbsoluteValue;
        private float currRelativeValue;

        // Saved indexes for comboboxes
        private string savedDeviceType = "";
        private string savedPortName = "";
        private int savedBitRate = 115200;
        private int savedDataBits = 8;
        private Parity savedParity = Parity.None;
        private StopBits savedStopBits = StopBits.One;
        private Handshake savedHandshake = Handshake.None;

        // Saved params for comboboxes
        private int savedMinVal=500;
        private int savedMaxVal=2500;

        private Dictionary<KeyGesture, RoutedEventHandler> gests = new Dictionary<KeyGesture, RoutedEventHandler>();

        public MotorConfig(ServoMotor sm, String fileConfig)
        {

            InitializeComponent();

            currentServoMotor = sm;
            filenameConfig = fileConfig;

            try
            {
                Init();
            }
            catch (RobotException fEx)
            {
                TextInfo.Text = "Error load init.";
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
}


        /// <summary>
        /// Initial configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Init()
        {
            
            if (System.IO.File.Exists(filenameConfig))
                defaultConfig = new RobotMotion(RobotControl.LoadConfigFile(filenameConfig), TimeSpan.FromMilliseconds(0));
            else 
                throw new RobotException("File Confing not found.");

            //Recupero seriale dei servo controller
            foreach (String s in CommandPololu.ListServoControllers)
            {
                ServoControllerCombo.Items.Add(s);
                TextServoController.Text += s + " | ";
            }

        
            
            for (int i = 0; i < 18; i++) 
            {
                ServoControllerPortCombo.Items.Add(i);
            }

            lblServoMotor.Content = currentServoMotor.Name + " #" + currentServoMotor.Channel;
            lblCurrentMinValue.Content = currentServoMotor.MinValue;
            lblCurrentMaxValue.Content = currentServoMotor.MaxValue;
            ServoControllerCombo.SelectedItem = currentServoMotor.SerialSC;
            ServoControllerPortCombo.SelectedItem = currentServoMotor.PortSC;
            currRelativeValue = currentServoMotor.PulseWidthNormalized;
            currAbsoluteValue = currentServoMotor.MappingOnMinMaxInterval(currRelativeValue);
            PositionBox.Text = String.Format(currAbsoluteValue.ToString("000", CultureInfo.InvariantCulture));
            // PositionBox.Text = currAbsoluteValue.ToString();




           

            ServoControllerCombo.IsEnabled = true;
            ServoControllerPortCombo.IsEnabled = true;

            if (!SliderControl.IsEnabled)
                SliderControl.IsEnabled = true;

        }



        #region Menu Buttons

        private void FileSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                defaultConfig.ServoMotorsList.Where(a => a.Channel == currentServoMotor.Channel).Select(c => { c = currentServoMotor; return c; });
                    
                RobotControl.SaveConfigFile(defaultConfig, filenameConfig);
                TextInfo.Text = "File saved correctly.";
                
            }
            catch (RobotException fEx)
            {
                TextInfo.Text = "Error occurs saving configuration file.";
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "Expression"; // Default file name
            saveDialog.DefaultExt = ".xml"; // Default file extension
            saveDialog.Filter = "XML file (*.xml)|*.xml"; // Filter files by extension "All Files (*.*)|*.*|XML file (*.xml)|*.xml"
            saveDialog.AddExtension = true; // Adds a extension if the user does not
            saveDialog.RestoreDirectory = true; // Restores the selected directory, next time

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                string filename = saveDialog.FileName;

                try
                {
                    defaultConfig.ServoMotorsList.Where(a => a.Channel == currentServoMotor.Channel).Select(c => { c = currentServoMotor; return c; });

                    defaultConfig.Name = filename;
                    RobotControl.SaveConfigFile(defaultConfig, filename);
                    TextInfo.Text = "File saved correctly.";
                    
                }
                catch (RobotException fEx)
                {
                    TextInfo.Text = "Error occurs saving configuration file.";
                    ErrorDialog errorDiag = new ErrorDialog();
                    errorDiag.tbInstructionText.Text = fEx.Message;
                    errorDiag.Show();
                }
            }
        }

        private void TestConfigButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                                    
                    defaultConfig.Duration = time;
                    RobotControl.ExecuteMotion(defaultConfig);
                    TextInfo.Text = "Testing configuration...";
               
            }
            catch (RobotException fEx)
            {
                TextInfo.Text = "Error in testing configuration.";
                WarningDialog warningDiag = new WarningDialog();
                warningDiag.tbInstructionText.Text = fEx.Message;
                warningDiag.Show();
            }
        }

        #endregion


        #region Settings Dialog

      
     

        /// <summary>
        /// Display the current device settings on the statusbar.
        /// </summary>
        /// <param name="selectedDevice"></param>
        /// <param name="msgDevice"></param>
        /// <param name="minLimit"></param>
        /// <param name="maxLimit"></param>
        /// <param name="msgLimit"></param>
        private void DisplayDeviceWarning(string selectedDevice, string msgDevice, int minLimit, int maxLimit, string msgLimit)
        {
            if (msgDevice != "")
            {
                WarningDialog warningDialog = new WarningDialog();
                warningDialog.tbInstructionText.Text = msgDevice;
                warningDialog.Show();
            }
            else
            {
                savedDeviceType = selectedDevice;
            }

            if (msgLimit != "")
            {
                WarningDialog warningDialog = new WarningDialog();
                warningDialog.tbInstructionText.Text = msgLimit;
                warningDialog.Show();
            }
            else
            {
                //FACEBody.MinLimit = minLimit;
                //FACEBody.MaxLimit = maxLimit;
                //SliderControl.Minimum = FACEBody.MinLimit;
                //SliderControl.Maximum = FACEBody.MaxLimit;
                //SliderControl.Value = FACEBody.MinLimit;
                //savedMinVal = FACEBody.MinLimit;
                //savedMaxVal = FACEBody.MaxLimit;
            }
        }
        

 

        /// <summary>
        /// Display the currente message on the statusbar.
        /// </summary>
        /// <param name="msg">The message to be set on the statusbar</param>
        private void DisplayCurrentMessage(string msg)
        {
            //TextError.Text = msg;
            TextInfo.Text = msg;
        }


       


        /// <summary>
        /// Updates the time box in the status bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SBTimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                time = Convert.ToInt32(SBTimeBox.Text, NumberFormatInfo.InvariantInfo);
                if (time < 0)
                {
                    time = 0;
                    SBTimeBox.Text = String.Format(time.ToString("", CultureInfo.InvariantCulture));
                }
            }
            catch
            {
                time = 0;
                SBTimeBox.Text = String.Format(time.ToString("0", CultureInfo.InvariantCulture));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Close();
        }

        #endregion


        #region Combobox

    

        private void ServoControllerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServoControllerCombo.SelectedValue != null)
            {
                if(currentServoMotor.SerialSC.Equals(ServoControllerCombo.SelectedValue))
                    ManualPanel.IsEnabled = true;

                SetSC.IsEnabled=true;
            }
            else
            {
                ManualPanel.IsEnabled = false;
                SetSC.IsEnabled = false;
            }
        }

      
        private void SetSC_Click(object sender, RoutedEventArgs e)
        {
            //(defaultConfig.ServoMotorsList[currChannel]).SerialSC = ServoControllerCombo.SelectedItem.ToString();
            //(defaultConfig.ServoMotorsList[currChannel]).PortSC = (int)ServoControllerPortCombo.SelectedItem;
            ManualPanel.IsEnabled = true;
        }

        /// <summary>
        /// Resets the combobox content.
        /// </summary>
        /// <param name="combo"></param>
        private void ClearCombobox(ComboBox combo)
        {
            for (int i = combo.Items.Count - 1; i >= 0; i--)
                combo.Items.RemoveAt(i);
        }

        #endregion

    


        #region Set Min/Max and controls

        /// <summary>
        /// Set the lower limit of the servo motor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetMinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currAbsoluteValue >= RobotControl.MinLimit && currAbsoluteValue <= RobotControl.MaxLimit)
                {
                    currentServoMotor.MinValue = currAbsoluteValue;
                    lblCurrentMinValue.Content = currAbsoluteValue;
                }
                else
                {
                  
                    WarningDialog warningDiag = new WarningDialog();
                    warningDiag.tbInstructionText.Text = "The position value is out of the limits.";
                    warningDiag.Show();
                }
            }
            catch
            {
                currAbsoluteValue = RobotControl.MinLimit;
                PositionBox.Text = String.Format(currAbsoluteValue.ToString("000", CultureInfo.InvariantCulture));
            }
        }


        /// <summary>
        /// Set the upper limit of the servo motor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetMaxButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currAbsoluteValue >= RobotControl.MinLimit && currAbsoluteValue <= RobotControl.MaxLimit)
                {
                    currentServoMotor.MaxValue = currAbsoluteValue;
                    lblCurrentMaxValue.Content = currAbsoluteValue;
                }
                else
                {
                   
                    WarningDialog warningDiag = new WarningDialog();
                    warningDiag.tbInstructionText.Text = "The position value is out of the limits.";
                    warningDiag.Show();
                }
            }
            catch
            {
                currAbsoluteValue = RobotControl.MinLimit;
                PositionBox.Text = String.Format(currAbsoluteValue.ToString("000", CultureInfo.InvariantCulture));
            }
        }

        #endregion

        private void PositionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                currAbsoluteValue = Convert.ToInt32(PositionBox.Text, NumberFormatInfo.InvariantInfo);

                if (currAbsoluteValue >= RobotControl.MinLimit && currAbsoluteValue <= RobotControl.MaxLimit)
                    SliderControl.Value = currentServoMotor.MappingOnUnitaryInterval(currAbsoluteValue);
                else
                    PositionBox.Text = String.Format(currentServoMotor.PulseWidth.ToString());
            }
            catch
            {
                time = 0;
                PositionBox.Text = String.Format(time.ToString("0", CultureInfo.InvariantCulture));
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sliderCtrl = e.OriginalSource as Slider;

			try
            {
                currAbsoluteValue = currentServoMotor.MappingOnMinMaxInterval((float)SliderControl.Value);
                if (currAbsoluteValue >= RobotControl.MinLimit && currAbsoluteValue <= RobotControl.MaxLimit)
                {
                    
                    PositionBox.Text = String.Format(currAbsoluteValue.ToString("000", CultureInfo.InvariantCulture));
                }
                else
                {
                    if (currAbsoluteValue < RobotControl.MinLimit)
                    {
                        currAbsoluteValue = RobotControl.MinLimit;
                        PositionBox.Text = String.Format((RobotControl.MinLimit).ToString("000", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        currAbsoluteValue = RobotControl.MaxLimit;
                        PositionBox.Text = String.Format((RobotControl.MaxLimit).ToString("000", CultureInfo.InvariantCulture));
                        
                    }
                }

                currentServoMotor.PulseWidthNormalized = (float)SliderControl.Value;

            }
            catch
            {
                time = 0;
                PositionBox.Text = String.Format(time.ToString("000", CultureInfo.InvariantCulture));
            }

           
          

        }

 


        #region Test Values

        /// <summary>
        /// Test min/max values for the current servo motor considering the absolutes min/max values
        /// (FACEBody.MinLimit - FACEBody.MaxLimit)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestMinMaxButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (currAbsoluteValue >= RobotControl.MinLimit && currAbsoluteValue <= RobotControl.MaxLimit)
                {
                    //send position directly to the servomotor, without animator
                    int port = (int)currentServoMotor.PortSC;
                    string serial = currentServoMotor.SerialSC;

                    currentServoMotor.SendPosition(time);
                    //FACEBody.SendRawCommand(currChannel, port, currAbsoluteValue, serial, time);
                }
                else
                {
                    if (currAbsoluteValue < RobotControl.MinLimit)
                    {
                        currAbsoluteValue = RobotControl.MinLimit;
                        PositionBox.Text = String.Format((RobotControl.MinLimit).ToString("000", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        currAbsoluteValue = RobotControl.MaxLimit;
                        PositionBox.Text = String.Format((RobotControl.MaxLimit).ToString("000", CultureInfo.InvariantCulture));
                    }

                    WarningDialog warningDiag = new WarningDialog();
                    warningDiag.tbInstructionText.Text = "The position value is out of the limits.";
                    warningDiag.Show();
                }
                
            }
            catch (RobotException fEx)
            {
                // Set Info box
                TextInfo.Text = "";

                WarningDialog warningDiag = new WarningDialog();
                warningDiag.tbInstructionText.Text = fEx.Message;
                warningDiag.Show();
            }
        }


        /// <summary>
        /// Test min/max values for the current servo motor considering the relative min/max values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestValueButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                int minVal = (int)currentServoMotor.MinValue;
                int maxVal = (int)currentServoMotor.MaxValue;
                //int port=(int)currentServoMotor.PortSC;
                //string serial = defaultConfig.ServoMotorsList[currChannel].SerialSC;

                if ((currAbsoluteValue >= minVal && currAbsoluteValue <= maxVal) 
                    || (currAbsoluteValue >= maxVal && currAbsoluteValue <= minVal))
                {
                    //send position directly to the servomotor, without animator
                    // FACEBody.SendRawCommand(currChannel, currAbsoluteValue, time);
                    currentServoMotor.SendPosition(time);
                    //update the local config to be saved
                    //defaultConfig.ServoMotorsList[currChannel].PulseWidthNormalized = FACEBody.CurrentMotorState[currChannel].PulseWidthNormalized;
                }
                else
                {
                    WarningDialog warningDiag = new WarningDialog();
                    warningDiag.tbInstructionText.Text = "The position value is out of the servo motor limits.";
                    warningDiag.Show();
                }
                
            }
            catch (RobotException fEx)
            {
                WarningDialog warningDiag = new WarningDialog();
                warningDiag.tbInstructionText.Text = fEx.Message;
                warningDiag.Show();
            }
        }

        #endregion

       

       

       

    }

}