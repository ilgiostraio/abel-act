using Act.Lib;
using Act.Lib.Control;
using Act.Lib.ControllersLibrary;
using Act.Lib.Robot;
using System;
using System.Collections.Generic;
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

namespace Act.Face.FACEGui20.UI
{
    public partial class ToolBar: UserControl
    {
        FACEGui20Win parent;
        private String lastOpenFilename;
        public ToolBar()
        {
            InitializeComponent();
           
           
        }

       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            parent = (FACEGui20Win) Window.GetWindow(this);

            if (parent == null)
                throw new Exception("TopMenu UserControls should be inserted in Window before being loaded");


        }

        #region Toolbar button

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewFileButton_Click(object sender, RoutedEventArgs e)
        {
            switch (parent.visualMode)
            {
                case Mode.View:
                    break;

                case Mode.Edit:
                    try
                    {
                        foreach (SliderController sliderCtrl in parent.sliders)
                        {
                            DockPanel dp = sliderCtrl.Content as DockPanel;
                            int index = Int32.Parse(dp.Uid);
                            StackPanel sp = dp.Children[1] as StackPanel;
                            //string name = Enum.GetName(typeof(NameControls), index);

                            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sp); i++)
                            {
                                Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, i);
                                if (childVisual.GetType() == typeof(Slider))
                                {
                                    (childVisual as Slider).Value = RobotControl.CurrentMotorState[index].PulseWidthNormalized;
                                }
                            }
                            sp.IsEnabled = false;
                        }

                        //PleasureTextbox.Text = "0.000";
                        //ArousalTextbox.Text = "0.000";
                        //DominanceTextbox.Text = "0.000";
                        //NameTextbox.Text = "ExpressionName";
                    }
                    catch (RobotException fEx)
                    {
                        parent.TextError.Text = "Error occurs opening a new file";
                        ErrorDialog errorDiag = new ErrorDialog();
                        errorDiag.tbInstructionText.Text = fEx.Message;
                        errorDiag.Show();
                    }
                    break;

                case Mode.Net:
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            parent.loading = true;

            Microsoft.Win32.OpenFileDialog dlg;
            dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = parent.expressionsPath;
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML file (*.xml)|*.xml"; // Filter files by extension
            dlg.RestoreDirectory = true;

            Nullable<bool> result;
            result = dlg.ShowDialog();

            if (result == true)
            {
                lastOpenFilename = dlg.FileName;
                RobotMotion motion = RobotMotion.LoadFromXmlFormat(lastOpenFilename);

                switch (parent.visualMode)
                {
                    case Mode.View:
                        break;

                    case Mode.Edit:
                        try
                        {
                            float defValue = 0;
                            for (int i = 0; i < motion.ServoMotorsList.Count; i++)
                            {
                                defValue = motion.ServoMotorsList[i].PulseWidthNormalized;

                                DockPanel dp = parent.sliders[i].Content as DockPanel;
                                int index = Int32.Parse(dp.Uid);
                                StackPanel sp = dp.Children[1] as StackPanel;

                                for (int k = 0; k < VisualTreeHelper.GetChildrenCount(sp); k++)
                                {
                                    Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, k);
                                    if (childVisual.GetType() == typeof(Slider))
                                    {
                                        if (defValue != -1)
                                        {
                                            (dp.Children[0] as CheckBox).IsChecked = true;
                                            (childVisual as Slider).Value = defValue;
                                        }
                                        else
                                        {
                                            (childVisual as Slider).Value = RobotControl.DefaultMotorState[index].PulseWidthNormalized;
                                            (dp.Children[0] as CheckBox).IsChecked = false;
                                        }
                                    }
                                }
                            }
                            //PleasureTextbox.Text = motion.ECSCoord.Pleasure.ToString();
                            //ArousalTextbox.Text = motion.ECSCoord.Arousal.ToString();
                            //DominanceTextbox.Text = motion.ECSCoord.Dominance.ToString();
                            //NameTextbox.Text = motion.Name;
                        }
                        catch (RobotException fEx)
                        {
                            parent.TextError.Text = "Error occurs loading " + dlg.FileName.ToString().Remove(dlg.FileName.Length - 4) + " file.";
                            ErrorDialog errorDiag = new ErrorDialog();
                            errorDiag.tbInstructionText.Text = fEx.Message;
                            errorDiag.Show();
                        }
                        break;
                }
                parent.loading = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            RobotMotion motionToSave = new RobotMotion(RobotControl.CurrentMotorState.Count);
            motionToSave.Duration = parent.expressionTime;
            motionToSave.DelayTime = 0;
            //motionToSave.ECSCoord = new ECS.ECSCoordinate(Single.Parse(PleasureTextbox.Text), Single.Parse(ArousalTextbox.Text), Single.Parse(DominanceTextbox.Text));
            //motionToSave.Name = NameTextbox.Text;
            motionToSave.Priority = 10;

            switch (parent.visualMode)
            {
                case Mode.View:
                    break;

                case Mode.Edit:
                    foreach (SliderController sliderCtrl in parent.sliders)
                    {
                        DockPanel dp = sliderCtrl.Content as DockPanel;
                        int index = Int32.Parse(dp.Uid);
                        CheckBox cb = dp.Children[0] as CheckBox;
                        StackPanel sp = dp.Children[1] as StackPanel;

                        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sp); i++)
                        {
                            Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, i);
                            if (childVisual.GetType() == typeof(TextBox))
                            {
                                if ((bool)cb.IsChecked)
                                {
                                    float newValue = Convert.ToSingle((childVisual as TextBox).Text, NumberFormatInfo.InvariantInfo);
                                    motionToSave.ServoMotorsList.ElementAt(index).PulseWidthNormalized = newValue;
                                }
                                else
                                {
                                    motionToSave.ServoMotorsList.ElementAt(index).PulseWidthNormalized = -1;
                                }
                            }
                        }
                    }
                    break;
            }

            try
            {
                RobotMotion.SaveAsXmlFormat(motionToSave, lastOpenFilename);
            }
            catch (RobotException fEx)
            {
                parent.TextError.Text = "Error occurs saving " + lastOpenFilename.ToString().Remove(lastOpenFilename.Length - 4) + " expression..";
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "Expression"; // Default file name
            saveDialog.DefaultExt = ".xml";
            saveDialog.Filter = "XML file (*.xml)|*.xml"; // Filter files by extension "All Files (*.*)|*.*|XML file (*.xml)|*.xml"
            saveDialog.AddExtension = true; // Adds a extension if the user does not
            saveDialog.InitialDirectory = parent.expressionsPath;
            saveDialog.RestoreDirectory = true;

            Nullable<bool> result = saveDialog.ShowDialog();

            RobotMotion motionToSave = new RobotMotion(RobotControl.CurrentMotorState.Count);
            motionToSave.Duration = parent.expressionTime;
            motionToSave.DelayTime = 0;
            //motionToSave.ECSCoord = new ECS.ECSCoordinate(Single.Parse(PleasureTextbox.Text), Single.Parse(ArousalTextbox.Text), Single.Parse(DominanceTextbox.Text));
            //motionToSave.Name = NameTextbox.Text;
            motionToSave.Priority = 10;

            if (result == true)
            {
                string filename = saveDialog.FileName;

                switch (parent.visualMode)
                {
                    case Mode.View:
                        break;

                    case Mode.Edit:
                        foreach (SliderController sliderCtrl in parent.sliders)
                        {
                            DockPanel dp = sliderCtrl.Content as DockPanel;
                            int index = Int32.Parse(dp.Uid);
                            CheckBox cb = dp.Children[0] as CheckBox;
                            StackPanel sp = dp.Children[1] as StackPanel;

                            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sp); i++)
                            {
                                Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, i);
                                if (childVisual.GetType() == typeof(TextBox))
                                {
                                    if ((bool)cb.IsChecked)
                                    {
                                        float newValue = Convert.ToSingle((childVisual as TextBox).Text, NumberFormatInfo.InvariantInfo);
                                        motionToSave.ServoMotorsList.ElementAt(index).PulseWidthNormalized = newValue;
                                    }
                                    else
                                    {
                                        motionToSave.ServoMotorsList.ElementAt(index).PulseWidthNormalized = -1;
                                    }
                                }
                            }
                        }
                        break;
                }

                try
                {
                    motionToSave.Name = (filename.Split(new Char[] { '.' }))[0];
                    RobotMotion.SaveAsXmlFormat(motionToSave, filename);
                }
                catch (RobotException fEx)
                {
                    parent.TextError.Text = "Error occurs saving " + filename.ToString().Remove(filename.Length - 4) + " expression..";
                    ErrorDialog errorDiag = new ErrorDialog();
                    errorDiag.tbInstructionText.Text = fEx.Message;
                    errorDiag.Show();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TestExpressionButton_Click(object sender, RoutedEventArgs e)
        {
            RobotMotion motionToTest = new RobotMotion(RobotControl.CurrentMotorState.Count);
            motionToTest.Duration = parent.expressionTime;
            motionToTest.DelayTime = 0;
            //motionToTest.ECSCoord = new ECS.ECSCoordinate(Single.Parse(PleasureTextbox.Text), Single.Parse(ArousalTextbox.Text), Single.Parse(DominanceTextbox.Text));
            //motionToTest.Name = NameTextbox.Text;
            motionToTest.Priority = 10;

            switch (parent.visualMode)
            {
                case Mode.View:
                    break;

                case Mode.Edit:
                    foreach (SliderController sliderCtrl in parent.sliders)
                    {
                        DockPanel dp = sliderCtrl.Content as DockPanel;
                        int index = Int32.Parse(dp.Uid);
                        CheckBox cb = dp.Children[0] as CheckBox;

                        if ((bool)cb.IsChecked)
                        {
                            StackPanel sp = dp.Children[1] as StackPanel;
                            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sp); i++)
                            {
                                Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, i);
                                if (childVisual.GetType() == typeof(TextBox))
                                {
                                    float newValue = Convert.ToSingle((childVisual as TextBox).Text, NumberFormatInfo.InvariantInfo);
                                    motionToTest.ServoMotorsList[index].PulseWidthNormalized = newValue;
                                }
                            }
                        }
                        else
                        {
                            motionToTest.ServoMotorsList[index].PulseWidthNormalized = -1;
                        }
                    }
                    break;

                case Mode.Net:
                    break;
            }

            try
            {
                RobotControl.ExecuteMotion(motionToTest);
                parent.SBInfoBox.Text = "Testing expression..";
                //parent.StartProgressbarTime(parent.expressionTime);
            }
            catch (RobotException fEx)
            {
                parent.SBInfoBox.Text = "";
                //SBProgressBar.Value = SBProgressBar.Minimum;

                parent.TextError.Text = "Warning! " + fEx.Message;
                WarningDialog warningDiag = new WarningDialog();
                warningDiag.tbInstructionText.Text = fEx.Message;
                warningDiag.Show();
            }
        }


        /// <summary>
        /// Updates the slider values from the current motor positions (useful for expressions sent through ECS)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            //List<ServoMotor> expressionConfig = FACEBody.CurrentState;
            //List<ServoMotor> defaultConfig = new List<ServoMotor>(FACEBody.DefaultState.ServoMotorsList); //temp ***
            //float defValue = 0;

            //try
            //{
            //    for (int i = 0; i < expressionConfig.ServoMotorsList.Count; i++)
            //    {
            //        defValue = expressionConfig.ServoMotorsList.ElementAt(i).PulseWidth;

            //        DockPanel dp = sliders[i].Content as DockPanel;
            //        int index = Int32.Parse(dp.Uid);
            //        StackPanel sp = dp.Children[1] as StackPanel;

            //        for (int k = 0; k < VisualTreeHelper.GetChildrenCount(sp); k++)
            //        {
            //            Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, k);
            //            if (childVisual.GetType() == typeof(Slider))
            //            {
            //                if (defValue != -1)
            //                {
            //                    (dp.Children[0] as CheckBox).IsChecked = true;
            //                    (childVisual as Slider).Value = defValue;
            //                    //ServoMotorGroup.ExecuteMovement(index, motorPos, expressionTime);
            //                    //expressionConfig.ServoMotorsList.ElementAt(index).PulseWidth = motorPos;
            //                }
            //                else
            //                {
            //                    (childVisual as Slider).Value = defaultConfig.ServoMotorsList.ElementAt(index).PulseWidth;
            //                    (dp.Children[0] as CheckBox).IsChecked = false;
            //                }
            //            }
            //        }
            //    }

            //    //PleasureTextbox.Text = String.Format(expressionConfig.Pleasure.ToString("0.00", CultureInfo.InvariantCulture));
            //    //ArousalTextbox.Text = String.Format(expressionConfig.Arousal.ToString("0.00", CultureInfo.InvariantCulture));
            //    //DominanceTextbox.Text = String.Format(expressionConfig.Dominance.ToString("0.00", CultureInfo.InvariantCulture));
            //    //NameTextbox.Text = expressionConfig.Name;
            //}
            //catch (FACException fEx)
            //{
            //    //TextError.Text = "Error occurs opening " + filename.ToString().Remove(filename.Length - 4) + " expression.";
            //    ErrorDialog errorDiag = new ErrorDialog();
            //    errorDiag.tbInstructionText.Text = fEx.Message;
            //    errorDiag.Show();
            //}
        }

        #endregion

    }
}