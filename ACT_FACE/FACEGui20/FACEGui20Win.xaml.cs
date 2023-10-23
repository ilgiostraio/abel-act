using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;
using System.Windows.Threading;
using System.Threading;
using System.Xml.Linq;
using System.Configuration;
using Newtonsoft.Json;


using Act.Lib;
using Act.Lib.Animator;
using Act.Lib.Control;
using Act.Lib.ControllersLibrary;
using Act.Lib.FaceControl;
using Act.Lib.Robot;
using Act.Lib.ServoController;

namespace Act.Face.FACEGui20
{


   
    /// <summary>
    /// Interaction logic for FACEGui20Win.xaml
    /// </summary>
    public partial class FACEGui20Win : Window
    {
        private string motionsPath = @"Motions\";
        public  string expressionsPath = @"Expressions\";
        private string logsPath = @"Logs\";
        private string testPath = @"Expressions\";    
        //private string animationsPath = @"Animations\";

        public List<ServoMotor> currentSmState;
        public List<RobotMotion> ecsMotions;
        public string config;


      
        public int expressionTime;
        public int NeckTime;
        private double speed;
        private double startTimeUI; //time for log

        public Mode visualMode;
        private TypeRobot typeRobot = TypeRobot.FACE;

        public TypeRobot tRobot {
            get { return typeRobot; }
        }


        public SliderController[] sliders;

        private Dictionary<KeyGesture, RoutedEventHandler> gests = new Dictionary<KeyGesture, RoutedEventHandler>(); // per le scorciatoie da tastiera

        private delegate void UpdateProgressBarTimeDelegate(DependencyProperty dp, Object value);

        //public delegate void DialogYesButtonEventHandler(object sender, EventArgs e);
        //public delegate void DialogNoButtonEventHandler(object sender, EventArgs e);
        //public delegate void DialogCancelButtonEventHandler(object sender, EventArgs e);


        private ECS ecs;
       
        public Gamepad configGamepad;

        public bool yarpExpressionOn = false;

        private System.Threading.Thread animation;

        public FACEGui20Win()
        {


            //var dllDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/lib";
            //Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);
           
            InitializeComponent();


            loadRobotConfig();


            InitGuiTool();
            InitECSMode();
            InitGamePad();
            //InitLog();

            Show();
        }

        private void loadRobotConfig() 
        {
            if (typeRobot == TypeRobot.FACE)
            {
                FACERobotControl face = new FACERobotControl(); //serve a precompilare il robot controll
                config = RobotControl.TypeRobot + ConfigurationManager.AppSettings["ConfigMotorFACE"].ToString();
             
            }   
            else
            {
                AbelRobotControl abel = new AbelRobotControl();
                config = RobotControl.TypeRobot + ConfigurationManager.AppSettings["ConfigMotorAbel"].ToString();//"Config.xml";
                //config = "C:\\Users\\abel\\Documents\\NON TOCCARE\\ACT\\ACT_FACE\\FACEGui20\\Abel\\Config\\Config.xml";
            }
            SBInfoBox.Text = config;

            RobotControl.LoadConfigFile(config);

            String ecs_path = RobotControl.TypeRobot.ToString();
            Console.WriteLine(ecs_path);
            if (typeRobot == TypeRobot.FACE)
                ecs_path += "/ECS_FACE.xml";
            else
                ecs_path += "/ECS_Abel.xml";

            ecs = ECS.LoadFromXmlFormat(ecs_path);

        }

        #region Init

        /// <summary>
        /// Initialize Gui window
        /// </summary>
        private void InitGuiTool()
        {
            visualMode = Mode.Edit;


            currentSmState = RobotControl.CurrentMotorState;

            sliders = new SliderController[RobotControl.NumberOfMotors];

            expressionTime = Convert.ToInt32(SBTimeBox.Text);
            NeckTime = Convert.ToInt32(SBTimeNeckBox.Text);
            speed =  Convert.ToDouble(txtSpeed.Text);

            //gests.Add(new KeyGesture(Key.T, ModifierKeys.Control), SettingsButton_Click);
            //FACEBody.VideoCamProblem += new EventHandler<WarningEventArgs>(Body_VideoCamProblem);
            //updatePbTimeDelegate = new UpdateProgressBarTimeDelegate(SBProgressBar.SetValue);


            if (typeRobot == TypeRobot.FACE)
            {
                AbelView.Visibility = Visibility.Hidden;
                FACEView.Visibility = Visibility.Visible;
            }
            else
            {
                FACEView.Visibility = Visibility.Hidden;
                AbelView.Visibility = Visibility.Visible;

            }


        }


        /// <summary>
        /// Initialize ECS mode panel
        /// </summary>
        private void InitECSMode()
        {
           
            foreach (ECSMotor ecsM in ecs.ECSMotorList)
            {
                ecsM.FillMap();
            }

            ecsMotions = new List<RobotMotion>();
            DirectoryInfo dir = new DirectoryInfo(RobotControl.TypeRobot + "\\" + expressionsPath);
            foreach (FileInfo fi in dir.GetFiles("*.xml"))
            {
                RobotMotion f = RobotMotion.LoadFromXmlFormat(fi.FullName);
                ecsMotions.Add(f);
                Console.WriteLine(f);
            }
        }

        private void InitGamePad() 
        {
            try
            {
                using (StreamReader r = new StreamReader("./" + RobotControl.TypeRobot + "/Config/configGamepad.json"))
                {
                    string json = r.ReadToEnd();
                    configGamepad = JsonConvert.DeserializeObject<Gamepad>(json);
                } 

            }
            catch (Exception ex)
            {

                throw new RobotException("Some error occurs during load configGamepad file.", ex, typeof(FACEGui20Win).FullName);
            }
        }


        /// <summary>
        /// Initialize View mode panel
        /// </summary>
        public void InitViewMode()
        {
            //int index = Convert.ToInt32(EyeLidsLowerMB.Uid);
            //string name = EyeLidsLowerMB.Name.Substring(0, EyeLidsLowerMB.Name.Length - 2);
            //EyeLidsLowerMB.SliderLabel.Foreground = new SolidColorBrush(grayColor);
            //EyeLidsLowerMB.SliderLabel.Content = name + " (" + index + ")";
            //EyeLidsLowerMB.SliderCheckbox.IsChecked = false;
            //EyeLidsLowerMB.SliderControl.Value = FACEBody.CurrentMotorState[index].PulseWidthNormalized;
            //EyeLidsLowerMB.SliderValueChanged += new RoutedEventHandler(SliderCtlr_SliderValueChanged);
            //EyeLidsLowerMB.CheckboxChecked += new RoutedEventHandler(SliderCtrl_CheckboxChecked);
            //EyeLidsLowerMB.CheckboxUnchecked += new RoutedEventHandler(SliderCtrl_CheckboxUnchecked);

            //index = Convert.ToInt32(EyeLidsUpperMB.Uid);
            //name = EyeLidsLowerMB.Name.Substring(0, EyeLidsUpperMB.Name.Length - 2);
            //EyeLidsUpperMB.SliderLabel.Foreground = new SolidColorBrush(grayColor);
            //EyeLidsUpperMB.SliderLabel.Content = name + " (" + index + ")";
            //EyeLidsUpperMB.SliderCheckbox.IsChecked = false;
            //EyeLidsUpperMB.SliderControl.Value = FACEBody.CurrentMotorState[index].PulseWidthNormalized;
            //EyeLidsUpperMB.SliderValueChanged += new RoutedEventHandler(SliderCtlr_SliderValueChanged);
            //EyeLidsUpperMB.CheckboxChecked += new RoutedEventHandler(SliderCtrl_CheckboxChecked);
            //EyeLidsUpperMB.CheckboxUnchecked += new RoutedEventHandler(SliderCtrl_CheckboxUnchecked);
        }


        /// <summary>
        /// Initialize Edit mode panel
        /// </summary>
        /// <param name="panel"></param>
        public void InitEditMode(StackPanel panel)
        {

           

            foreach (Control ctrl in panel.Children)
            {
                if (ctrl.GetType() == typeof(SliderController))
                {
                    SliderController sliderCtrl = ctrl as SliderController;

                    int index = typeRobot == TypeRobot.FACE ? Convert.ToInt32(sliderCtrl.Uid) : Convert.ToInt32(sliderCtrl.Uid) - 1;

                    string name = sliderCtrl.Name.Substring(0, sliderCtrl.Name.Length);

                    DockPanel dp = sliderCtrl.Content as DockPanel;
                    dp.Uid = index.ToString();
                    dp.Name = name + dp.Name;

                    CheckBox cb = dp.Children[0] as CheckBox;
                    if (!String.IsNullOrEmpty(RobotControl.CurrentMotorState[index].SerialSC))
                        cb.IsChecked = true;
                    else
                        cb.IsChecked = false;

                    StackPanel sp = dp.Children[1] as StackPanel;
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sp); i++)
                    {
                        Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, i);

                        if (childVisual.GetType() == typeof(Label))
                        {
                            Label label = childVisual as Label;
                            label.Uid = index.ToString();
                            label.Content = "(" + RobotControl.CurrentMotorState[index].Channel + ") " + name ;
                        }
                        else if (childVisual.GetType() == typeof(Slider))
                        {
                            Slider slider = childVisual as Slider;
                            slider.Uid = index.ToString();
                            slider.Value = RobotControl.CurrentMotorState[index].PulseWidthNormalized;
                        }
                        else if (childVisual.GetType() == typeof(TextBox))
                        {
                            TextBox textbox = childVisual as TextBox;
                            textbox.Uid = index.ToString();
                        }
                    }

                    if(!String.IsNullOrEmpty(RobotControl.CurrentMotorState[index].SerialSC))
                        sp.IsEnabled = true;
                    else
                        sp.IsEnabled = false;
                    sliderCtrl.SliderValueChanged += new RoutedEventHandler(SliderCtlr_SliderValueChanged);
                    sliderCtrl.CheckboxChecked += new RoutedEventHandler(SliderCtrl_CheckboxChecked);
                    sliderCtrl.CheckboxUnchecked += new RoutedEventHandler(SliderCtrl_CheckboxUnchecked);
                    sliders[index] = sliderCtrl;
                }
            }
        }
        public void InitNewEditMode(StackPanel panel)
        {
            List<ServoMotor> m;
           
            if(panel.Name.Contains("Right"))
                m = RobotControl.CurrentMotorState.FindAll(c => c.Name.Contains("Right")).ToList();
            else
                m = RobotControl.CurrentMotorState.FindAll(c => c.Name.Contains("Left")).ToList();

            foreach (ServoMotor ctrl in m)
            {


                SliderController sliderCtrl = new SliderController();

                int index = ctrl.Channel;
                string name = ctrl.Name;

                sliderCtrl.Uid = index.ToString();
                sliderCtrl.Name = name;

                    

                DockPanel dp = sliderCtrl.Content as DockPanel;
                dp.Uid = index.ToString();
                dp.Name = name + dp.Name;

                CheckBox cb = dp.Children[0] as CheckBox;
                if (!String.IsNullOrEmpty(RobotControl.CurrentMotorState[index].SerialSC))
                    cb.IsChecked = true;
                else
                    cb.IsChecked = false;

                StackPanel sp = dp.Children[1] as StackPanel;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sp); i++)
                {
                    Visual childVisual = (Visual)VisualTreeHelper.GetChild(sp, i);

                    if (childVisual.GetType() == typeof(Label))
                    {
                        Label label = childVisual as Label;
                        label.Uid = index.ToString();
                        label.Content = name + " (" + index + ")";
                    }
                    else if (childVisual.GetType() == typeof(Slider))
                    {
                        Slider slider = childVisual as Slider;
                        slider.Uid = index.ToString();
                        slider.Value = RobotControl.CurrentMotorState[index].PulseWidthNormalized;
                    }
                    else if (childVisual.GetType() == typeof(TextBox))
                    {
                        TextBox textbox = childVisual as TextBox;
                        textbox.Uid = index.ToString();
                    }
                }

                if (!String.IsNullOrEmpty(RobotControl.CurrentMotorState[index].SerialSC))
                    sp.IsEnabled = true;
                else
                    sp.IsEnabled = false;

                sliderCtrl.SliderValueChanged += new RoutedEventHandler(SliderCtlr_SliderValueChanged);
                sliderCtrl.CheckboxChecked += new RoutedEventHandler(SliderCtrl_CheckboxChecked);
                sliderCtrl.CheckboxUnchecked += new RoutedEventHandler(SliderCtrl_CheckboxUnchecked);
                sliderCtrl.MouseDoubleClick += new MouseButtonEventHandler(SliderController_MouseDoubleClick);

                sliders[index] = sliderCtrl;

                panel.Children.Add(sliderCtrl);


            }
        }


        



        private void InitLog()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("it-IT", false).DateTimeFormat);
            string time = String.Format("{0:HH.mm.ss}", DateTime.Now);
            logsPath += "log_" + date + "_" + time + ".xml";

            if (!File.Exists(logsPath))
            {
                XDocument xDoc = new XDocument(new XDeclaration("1.0", System.Text.Encoding.UTF8.WebName, "yes"));

                XElement xRoot = new XElement(XName.Get("Session"));
                xRoot.SetAttributeValue("Date", date);
                DateTime dt = DateTime.Now;
                xRoot.SetAttributeValue("Time", String.Format("{0:HH:mm:ss.ffff}", dt));
                xRoot.SetAttributeValue("Timestamp", String.Format("{0:0.000000}", FromDateToDouble(dt)).Replace(",", "."));
                xDoc.Add(xRoot);

                startTimeUI = FromDateToDouble(dt);

                XElement enums = new XElement(XName.Get("Enums"));
                foreach (FACEActions item in Enum.GetValues(typeof(FACEActions)))
                {
                    XElement xmlElem = new XElement("FACEActions");
                    xmlElem.SetAttributeValue("Type", Enum.GetName(typeof(FACEActions), item));
                    xmlElem.SetAttributeValue("Value", (int)item);
                    enums.Add(xmlElem);
                }
                xRoot.Add(enums);

                XElement actions = new XElement(XName.Get("Actions"));
                xRoot.Add(actions);

                xDoc.Save(logsPath);
            }
            else
            {
                XDocument xDoc = XDocument.Load(logsPath);
                startTimeUI = Double.Parse(xDoc.Root.Attribute(XName.Get("Timestamp")).Value.Replace(".", ","));
            }
        }


        #endregion



        public void RobotSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Application.Current.MainWindow.IsInitialized == false)
                return;

           

            CheckboxAnimator.IsChecked = false;  //stop animator 
            CheckboxYarpExp.IsChecked = false;    //stop yarpexpression


            switch (((ComboBox)e.OriginalSource).SelectedValue)
            {
                case "FACE":
                    typeRobot = TypeRobot.FACE;
                    break;
                default:
                    typeRobot = TypeRobot.Abel;
                    break;
            }

            loadRobotConfig();
            InitGuiTool();
            InitECSMode();
           

        }

        

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void UpdateSliders(List<ServoMotor>cs)
        {
            try
            {

                foreach (SliderController sliderControl in sliders)
                {
                    DockPanel dp = sliderControl.Content as DockPanel;
                    int index = Int32.Parse(dp.Uid);

                    if (cs[index].PulseWidthNormalized != -1)
                    {
                        sliderControl.SliderValueChanged -= new RoutedEventHandler(SliderCtlr_SliderValueChanged);


                        sliderControl.SliderControl.Value = cs[index].PulseWidthNormalized;

                        sliderControl.SliderValueChanged += new RoutedEventHandler(SliderCtlr_SliderValueChanged);
                      
                    }
                }

             
            }
            catch (RobotException fEx)
            {
                //TextError.Text = "Error occurs loading " + filename.ToString().Remove(filename.Length - 4) + " expression.";
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
        }


        public void LoadAndSendExpression(string fileName, FACEActions actionType)
        {
            try
            {
                SBInfoBox.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    SBInfoBox.Text = "Testing " + fileName.ToString().Remove(fileName.Length - 4) + " expression..";
                }));
                RobotControl.ExecuteFile(fileName, 10);



                //LogEvents(actionType);
                //StartProgressbarTime(currExpression.Face.Time);

            }
            catch (RobotException fEx)
            {
                TextError.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    TextError.Text = "Error occurs testing " + fileName.ToString().Remove(fileName.Length - 4) + " expression..";
                }));
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
        }

        public void LoadAndSendExpression(string fileName)
        {
            try
            {
                if (ComPorts.OpenedPort)
                {
                    SBInfoBox.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                    {
                        SBInfoBox.Text = "Testing " + fileName.ToString().Remove(fileName.Length - 4) + " expression..";
                    }));
                    RobotControl.ExecuteFile(fileName, 10);
                }
                else
                {
                    WarningDialog warningDialog = new WarningDialog();
                    warningDialog.tbInstructionText.Text = "There are not opened ports!";
                    warningDialog.Show();
                }

            }
            catch (RobotException fEx)
            {
                TextError.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    TextError.Text = "Error occurs testing " + fileName.ToString().Remove(fileName.Length - 4) + " expression..";
                }));
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
        }


        public void SetNewPositionMotors(List<ServoMotor> ListMotors, TimeSpan t)
        {
            RobotMotion motionToTest = new RobotMotion(ListMotors, t);


            try
            {
                //SBInfoBox.Text = "Testing expression..";
                RobotControl.ExecuteMotion(motionToTest);
                //StartProgressbarTime(expressionConfig.Time);

                //// Trovo tutti quelli a -1 e aggiorno con il vecchio valore
                //foreach (ServoMotor sm in ListMotors.FindAll(a => a.PulseWidth == -1.0))
                //    ListMotors.Find(a => a.Channel == sm.Channel).PulseWidth = currentSmState.Find(a => a.Channel == sm.Channel).PulseWidth;


                //currentSmState = ListMotors;

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                {
                    //
                    UpdateSliders(ListMotors);
                }));

            }
            catch (RobotException fEx)
            {
                SBInfoBox.Text = "";
                //SBProgressBar.Value = SBProgressBar.Minimum;

                TextError.Text = "Warning! " + fEx.Message;
                WarningDialog warningDiag = new WarningDialog();
                warningDiag.tbInstructionText.Text = fEx.Message;
                warningDiag.Show();
            }


        }


        #endregion


        #region Movements

        public void CloseEyes()
        {
            LoadAndSendExpression(motionsPath + "CloseEyes.xml", FACEActions.None);
        }

        public void OpenEyes()
        {
            LoadAndSendExpression(motionsPath + "OpenEyes.xml", FACEActions.None);
        }
        #endregion


        #region edit mode
        public bool loading = false; //testare: mettere loading = true in Load button


        private void SliderCtlr_SliderValueChanged(object sender, RoutedEventArgs e)
        {
            if (!loading)
            {
                SliderController sliderCtrl = e.Source as SliderController;
                DockPanel dp = sliderCtrl.Content as DockPanel;
                int index = Int32.Parse(dp.Uid);

                Slider sliderObj = e.OriginalSource as Slider;
                try
                {
                    float value = Convert.ToSingle(sliderObj.Value, NumberFormatInfo.InvariantInfo);
                    
                    RobotControl.ExecuteSingleMovement(index, value, TimeSpan.FromMilliseconds(expressionTime), 10, TimeSpan.FromMilliseconds(0));
                    //StartProgressbarTime(expressionConfig.Time);
                }
                catch (RobotException fEx)
                {
                    SBInfoBox.Text = "";
                    //SBProgressBar.Value = SBProgressBar.Minimum;

                    TextError.Text = "Warning! " + fEx.Message;
                    WarningDialog warningDiag = new WarningDialog();
                    warningDiag.tbInstructionText.Text = fEx.Message;
                    warningDiag.Show();
                }
            }
        }


        private void SliderCtrl_CheckboxChecked(object sender, RoutedEventArgs e)
        {
            SliderController sliderCtrl = e.Source as SliderController;
            DockPanel dp = sliderCtrl.Content as DockPanel;
            StackPanel sp = dp.Children[1] as StackPanel;
            sp.IsEnabled = true;
        }

        private void SliderCtrl_CheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            SliderController sliderCtrl = e.Source as SliderController;
            DockPanel dp = sliderCtrl.Content as DockPanel;
            StackPanel sp = dp.Children[1] as StackPanel;
            sp.IsEnabled = false;
        }


        public void EditServoMotor(object sender)
        {
            int s = Convert.ToInt32((sender as SliderController).Uid);

            ServoMotor sm = RobotControl.DefaultMotorState.Find(a => a.Channel == s);

            MotorConfig mc = new MotorConfig(sm, config);
            mc.ShowDialog();

            RobotControl.LoadConfigFile(config);
            currentSmState = RobotControl.CurrentMotorState;

            UpdateSliders(currentSmState);
        }

        private void SliderController_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //EditServoMotor(sender);

        }

    

        #endregion

            

        public void ECSEventHandler(object sender, RoutedEventArgs e)
        {
            Point p = (Point)e.OriginalSource;

            RobotMotion motionToTest = new RobotMotion(RobotControl.CurrentMotorState.Count);
            foreach (ECSMotor m in ecs.ECSMotorList)
            {
                float f = ecs.GetECSValue(m.Channel, (float)p.X, (float)p.Y);
                motionToTest.ServoMotorsList[m.Channel].PulseWidthNormalized = f;
                Console.WriteLine(f);
            }

            motionToTest.Duration = expressionTime;
            motionToTest.Priority = 10;
            motionToTest.ECSCoord = new ECS.ECSCoordinate((float)p.X, (float)p.Y, 0);


            SetNewPositionMotors(motionToTest.ServoMotorsList, TimeSpan.FromMilliseconds(expressionTime));

        }


        private void LookAtEventHandler(object sender, RoutedEventArgs e)
        {
            Point p = (Point)e.OriginalSource;
            float x = (float)p.X;
            float y = (float)p.Y;

            RobotMotion motionToTest = new RobotMotion(RobotControl.CurrentMotorState.Count);

            int lookAtDuration = 120; //the lookat function works every 110msec so it can't act longer requests
            float minAmplitudeX = 0.002f, minAmplitudeY = 0.002f, maxAmplitudeX = 1.0f, maxAmplitudeY = 1.0f;

            float old_PosX = RobotControl.CurrentMotorState[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized;
            float old_PosY = RobotControl.CurrentMotorState[(int)AbelRobotControl.MotorsNames.EyelidTopRight].PulseWidthNormalized;


            motionToTest.ServoMotorsList.Where(w => w.PulseWidthNormalized != -1).ToList().ForEach(s => s.PulseWidthNormalized = -1);
            motionToTest.Duration = lookAtDuration;
            motionToTest.Priority = 10;

            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized = old_PosX;
            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyelidTopRight].PulseWidthNormalized = old_PosY;

            if (Math.Abs(x - old_PosX) > minAmplitudeX || Math.Abs(y - old_PosY) > minAmplitudeY)
            {
                if (Math.Abs(x - old_PosX) > minAmplitudeX)
                {
                    if (Math.Abs(x - old_PosX) < maxAmplitudeX)
                    {
                        motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized = x;
                        motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeIOLeft].PulseWidthNormalized = x;
                    }
                    else
                    {
                        if (x - old_PosX > 0)
                        {
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized = old_PosX + maxAmplitudeX;
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeIOLeft].PulseWidthNormalized = old_PosX + maxAmplitudeX;
                        }
                        else
                        {
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized = old_PosX - maxAmplitudeX;
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeIOLeft].PulseWidthNormalized = old_PosX - maxAmplitudeX;
                        }
                    }
                }

                if (Math.Abs(y - old_PosY) > minAmplitudeY)
                {
                    if (Math.Abs(x - old_PosY) < maxAmplitudeY)
                    {   
                        motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyelidTopRight].PulseWidthNormalized = y;
                        motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUDLeft].PulseWidthNormalized = y;
                    }
                    else
                    {
                        if (y - old_PosY > 0)
                        {
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyelidTopRight].PulseWidthNormalized = old_PosY + maxAmplitudeY;
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUDLeft].PulseWidthNormalized = old_PosY + maxAmplitudeY;
                        }
                        else
                        {
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyelidTopRight].PulseWidthNormalized = old_PosY - maxAmplitudeY;
                            motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.EyeUDLeft].PulseWidthNormalized = old_PosY - maxAmplitudeY;
                        }
                    }
                }

     

            }




            SetNewPositionMotors(motionToTest.ServoMotorsList, TimeSpan.FromMilliseconds(NeckTime));

       
        }


        #region PreviewKey events

        private float turnNeckValue, upperNodNeckValue;

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Left && e.Key != Key.Right)
            return;

            upperNodNeckValue = RobotControl.CurrentMotorState[(int)FACERobotControl.FaceTrackingControls.UpperNod].PulseWidthNormalized;
            turnNeckValue = RobotControl.CurrentMotorState[(int)FACERobotControl.FaceTrackingControls.Turn].PulseWidthNormalized;

            float step = 0.01f;

            switch (e.Key)
            {
                case Key.Up:
                    if (upperNodNeckValue < 1)
                        upperNodNeckValue += step;
                    break;
                case Key.Down:
                    if (upperNodNeckValue > 0)
                        upperNodNeckValue -= step;
                    break;
                case Key.Left:
                    if (turnNeckValue < 1)
                        turnNeckValue += step;
                    break;
                case Key.Right:
                    if (turnNeckValue > 0)
                        turnNeckValue -= step;
                    break;
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    RobotControl.ExecuteSingleMovement((int)FACERobotControl.FaceTrackingControls.UpperNod, upperNodNeckValue, TimeSpan.FromMilliseconds(expressionTime), 10, TimeSpan.FromMilliseconds(0));
                    break;
                case Key.Down:
                    RobotControl.ExecuteSingleMovement((int)FACERobotControl.FaceTrackingControls.UpperNod, upperNodNeckValue, TimeSpan.FromMilliseconds(expressionTime), 10, TimeSpan.FromMilliseconds(0));
                    break;
                case Key.Left:
                    RobotControl.ExecuteSingleMovement((int)FACERobotControl.FaceTrackingControls.Turn, turnNeckValue, TimeSpan.FromMilliseconds(expressionTime), 10, TimeSpan.FromMilliseconds(0));
                    break;
                case Key.Right:
                    RobotControl.ExecuteSingleMovement((int)FACERobotControl.FaceTrackingControls.Turn, turnNeckValue, TimeSpan.FromMilliseconds(expressionTime), 10, TimeSpan.FromMilliseconds(0));
                    break;
            }
        }


        protected override void OnPreviewKeyDown(KeyEventArgs args)
        {
            foreach (KeyGesture gest in gests.Keys)
            {
                if (gest.Matches(null, args))
                {
                    gests[gest](this, args);
                    args.Handled = true;
                }
            }
        }



        #endregion

        #region Test


        private DispatcherTimer clockTimer;
        private DateTime startClockTime;
        private TimeSpan timeDiff;

        private string logName;
        private DateTime startTime;
        private DateTime lastExpressionTime;
        private DateTime answerTime;
        private RadioButton selected;

        private int answer;
        private int countTick = 0;
        private int idExpression = 0;
        private List<int> outputList;

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public static readonly RoutedEvent TickEvent = EventManager.RegisterRoutedEvent("KeySpacePressed",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DispatcherTimer));

        public enum FACEAnswers
        {
            Pride = 0, Happiness, Embarrassment, Neutral, Surprise, Disgust, Pain, Pity, Contempt, Sadness,
            Interest, Shame, Fear, Excitement, Anger, Other
        };

        #region Logs

        private void InitTestLog()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd", new System.Globalization.CultureInfo("it-IT", false).DateTimeFormat);
            string time = String.Format("{0:HH.mm.ss}", DateTime.Now);
            logName = "TestFACE_" + date + "_" + time + ".xml";
            logsPath += logName;

            if (!File.Exists(logsPath))
            {
                XDocument xDoc = new XDocument(new XDeclaration("1.0", System.Text.Encoding.UTF8.WebName, "yes"));

                XElement xRoot = new XElement(XName.Get("Test"));
                xRoot.SetAttributeValue("Date", date);
                startTime = DateTime.Now;
                lastExpressionTime = startTime;
                xRoot.SetAttributeValue("Time", String.Format("{0:HH:mm:ss.ffff}", startTime));
                xRoot.SetAttributeValue("Timestamp", String.Format("{0:0.000000}", FromDateToDouble(startTime)).Replace(",", "."));
                xDoc.Add(xRoot);

                XElement xmlElem = new XElement("PersonalInfo");
                xmlElem.SetAttributeValue("Name", TextboxName.Text);
                xmlElem.SetAttributeValue("Surname", TextboxSurname.Text);
                xmlElem.SetAttributeValue("Birth", TextboxDay.Text + "/" + TextboxMonth.Text + "/" + TextboxYear.Text);
                xmlElem.SetAttributeValue("Age", (2012 - Int32.Parse(TextboxYear.Text)).ToString());
                xmlElem.SetAttributeValue("Sex", ((bool)MRadioButton.IsChecked) ? "M" : "F");
                xmlElem.SetAttributeValue("Faculty", TextboxFaculty.Text);
                xmlElem.SetAttributeValue("Job", TextboxJob.Text);
                xDoc.Root.Add(xmlElem);

                XElement actions = new XElement(XName.Get("Events"));
                xRoot.Add(actions);

                xDoc.Save(logsPath);
            }
            else
            {
                XDocument xDoc = XDocument.Load(logsPath);
            }
        }

        private void LogFaceEvents(FACEActions actionType)
        {
            try
            {
                XDocument xDoc = XDocument.Load(logsPath);

                XElement xmlElem = new XElement("Action");
                xmlElem.SetAttributeValue("Type", "Expression");
                xmlElem.SetAttributeValue("Value", actionType);
                xmlElem.SetAttributeValue("Index", (int)actionType);
                xmlElem.SetAttributeValue("Timestamp", String.Format("{0:0.000000}", FromDateToDouble(lastExpressionTime)).Replace(",", "."));
                xmlElem.SetAttributeValue("Time", String.Format("{0:HH:mm:ss.ffff}", DateTime.Now));

                xDoc.Root.Element("Events").Add(xmlElem);
                xDoc.Save(logsPath);
            }
            catch
            {
                ErrorDialog errDialog = new ErrorDialog();
                errDialog.tbInstructionText.Text = "Some problems occurred writing the log file.";
                errDialog.Show();
            }
        }

        private void LogSubjectEvents(int answer, DateTime dt)
        {
            try
            {
                XDocument xDoc = XDocument.Load(logsPath);

                XElement xmlElem = new XElement("Action");
                xmlElem.SetAttributeValue("Type", "Answer");
                xmlElem.SetAttributeValue("Value", Enum.GetName(typeof(FACEAnswers), answer));
                xmlElem.SetAttributeValue("Index", answer);
                xmlElem.SetAttributeValue("Timestamp", String.Format("{0:0.000000}", FromDateToDouble(dt)).Replace(",", "."));
                xmlElem.SetAttributeValue("ResponseTime", String.Format("{0:0}", (dt.Subtract(lastExpressionTime)).ToString()));

                xDoc.Root.Element("Events").Add(xmlElem);
                xDoc.Save(logsPath);
            }
            catch
            {
                ErrorDialog errDialog = new ErrorDialog();
                errDialog.tbInstructionText.Text = "Some problems occurred writing the log file.";
                errDialog.Show();
            }
        }

        private void LogRestEvent(TimeSpan restSpan)
        {
            try
            {
                XDocument xDoc = XDocument.Load(logsPath);

                XElement xmlElem = new XElement("Action");
                xmlElem.SetAttributeValue("Type", "Rest");
                xmlElem.SetAttributeValue("RestTime", String.Format("{0:0}", restSpan.ToString()));

                xDoc.Root.Element("Events").Add(xmlElem);
                xDoc.Save(logsPath);
            }
            catch
            {
                ErrorDialog errDialog = new ErrorDialog();
                errDialog.tbInstructionText.Text = "Some problems occurred writing the log file.";
                errDialog.Show();
            }
        }

        private void ConvertTimestamp()
        {
            string[] filePaths = Directory.GetFiles(@"D:\Università\_Test espressioni\DATI\Conversione", "*.xml");
            foreach (string file in filePaths)
            {
                FileInfo fInfo = new FileInfo(file);
                XDocument xDoc = XDocument.Load(file);
                XElement root = xDoc.Root; //Test
                XElement events = root.Element(XName.Get("Events"));
                IEnumerable<XElement> elements = events.Elements(XName.Get("Action"));

                XDocument newDoc = new XDocument(new XDeclaration("1.0", System.Text.Encoding.UTF8.WebName, "yes"));

                XElement xRoot = new XElement(XName.Get("Test"));
                newDoc.Add(xRoot);
                XElement xmlElem = new XElement("PersonalInfo");
                newDoc.Root.Add(xmlElem);
                XElement newEvents = new XElement(XName.Get("Events"));
                newDoc.Root.Add(newEvents);

                DateTime parsedDate = DateTime.Now;
                foreach (XElement elem in elements)
                {
                    XElement el = new XElement("Action");

                    if ((string)elem.Attribute("Type") == "Rest")
                    {
                        el.SetAttributeValue("Type", (string)elem.Attribute("Type"));
                        el.SetAttributeValue("RestTime", (string)elem.Attribute("RestTime"));
                    }
                    else if ((string)elem.Attribute("Type") == "Expression")
                    {
                        el.SetAttributeValue("Type", (string)elem.Attribute("Type"));
                        el.SetAttributeValue("Value", (string)elem.Attribute("Value"));
                        el.SetAttributeValue("Index", (string)elem.Attribute("Index"));
                        string date = (string)root.Attribute("Date") + " " + (string)elem.Attribute("Time");
                        DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss.ffff", null, DateTimeStyles.None, out parsedDate);
                        double ts = FromDateToDouble(parsedDate);
                        el.SetAttributeValue("Timestamp", ts);
                        el.SetAttributeValue("Time", (string)elem.Attribute("Time"));

                        //string ts = (string)elem.Attribute("Timestamp");
                        //el.SetAttributeValue("Timestamp", ts);
                        //double parsed = Double.Parse(ts.Replace('.', ','));
                        //DateTime newdt = FromDoubleToDate(parsed);
                        //el.SetAttributeValue("Time", String.Format("{0:HH:mm:ss.ffff}", newdt));                        
                        //string tts = (1355324775.6170).ToString();
                        //double parsedt = Double.Parse(tts.Replace('.', ','));
                        //DateTime newdtt = FromDoubleToDate(parsedt);
                    }
                    else if ((string)elem.Attribute("Type") == "Answer")
                    {
                        if ((string)elem.Attribute("Value") != "-")
                        {
                            el.SetAttributeValue("Type", (string)elem.Attribute("Type"));
                            el.SetAttributeValue("Value", (string)elem.Attribute("Value"));
                            el.SetAttributeValue("Index", (string)elem.Attribute("Index"));
                            string stamp = (string)elem.Attribute("Timestamp");
                            el.SetAttributeValue("Timestamp", stamp.Substring(0, stamp.Length - 2));
                            string respTime = (string)elem.Attribute("ResponseTime");
                            TimeSpan response = TimeSpan.Parse(respTime);
                            double parsedt = Double.Parse(stamp.Replace('.', ','));
                            DateTime rTime = FromDoubleToDate(parsedt);
                            //DateTime rTime = parsedDate.Add(response);
                            //el.SetAttributeValue("Time", String.Format("{0:HH:mm:ss.ffff}", rTime));
                            el.SetAttributeValue("Time", String.Format("{0:HH:mm:ss.ffff}", rTime));
                            el.SetAttributeValue("ResponseTime", respTime);
                        }
                    }
                    newDoc.Root.Element("Events").Add(el);
                }
                newDoc.Save(@"D:\Università\_Test espressioni\DATI\Conversione\NewFiles\OK_" + fInfo.Name);
            }
        }

        #endregion


        #region Buttons

        private void RestTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextboxName.Text != "" && TextboxSurname.Text != "" && TextboxDay.Text != "" && TextboxMonth.Text != "" &&
                TextboxYear.Text != "" && (MRadioButton.IsChecked == true || FRadioButton.IsChecked == true))
            {
                PersonalDataPanel.IsEnabled = false;
                InitTestLog();
                StartClock();
            }
            else
            {
                MessageBox.Show("Uno o più campi non sono corretti!");
            }
        }

        private void GotoTestButton_Click(object sender, RoutedEventArgs e)
        {
            LogRestEvent(DateTime.Now.Subtract(startClockTime));
            clockTimer.Stop();
            GotoTestButton.IsEnabled = true;
            TestPanel.IsEnabled = true;
            LeftPanel.IsEnabled = false;
        }

        private void StartTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComPorts.OpenedPort)
            {
                //t = new System.Threading.Timer(new System.Threading.TimerCallback(TimerProc));                
                List<int> inputList = new List<int> { 1, 2, 3, 4, 5, 6 };
                outputList = ShuffleList(inputList);

                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
                dispatcherTimer.Start();
                //ComPorts.SelectedPort.WriteLine(String.Format("{0:HH.mm.ss}", DateTime.Now));
            }
            else
            {
                WarningDialog warningDialog = new WarningDialog();
                warningDialog.tbInstructionText.Text = "There are not opened ports!";
                warningDialog.Show();
            }
        }

        private void NextTestButton_Click(object sender, RoutedEventArgs e)
        {
            LogSubjectEvents(answer, answerTime);
            selected.IsChecked = false;

            if (countTick == 0)
            {
                countTick = 1;
                RaiseEvent(new RoutedEventArgs(TickEvent));
            }
        }

        private void StopTestButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            PersonalDataPanel.IsEnabled = true;
            TestPanel.IsEnabled = false;
        }

        #endregion


        #region Controls

        private void StartClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1.0);
            clockTimer.Start();
            TimeSpan end = new TimeSpan(0, 3, 1);
            startClockTime = DateTime.Now;
            clockTimer.Tick += new EventHandler(delegate(object s, EventArgs a)
            {
                timeDiff = DateTime.Now.Subtract(startClockTime);
                if (timeDiff.CompareTo(end) == -1)
                {
                    string sec = (timeDiff.Seconds < 10) ? "0" + timeDiff.Seconds.ToString() : timeDiff.Seconds.ToString();
                    ClockTextbox.Text = "0" + timeDiff.Hours + ":0" + timeDiff.Minutes + ":" + sec;
                }
                else
                {
                    GotoTestButton.IsEnabled = true;
                }
            });
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (countTick == 0)
            {
                switch (outputList[idExpression])
                {
                    case 1:
                        LoadAndSendExpressionForTest(testPath + "AU_Anger.xml", FACEActions.Anger);
                        break;
                    case 2:
                        LoadAndSendExpressionForTest(testPath + "AU_Disgust.xml", FACEActions.Disgust);
                        break;
                    case 3:
                        LoadAndSendExpressionForTest(testPath + "AU_Fear.xml", FACEActions.Fear);
                        break;
                    case 4:
                        LoadAndSendExpressionForTest(testPath + "AU_Happiness.xml", FACEActions.Happiness);
                        break;
                    case 5:
                        LoadAndSendExpressionForTest(testPath + "AU_Sadness.xml", FACEActions.Sadness);
                        break;
                    case 6:
                        LoadAndSendExpressionForTest(testPath + "AU_Surprise.xml", FACEActions.Surprise);
                        break;
                }
                //lastExpressionTime = DateTime.Now;
                idExpression++;
                countTick = 1;
                //ComPorts.SelectedPort.WriteLine(String.Format("----- {0:HH.mm.ss} espressione", DateTime.Now));
            }
            else if (countTick == 1)
            {
                LoadAndSendExpressionForTest(testPath + "AU_Neutral.xml", FACEActions.Neutral);
                countTick = 2;
                //ComPorts.SelectedPort.WriteLine(String.Format("----- {0:HH.mm.ss} Neutra", DateTime.Now));
            }
            else if (countTick == 2)
            {
                countTick = 0;
                if (idExpression == outputList.Count)
                {
                    dispatcherTimer.Stop();
                    NextTestButton.IsEnabled = false;
                }
            }

            // Forcing the CommandManager to raise the RequerySuggested event
            //CommandManager.InvalidateRequerySuggested();
        }

        private void LoadAndSendExpressionForTest(string fileName, FACEActions actionType)
        {
            try
            {
                //SBInfoBox.Text = "Testing " + fileName.ToString().Remove(fileName.Length - 4) + " expression..";
                RobotControl.ExecuteFile(fileName, 10);
                lastExpressionTime = DateTime.Now;
                LogFaceEvents(actionType);
            }
            catch (RobotException fEx)
            {
                TextError.Text = "Error occurs testing " + fileName.ToString().Remove(fileName.Length - 4) + " expression..";
                ErrorDialog errorDiag = new ErrorDialog();
                errorDiag.tbInstructionText.Text = fEx.Message;
                errorDiag.Show();
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (selected != null)
                selected.IsChecked = false;
            answerTime = DateTime.Now;
            selected = (RadioButton)sender;
            answer = Int32.Parse(selected.Uid);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (dispatcherTimer.IsEnabled)
                {
                    LoadAndSendExpressionForTest(testPath + "AU_Neutral.xml", FACEActions.Neutral);
                    //ComPorts.SelectedPort.WriteLine(String.Format("----- {0:HH.mm.ss} Barra spaziatrice", DateTime.Now));
                    //if (countTick == 0)
                    //{
                    //    LoadAndSendExpressionForTest(testPath + "AU_Neutral.xml", FACEActions.Neutral);
                    //    countTick = 1;
                    //}
                    //else if (countTick == 1)
                    //    countTick = 2;

                    //RaiseEvent(new RoutedEventArgs(TickEvent));                    
                }
                else
                {
                    WarningDialog warningDialog = new WarningDialog();
                    warningDialog.tbInstructionText.Text = "There are not opened ports!";
                    warningDialog.Show();
                }
            }
        }

        private List<int> ShuffleList(List<int> inputList)
        {
            List<int> randomList = new List<int>();
            System.Random r = new System.Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }
            return randomList; //return the new random list
        }

        #endregion

        #endregion

    
        #region Statusbar


        private void CheckboxAnimator_Checked(object sender, RoutedEventArgs e)
        {
            animation = new System.Threading.Thread(new ThreadStart(AnimatorEngine.StartAnimation));
            animation.Start();
        }

        private void CheckboxAnimator_Unchecked(object sender, RoutedEventArgs e)
        {
            if (AnimatorEngine.Running)
                AnimatorEngine.StopAnimation();
        }


        private void CheckboxYarpExp_Checked(object sender, RoutedEventArgs e)
        {
            yarpExpressionOn = true;
        }



        private void CheckboxYarpExp_Unchecked(object sender, RoutedEventArgs e)
        {
            yarpExpressionOn = false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SBTimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                expressionTime = Convert.ToInt32(SBTimeBox.Text, NumberFormatInfo.InvariantInfo);
                if (expressionTime < 0)
                {
                    expressionTime = 0;
                    SBTimeBox.Text = String.Format(expressionTime.ToString("", CultureInfo.InvariantCulture));
                }
            }
            catch
            {
                expressionTime = 0;
                SBTimeBox.Text = String.Format(expressionTime.ToString("0", CultureInfo.InvariantCulture));
            }
        }


        private void SBTimeBoxNeck_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                NeckTime = Convert.ToInt32(SBTimeNeckBox.Text, NumberFormatInfo.InvariantInfo);
                if (NeckTime < 0)
                {
                    NeckTime = 0;
                    SBTimeNeckBox.Text = String.Format(NeckTime.ToString("", CultureInfo.InvariantCulture));
                }
            }
            catch
            {
                NeckTime = 0;
                SBTimeNeckBox.Text = String.Format(NeckTime.ToString("0", CultureInfo.InvariantCulture));
            }
        }

        private void txtSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                speed = Convert.ToDouble(txtSpeed.Text, NumberFormatInfo.InvariantInfo);
                if (speed < 0)
                {
                    speed = 1;
                    txtSpeed.Text = String.Format(speed.ToString("", CultureInfo.InvariantCulture));
                }
            }
            catch
            {
                speed = 1;
                txtSpeed.Text = String.Format(speed.ToString("0", CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxTime"></param>
        private void StartProgressbarTime(double maxTime)
        {
            //Configure the ProgressBar
            //SBProgressBar.Minimum = 0;
            //SBProgressBar.Maximum = maxTime;
            //SBProgressBar.Value = 0;

            ////Stores the value of the ProgressBar
            //double timeValue = 0;

            //// Create a new instance of our ProgressBar Delegate that points
            //// to the ProgressBar's SetValue method.
            //updatePbTimeDelegate = new UpdateProgressBarTimeDelegate(SBProgressBar.SetValue);

            ////Tight Loop:  Loop until the ProgressBar.Value reaches the max
            //do
            //{
            //    timeValue += 1;

            //    /*Update the Value of the ProgressBar:
            //      1)  Pass the "updatePbDelegate" delegate that points to the ProgressBar1.SetValue method
            //      2)  Set the DispatcherPriority to "Background"
            //      3)  Pass an Object() Array containing the property to update (ProgressBar.ValueProperty) and the new value */
            //    Dispatcher.Invoke(updatePbTimeDelegate,
            //        System.Windows.Threading.DispatcherPriority.Background,
            //        new object[] { ProgressBar.ValueProperty, timeValue });
            //}
            //while (SBProgressBar.Value != SBProgressBar.Maximum);

            //SBProgressBar.Value = SBProgressBar.Minimum;
            SBInfoBox.Text = "";
            TextError.Text = "";
        }



        #endregion

        #region Logging


        private void LogEvents(FACEActions actionType)
        {
            try
            {
                XDocument xDoc = XDocument.Load(logsPath);

                XElement xmlElem = new XElement("Action");
                xmlElem.SetAttributeValue("Type", actionType);
                xmlElem.SetAttributeValue("Value", (int)actionType);
                xmlElem.SetAttributeValue("Delta", String.Format("{0:0}", (FromDateToDouble(DateTime.Now) - startTimeUI) * Math.Pow(10, 3)).Replace(",", "."));
                xmlElem.SetAttributeValue("Timestamp", String.Format("{0:0.000000}", FromDateToDouble(DateTime.Now)).Replace(",", "."));

                xDoc.Root.Element("Actions").Add(xmlElem);
                xDoc.Save(logsPath);
            }
            catch
            {
                ErrorDialog errDialog = new ErrorDialog();
                errDialog.tbInstructionText.Text = "Some problems occurred writing the log file.";
                errDialog.Show();
            }
        }

        private double FromDateToDouble(DateTime time)
        {
            DateTime dTime = time - TimeZone.CurrentTimeZone.GetUtcOffset(time);
            TimeSpan span = dTime - new DateTime(1970, 1, 1);
            return Math.Round((span.Ticks / 10) * Math.Pow(10, -6), 4);
        }

        private DateTime FromDoubleToDate(double time)
        {
            TimeSpan v = new TimeSpan((long)((time * 10) / Math.Pow(10, -6)));
            DateTime res = new DateTime(1970, 1, 1) + v; //ora legale di Greenwich
            TimeSpan currentOffset = TimeZone.CurrentTimeZone.GetUtcOffset(res);
            return res + currentOffset;
        }



        //// Log for Win7 into registry
        //private void InitLogOnRegistry()
        //{            
        //    //if (EventLog.SourceExists("FACETool"))
        //    //{
        //    //    //An event log source should not be created and immediately used.
        //    //    //There is a latency time to enable the source, it should be created
        //    //    //prior to executing the application that uses the source.
        //    //    //Execute this sample a second time to use the new source.
        //    //    //EventLog.CreateEventSource("FACETool", "FACETool_Log");
        //    //    EventLog.DeleteEventSource("FACETool");
        //    //    EventLog.Delete("FACETool_Log");
        //    //}

        //    // Create an EventLog instance and assign its source.
        //    //EventLog myLog = new EventLog();
        //    //myLog.Source = "FACETool";

        //    // Write an informational entry to the event log.    
        //    EventLog.WriteEntry("FACETool", "Writing to event log.");

        //    EventLog[] logs = EventLog.GetEventLogs();
        //    EventLog selected = null;
        //    foreach (EventLog l in logs)
        //    {
        //        if (l.Log == "Application")
        //            selected = l;
        //    }

        //    List<string> messages = new List<string>();
        //    foreach (EventLogEntry entry in selected.Entries)
        //    {
        //        if (entry.Source == "FACETool")
        //            messages.Add(entry.Message);
        //    }

        //    //// To delete an entry in the Event Viewer list
        //    //string logName = "";
        //    //if (EventLog.SourceExists("FACETool_Log"))
        //    //{
        //    //    // Find the log associated with this source.    
        //    //    logName = EventLog.LogNameFromSourceName("FACETool_Log", ".");
        //    //    // Delete the source and the log.
        //    //    //EventLog.DeleteEventSource("FACETool_Log");
        //    //    EventLog.Delete(logName);
        //    //}
        //}

        //private void LogEventsOnRegistry(FACEActions actionType)
        //{
        //    //EventLog.WriteEntry("FACETool", fileName.Substring(0, fileName.Length - 4));
        //}

        #endregion



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // OpenClosePort();

            if (AnimatorEngine.Running)
            {
                AnimatorEngine.StopAnimation();
            }



            //if (statusWin != null)
            //    statusWin.Close();




            Close();


        }

    }



}


#region Respiration

//private void RespirationRateBox_TextChanged(object sender, TextChangedEventArgs e)
//{
//    if (RespirationRateBox != null)
//    {
//        int rate = 1;
//        try
//        {
//            rate = Convert.ToInt32(RespirationRateBox.Text, NumberFormatInfo.InvariantInfo);
//            if (rate < 1)
//            {
//                rate = 1;
//                RespirationRateBox.Text = String.Format(rate.ToString("0", CultureInfo.InvariantCulture));
//            }
//            FACEBody.StopRespiration();
//            FACEBody.RespirationRate = rate;
//            FACEBody.StartRespiration();
//        }
//        catch
//        {
//            rate = 1;
//            RespirationRateBox.Text = String.Format(rate.ToString("0", CultureInfo.InvariantCulture));
//        }
//    }
//}


//private void RespirationSpeedBox_TextChanged(object sender, TextChangedEventArgs e)
//{
//    if (RespirationSpeedBox != null)
//    {
//        int speed = 1;
//        try
//        {
//            speed = Convert.ToInt32(RespirationSpeedBox.Text, NumberFormatInfo.InvariantInfo);
//            if (speed < 1)
//            {
//                speed = 1;
//                RespirationSpeedBox.Text = String.Format(speed.ToString("00", CultureInfo.InvariantCulture));
//            }
//            FACEBody.StopRespiration();
//            FACEBody.RespirationSpeed = speed;
//            FACEBody.StartRespiration();
//            //Brain.ClosedEyesTime = closedTime;
//        }
//        catch
//        {
//            speed = 1;
//            RespirationSpeedBox.Text = String.Format(speed.ToString("0", CultureInfo.InvariantCulture));
//        }
//    }
//}

//private void CheckboxAutomaticRespiration_Checked(object sender, RoutedEventArgs e)
//{
//    FACEBody.StartRespiration();
//    //LogEvents(FACEActions.StartRespiration);

//    AutomaticRespirationParams.IsEnabled = true;
//    RespirationRateBox.IsEnabled = true;
//    RespirationSpeedBox.IsEnabled = true;
//}

//private void CheckboxAutomaticRespiration_Unchecked(object sender, RoutedEventArgs e)
//{
//    FACEBody.StopRespiration();

//    AutomaticRespirationParams.IsEnabled = false;
//    RespirationRateBox.IsEnabled = false;
//    RespirationSpeedBox.IsEnabled = false;
//}

#endregion
