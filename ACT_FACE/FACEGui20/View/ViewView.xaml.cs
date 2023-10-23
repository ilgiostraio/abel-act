using Act.Lib;
using Act.Lib.Control;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Act.Face.FACEGui20.View
{
    public partial class ViewView : UserControl
    {
        FACEGui20Win parent;
        string NewSetExpressionPath = @"NewSetExpressions\";

        private Color grayColor;

        public ViewView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            parent = (FACEGui20Win)Window.GetWindow(this);

            if (parent == null)
                throw new Exception("FACEditView UserControls should be inserted in Window before being loaded");
            else
            {
                grayColor = Color.FromRgb(19, 9, 109);

                parent.InitViewMode();
            }
        }

       

        /*  Expressions  */
        #region Expression
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/"+parent.config, FACEActions.Reset);

        }

        private void NeutralButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot+"/" +parent.expressionsPath + "AU_Neutral.xml", FACEActions.Neutral);

        }

        private void HappyButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/" + parent.expressionsPath + "AU_Happiness.xml", FACEActions.Happiness);

        }

        private void AngryButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/" + parent.expressionsPath + "AU_Anger.xml", FACEActions.Anger);

        }

        private void SadButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/" + parent.expressionsPath + "AU_Sadness.xml", FACEActions.Sadness);

        }

        private void DisgustButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/" + parent.expressionsPath + "AU_Disgust.xml", FACEActions.Disgust);

        }

        private void FearButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/" + parent.expressionsPath + "AU_Fear.xml", FACEActions.Fear);

        }

        private void SurpriseButton_Click(object sender, RoutedEventArgs e)
        {

            parent.LoadAndSendExpression(parent.tRobot + "/" + parent.expressionsPath + "AU_Surprise.xml", FACEActions.Surprise);

        }
        #endregion

        /*  Movements  */
        #region Movements
        public void YesMovementButton_Click(object sender, RoutedEventArgs e)
        {
            RobotControl.StartYesMovement();
            //LogEvents(FACEActions.Yes);
        }

        public void NoMovementButton_Click(object sender, RoutedEventArgs e)
        {
            RobotControl.StartNoMovement();
            //LogEvents(FACEActions.No);
        }


        public void CloseEyesButton_Click(object sender, RoutedEventArgs e)
        {
            parent.CloseEyes();
        }

        public void OpenEyesButton_Click(object sender, RoutedEventArgs e)
        {

            parent.OpenEyes();

        }

        #endregion

        #region New Set Expressions

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button1.xml");
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button2.xml");
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button3.xml");
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button4.xml");
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button5.xml");
        }
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button6.xml");
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button7.xml");
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            parent.LoadAndSendExpression(NewSetExpressionPath + "Button8.xml");
        }

        private void Window_PreviewKey(object sender, KeyEventArgs e)
        {
            if (EnabledKeyPress.IsChecked == true)
            {


                KeyConverter kc = new KeyConverter();
                string key = kc.ConvertToString(e.Key);
                Debug.WriteLine("Function linked to KeyPress: " + key);

                switch (key)
                {
                    case "1":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button1.xml");
                        break;

                    case "2":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button2.xml");
                        break;

                    case "3":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button3.xml");
                        break;

                    case "4":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button4.xml");
                        break;

                    case "5":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button5.xml");
                        break;

                    case "6":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button6.xml");
                        break;
                    case "7":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button7.xml");

                        break;
                    case "8":

                        parent.LoadAndSendExpression(NewSetExpressionPath + "Button8.xml");
                        break;


                }
            }
        }

        private void EnabledKeyPress_Click(object sender, RoutedEventArgs e)
        {
            if (EnabledKeyPress.IsChecked != true)
            {
                Button1.IsEnabled = false;
                Button2.IsEnabled = false;
                Button3.IsEnabled = false;
                Button4.IsEnabled = false;
                Button5.IsEnabled = false;
                Button6.IsEnabled = false;
                Button7.IsEnabled = false;
                Button8.IsEnabled = false;
            }
            else
            {
                Button1.IsEnabled = true;
                Button2.IsEnabled = true;
                Button3.IsEnabled = true;
                Button4.IsEnabled = true;
                Button5.IsEnabled = true;
                Button6.IsEnabled = true;
                Button7.IsEnabled = true;
                Button8.IsEnabled = true;

            }

        }
        #endregion


        #region Blinking

        private void BlinkRateBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BlinkRateBox != null)
            {

                int rate = 1;
                try
                {
                    rate = Convert.ToInt32(BlinkRateBox.Text, NumberFormatInfo.InvariantInfo);
                    if (rate < 1)
                    {
                        rate = 1;
                        BlinkRateBox.Text = String.Format(rate.ToString("0", CultureInfo.InvariantCulture));
                    }

                    if (AutomaticBlinkingCheckbox != null && AutomaticBlinkingCheckbox.IsChecked == true)
                    {
                        RobotControl.StopBlinking();
                        RobotControl.BlinkingRate = rate;
                        RobotControl.StartBlinking();
                    }
                }
                catch
                {
                    rate = 1;
                    BlinkRateBox.Text = String.Format(rate.ToString("0", CultureInfo.InvariantCulture));
                }
            }
        }

        private void BlinkTimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BlinkTimeBox != null)
            {
                int closedTime = 1;
                try
                {
                    closedTime = Convert.ToInt32(BlinkTimeBox.Text, NumberFormatInfo.InvariantInfo);
                    if (closedTime < 1)
                    {
                        closedTime = 1;
                        BlinkTimeBox.Text = String.Format(closedTime.ToString("00", CultureInfo.InvariantCulture));
                    }
                    if (AutomaticBlinkingCheckbox != null && AutomaticBlinkingCheckbox.IsChecked == true)
                    {
                        RobotControl.StopBlinking();
                        RobotControl.ClosedEyesTime = closedTime;
                        RobotControl.StartBlinking();
                    }
                }
                catch
                {
                    closedTime = 1;
                    BlinkTimeBox.Text = String.Format(closedTime.ToString("0", CultureInfo.InvariantCulture));
                }
            }
        }

        private void BlinkSpeedBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BlinkSpeedBox != null)
            {
                int speed = 1;
                try
                {
                    speed = Convert.ToInt32(BlinkSpeedBox.Text, NumberFormatInfo.InvariantInfo);
                    if (speed < 1)
                    {
                        speed = 1;
                        BlinkSpeedBox.Text = String.Format(speed.ToString("00", CultureInfo.InvariantCulture));
                    }
                    if (AutomaticBlinkingCheckbox != null && AutomaticBlinkingCheckbox.IsChecked == true)
                    {
                        RobotControl.StopBlinking();
                        RobotControl.SpeedEyesTime = speed;
                        RobotControl.StartBlinking();
                        //Brain.ClosedEyesTime = closedTime;
                    }
                }
                catch
                {
                    speed = 1;
                    BlinkSpeedBox.Text = String.Format(speed.ToString("0", CultureInfo.InvariantCulture));
                }
            }
        }

        private void CheckboxAutomaticBlinking_Checked(object sender, RoutedEventArgs e)
        {
            //FACEBody.StartBlinking(FACEBody.BlinkingRate);
            RobotControl.StartBlinking();
            //LogEvents(FACEActions.StartBlinking);

            AutomaticBlinkingParams.IsEnabled = true;

            ManualBlinkingCheckbox.IsChecked = false;
            ManualBlinkingSliderPanel.IsEnabled = false;
        }

        private void CheckboxAutomaticBlinking_Unchecked(object sender, RoutedEventArgs e)
        {
            RobotControl.StopBlinking();
            //LogEvents(FACEActions.StopBlinking);

            AutomaticBlinkingParams.IsEnabled = false;
        }

        private void CheckboxManualBlinking_Checked(object sender, RoutedEventArgs e)
        {
            AutomaticBlinkingCheckbox.IsChecked = false;
            AutomaticBlinkingParams.IsEnabled = false;

            ManualBlinkingSliderPanel.IsEnabled = true;
        }

        private void CheckboxManualBlinking_Unchecked(object sender, RoutedEventArgs e)
        {
            ManualBlinkingSliderPanel.IsEnabled = false;
        }


        #endregion


    }
}