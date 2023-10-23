using Act.Lib;
using Act.Lib.Control;
using Act.Lib.Robot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YarpManagerCS;

using Sense.Lib.FACELibrary;

namespace Act.Face.FACEGui20.View
{
    public partial class NetView : UserControl
    {
        FACEGui20Win parent;

        private string path_yarp = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\AppData\Roaming") + "\\yarp\\conf\\yarp.conf";
        private System.Timers.Timer TimerCheckStatusYarp;

        private string colYarp;// Ellipse color Yarp
        private string colMot;// Ellipse color Yarp Set Motor

        private YarpPort yarpPortSetMotorsEyes;

        private YarpPort yarpPortSetMotors;
        private YarpPort yarpPortFeedback;
        private YarpPort yarpPortButtons;
        private YarpPort yarpPortTrigger;

        private string setmotorseyes_in = "/FACEGui/yarpPortLookAtEyes:i";
        string setmotorseyes_out = "/AttentionModule/LookAtEyes:o";

        //private string setmotorseyes_out = "/RobotControl/Eyes:o";         verificare qui!!!

        private string setmotors_in = ConfigurationManager.AppSettings["YarpPortSetMotors_IN"].ToString();
        private string setmotors_out = ConfigurationManager.AppSettings["YarpPortSetMotors_OUT"].ToString();
        private string feedback_out = ConfigurationManager.AppSettings["YarpPortfeedback_OUT"].ToString();

        private string buttonGamepad_in = "/FACEGui/Buttons:i";
        private string buttonGamepad_out = "/GamepadController/Buttons:o";

        private string triggerGamepad_in = "/FACEGui/Trigger:i";
        private string triggerGamepad_out = "/GamepadController/Trigger:o";

        

        string receivedSetMotors = "";
        string receivedButtons = "";
        string receivedTrigger = "";
        string receivedDataEyes = "";

        private System.Threading.Thread senderThreadFeedBack = null;

        private bool closing = false;
        private bool yarpStatus = false;

        private Winner SubjectWin = new Winner();


        public NetView()
        {
            InitializeComponent();         
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            parent = (FACEGui20Win) Window.GetWindow(this);

            if (parent == null)
                throw new Exception("NetView UserControls should be inserted in Window before being loaded");

            parent.Closing += window_Closing;

            //roba per yarp
            ServerIPTextbox.Text = ServerYarpConf();
            MyIPLabel.Content = LocalIP();

            ThreadPool.QueueUserWorkItem(InitYarp);

            ThreadPool.QueueUserWorkItem(ReceiveDataSetMotorsEyes);

            ThreadPool.QueueUserWorkItem(ReceiveDataSetMotors);
            ThreadPool.QueueUserWorkItem(ReceiveDataButtons);
            ThreadPool.QueueUserWorkItem(ReceiveDataTrigger);

        }


        /// <summary>
        /// Initialize Timer for Test Yarp
        /// </summary>
        /// <param name="panel"></param>
        private void InitYarp(object s)
        {


            yarpPortSetMotorsEyes = new YarpPort();
            yarpPortSetMotorsEyes.openReceiver(setmotorseyes_out, setmotorseyes_in);


            yarpPortSetMotors = new YarpPort();
            yarpPortSetMotors.openReceiver(setmotors_out, setmotors_in);

            yarpPortButtons = new YarpPort();
            yarpPortButtons.openReceiver(buttonGamepad_out, buttonGamepad_in);
            
            yarpPortTrigger = new YarpPort();
            yarpPortTrigger.openReceiver(triggerGamepad_out, triggerGamepad_in);

            yarpPortFeedback = new YarpPort();
            yarpPortFeedback.openSender(feedback_out);

            // controllo se la connessione con le porte sono attive(unico metodo funzionante)
            colYarp = "red";
            colMot = "red";

            TimerCheckStatusYarp = new System.Timers.Timer();
            TimerCheckStatusYarp.Elapsed += new ElapsedEventHandler(CheckStatusYarp);
            TimerCheckStatusYarp.Interval = (1000) * (30);
            TimerCheckStatusYarp.Enabled = true;
            TimerCheckStatusYarp.Start();

            senderThreadFeedBack = new System.Threading.Thread(SendFeedBack);
            senderThreadFeedBack.Start();


            yarpStatus = true;


        }

        #region YARP



        void ReceiveDataSetMotorsEyes(object sender)
        {
            while (!yarpStatus)
                System.Threading.Thread.Sleep(200);

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();

            while (!closing)
            {
                if (!parent.yarpExpressionOn)
                    System.Threading.Thread.Sleep(200);

                yarpPortSetMotorsEyes.receivedData(out receivedDataEyes);
                //yarpPortLookAtEyes.receivedData(out receivedLookAt);
                if (receivedDataEyes != null && receivedDataEyes != "" && parent.yarpExpressionOn)
                {
                    //Debug.WriteLine(receivedSetMotors);
                    try
                    {
                        SubjectWin = ComUtils.XmlUtils.Deserialize<Winner>(receivedDataEyes);
                        float x = (float)SubjectWin.spinX;
                        float y = (float)SubjectWin.spinY;

                        float z = (float)SubjectWin.spinZ;

                        RobotMotion motionToTest = new RobotMotion(RobotControl.CurrentMotorState.Count);

                        int lookAtDuration = 120; //the lookat function works every 110msec so it can't act longer requests
                        float minAmplitudeX = 0.002f, minAmplitudeY = 0.002f, maxAmplitudeX = 1.0f, maxAmplitudeY = 1.0f;

                        float old_PosX = RobotControl.CurrentMotorState[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized;
                        float old_PosY = RobotControl.CurrentMotorState[(int)AbelRobotControl.MotorsNames.EyelidTopRight].PulseWidthNormalized;

                        //float old_PosZ = RobotControl.CurrentMotorState[(int)AbelRobotControl.MotorsNames.Jaw].PulseWidthNormalized;

                        



                        motionToTest.ServoMotorsList.Where(w => w.PulseWidthNormalized != -1).ToList().ForEach(s => s.PulseWidthNormalized = -1);
                        motionToTest.Duration = lookAtDuration;
                        motionToTest.Priority = 15;

                        //if (z < 1.0f)
                        //    motionToTest.ServoMotorsList.Find(a => a.Name == "MiddleBrowRight").PulseWidthNormalized = 0.0f;
                        //    //motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.SideBrowRight].PulseWidthNormalized = 0.0f;
                        
                        //else
                        //    motionToTest.ServoMotorsList.Find(a => a.Name == "MiddleBrowRight").PulseWidthNormalized = 1.0f;
                        //    //motionToTest.ServoMotorsList[(int)AbelRobotControl.MotorsNames.SideBrowRight].PulseWidthNormalized = 1.0f;
                        


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

                                    //motionToTest.ServoMotorsList.Find(a => a.Name == "EyeIOLeft").PulseWidthNormalized = x;
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
                                if (Math.Abs(y - old_PosY) < maxAmplitudeY)
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




                        parent.SetNewPositionMotors(motionToTest.ServoMotorsList, TimeSpan.FromMilliseconds(parent.expressionTime));


                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine("Error XML Set motors: " + exc.Message);
                    }



                }
            }
        }

        void ReceiveDataSetMotors(object sender)
        {

            while (!yarpStatus)
                System.Threading.Thread.Sleep(200);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (!closing)
            {
                if(!parent.yarpExpressionOn)
                    System.Threading.Thread.Sleep(200);

                yarpPortSetMotors.receivedData(out receivedSetMotors);
                if (receivedSetMotors != null && receivedSetMotors != "" && parent.yarpExpressionOn)
                {
                    //Debug.WriteLine(receivedSetMotors);
                    try
                    {
                        List<ServoMotor> newListMotorsPosistion = new List<ServoMotor>();

                        string xml = receivedSetMotors;
                        xml = xml.Replace(@"\", "");
                        xml = xml.Substring(1, xml.Length - 2);

                        if (xml.Substring(0, 1) == "?")
                            xml = xml.Remove(0, 1);

                        //Debug.WriteLine(xml.Substring(0, 5));
                        if (xml.Substring(0, 5) == "<?xml")
                        {
                            try
                            {
                                newListMotorsPosistion = ComUtils.XmlUtils.Deserialize<List<ServoMotor>>(receivedSetMotors);


                                //check exists new positions
                                if (newListMotorsPosistion.FindAll(a => ((a.PulseWidthNormalized <= 0 && a.PulseWidthNormalized >= 1) && (a.PulseWidthNormalized != -1.0))).Count > 0)
                                    continue;


                                parent.TextTIME.Dispatcher.Invoke(
                                           System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() => parent.TextTIME.Text = string.Format("{0} ms", stopWatch.Elapsed.Milliseconds))
                                 );

                                parent.SetNewPositionMotors(newListMotorsPosistion, TimeSpan.FromMilliseconds(parent.expressionTime));

                               
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            stopWatch.Restart();

                        }
                        else
                        {
                            switch (receivedSetMotors)
                            {
                                case "none":
                                    break;
                                case "Yes":
                                    RobotControl.StartYesMovement();
                                    break;
                                case "No":
                                    RobotControl.StartNoMovement();
                                    break;
                                case "OpenEyes":
                                    parent.OpenEyes();
                                    break;
                                case "CloseEyes":
                                    parent.CloseEyes();
                                    break;
                                default:
                                    MessageBox.Show("Moviement Unknown");
                                    break;
                            }
                        }






                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine("Error XML Set motors: " + exc.Message);
                    }


                }
            }
        }


        void ReceiveDataButtons(object sender)
        {
            while (!yarpStatus)
                System.Threading.Thread.Sleep(200);

            while (!closing)
            {

                yarpPortButtons.receivedData(out receivedButtons);
                if (receivedButtons != null && receivedButtons != "")
                {
                    try
                    {
                        receivedButtons = receivedButtons.Replace(@"\", "").Replace("\"", "");

                        string exp = "";
                        switch (receivedButtons)
                        {
                            case "A":
                                exp = parent.configGamepad.A;
                                break;
                            case "B":
                                exp = parent.configGamepad.B;
                                break;
                            case "X":
                                exp = parent.configGamepad.X;
                                break;
                            case "Y":
                                exp = parent.configGamepad.Y;
                                break;
                            case "DPadDown":
                                exp = parent.configGamepad.DPadDown;
                                break;
                            case "DPadUp":
                                exp = parent.configGamepad.DPadUp;
                                break;
                            case "DPadLeft":
                                exp = parent.configGamepad.DPadLeft;
                                break;
                            case "DPadRight":
                                exp = parent.configGamepad.DPadRight;
                                break;
                            case "LeftThumb":
                                exp = parent.configGamepad.LeftThumb;
                                break;
                            case "RightThumb":
                                exp = parent.configGamepad.RightThumb;
                                break;



                        }

                        if (exp != "")
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                            {
                                RobotMotion motion = parent.ecsMotions.Find(x => x.Name == "AU_" + exp);
                                parent.SetNewPositionMotors(motion.ServoMotorsList, TimeSpan.FromMilliseconds(parent.expressionTime));
                            }));


                        if (receivedButtons == "Start")
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                            {
                                parent.CheckboxAnimator.IsChecked = true;
                            }));
                        else if (receivedButtons == "Back")
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                            {
                                parent.CheckboxAnimator.IsChecked = false;

                            }));


                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine("Error XML Set motors: " + exc.Message);
                    }


                }
            }
        }



        void ReceiveDataTrigger(object sender)
        {
            while (!yarpStatus)
                System.Threading.Thread.Sleep(200);

            while (!closing)
            {

                yarpPortTrigger.receivedData(out receivedTrigger);
                if (receivedTrigger != null && receivedTrigger != "")
                {
                    //Debug.WriteLine(receivedSetMotors);
                    try
                    {
                        receivedTrigger = receivedTrigger.Replace(@"\", "").Replace("\"", "").Replace("[", "").Replace("]", "");

                        string k = receivedTrigger.Split(',')[0];
                        string v = receivedTrigger.Split(',')[1];



                        double f = Convert.ToDouble(v) / 255.0;

                        int m = 0;
                        if (k == "left")
                            m = parent.configGamepad.LeftTrigger;
                        else if (k == "right")
                            m = parent.configGamepad.RightTrigger;

                        if (m == 55)
                            return;

                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                new Action(delegate ()
                                {
                                    RobotMotion motionToTest = new RobotMotion(RobotControl.CurrentMotorState.Count);

                                    motionToTest.ServoMotorsList[m].PulseWidthNormalized = (float)f;

                                    motionToTest.Duration = parent.expressionTime;
                                    motionToTest.Priority = 10;

                                    parent.SetNewPositionMotors(motionToTest.ServoMotorsList, TimeSpan.FromMilliseconds(parent.expressionTime));

                                }));




                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine("Error XML Set motors: " + exc.Message);
                    }


                }


            }
        }

        private void SendFeedBack()
        {
            while (!yarpStatus)
                System.Threading.Thread.Sleep(200);

            while (!closing)
            {

                yarpPortFeedback.sendData(ComUtils.XmlUtils.Serialize<List<ServoMotor>>(RobotControl.CurrentMotorState));
                //Debug.WriteLine(" Turn " + FACEBody.CurrentMotorState[(int)MotorsNames.Turn].PulseWidth);


            }
        }

        private void YarpDisconnect()
        {

            if (TimerCheckStatusYarp != null)
                TimerCheckStatusYarp.Stop();

            if (yarpPortSetMotors.PortExists(setmotors_out))
                yarpPortSetMotors.Disconect(setmotors_in, setmotors_out);

            if (yarpPortSetMotorsEyes.PortExists(setmotorseyes_out))
                yarpPortSetMotorsEyes.Disconect(setmotorseyes_in, setmotorseyes_out);

            if (yarpPortButtons.PortExists(buttonGamepad_out))
                yarpPortSetMotors.Disconect(buttonGamepad_in, buttonGamepad_out);

            if (yarpPortTrigger.PortExists(triggerGamepad_out))
                yarpPortTrigger.Disconect(triggerGamepad_in, triggerGamepad_out);

            if (yarpPortFeedback != null)
                yarpPortFeedback.Close();

            if (yarpPortButtons != null)
                yarpPortButtons.Close();

            if (yarpPortTrigger != null)
                yarpPortTrigger.Close();

            if (senderThreadFeedBack != null)
                senderThreadFeedBack.Abort();

        }

      

        private void CheckStatusYarp(object source, ElapsedEventArgs e)
        {


            if (yarpPortSetMotors.PortExists(setmotors_out) && colMot == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { Ellexp.Fill = Brushes.Green; parent.BarEllAttention.Fill = Brushes.Green; }));
                colMot = "green";
            }
            else if (!yarpPortSetMotors.PortExists(setmotors_out) && colMot == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { Ellexp.Fill = Brushes.Red; parent.BarEllAttention.Fill = Brushes.Red; }));
                colMot = "red";
            }

            if (yarpPortSetMotors.NetworkExists() && colYarp == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { Ellyarp.Fill = Brushes.Green; parent.BarEllyarp.Fill = Brushes.Green; }));
                colYarp = "green";
            }
            else if (!yarpPortSetMotors.NetworkExists() && colYarp == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { Ellyarp.Fill = Brushes.Red; parent.BarEllyarp.Fill = Brushes.Red; }));
                colYarp = "red";
            }
        }

        /// <summary>
        /// Read server IP address from yarp.conf file
        /// </summary>
        /// <returns>The current server IP address</returns>
        private string ServerYarpConf()
        {
            string line = "";
            try
            {
                StreamReader sr = new StreamReader(path_yarp);
                line = sr.ReadLine();
                sr.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }

            return line.Split(' ')[0];
        }

        /// <summary>
        /// Write the server IP address in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveYarpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path_yarp);
                sw.WriteLine(ServerIPTextbox.Text + " 10000" + " yarp");
                sw.Close();
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception: " + exc.Message);
            }

        }

        /// <summary>
        /// Read the local IP address
        /// </summary>
        /// <returns>The local IP address of this machine</returns>
        private string LocalIP()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.WriteLine(addr);
                    return addr.ToString();
                }
            }
            return "";
        }

        #endregion

        #region UDP
        private int count = 0;

        private void ListenUDPButton_Click(object sender, RoutedEventArgs e)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.125"), 5566);
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            ManualResetEvent wait = new ManualResetEvent(false);

            UdpClient listener = new UdpClient(localEndPoint);

            System.Threading.Thread receiver = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    while (listener.Available != 0)
                    {
                        //wait.Set();
                        byte[] received = listener.Receive(ref remoteIpEndPoint);
                        // int code = Int32.Parse(Encoding.ASCII.GetString(received));

                        System.Diagnostics.Debug.WriteLine("[" + count + "]" + Encoding.ASCII.GetString(received));
                        count++;

                        //    float neckIncr = 0.10f;
                        //    int neckTime = 300; // to be tested
                        //    float turnNeckUDP = 0, upperNodUDP = 0;

                        //    switch (code)
                        //    {
                        //        case 1:
                        //            LoadAndSendExpression(expressionsPath + "AU_Neutral.xml", FACEActions.Neutral);
                        //            break;
                        //        case 2:
                        //            LoadAndSendExpression(expressionsPath + "AU_Anger.xml", FACEActions.Neutral);
                        //            break;
                        //        case 3:
                        //            LoadAndSendExpression(expressionsPath + "AU_Disgust.xml", FACEActions.Neutral);
                        //            break;
                        //        case 4:
                        //            LoadAndSendExpression(expressionsPath + "AU_Fear.xml", FACEActions.Neutral);
                        //            break;
                        //        case 5:
                        //            LoadAndSendExpression(expressionsPath + "AU_Happiness.xml", FACEActions.Neutral);
                        //            break;
                        //        case 6:
                        //            LoadAndSendExpression(expressionsPath + "AU_Sadness.xml", FACEActions.Neutral);
                        //            break;
                        //        case 7:
                        //            LoadAndSendExpression(expressionsPath + "AU_Surprise.xml", FACEActions.Neutral);
                        //            break;
                        //        case 8:
                        //            turnNeckUDP = FACEBody.CurrentMotorState[(int)MotorsNames.Turn].PulseWidth;
                        //            if (turnNeckUDP >= neckIncr)
                        //                turnNeckUDP -= neckIncr;
                        //            FACEBody.ExecuteSingleMovement((int)FaceTrackingControls.Turn, turnNeckUDP, TimeSpan.FromMilliseconds(neckTime), 10, TimeSpan.FromMilliseconds(0));
                        //            break;
                        //        case 9:
                        //            turnNeckUDP = FACEBody.CurrentMotorState[(int)MotorsNames.Turn].PulseWidth;
                        //            if (turnNeckUDP <= 1 - neckIncr)
                        //                turnNeckUDP += neckIncr;
                        //            FACEBody.ExecuteSingleMovement((int)FaceTrackingControls.Turn, turnNeckUDP, TimeSpan.FromMilliseconds(neckTime), 10, TimeSpan.FromMilliseconds(0));
                        //            break;
                        //        case 10:
                        //            upperNodUDP = FACEBody.CurrentMotorState[(int)MotorsNames.UpperNod].PulseWidth;
                        //            if (upperNodUDP >= neckIncr)
                        //                upperNodUDP -= neckIncr;
                        //            FACEBody.ExecuteSingleMovement((int)FaceTrackingControls.UpperNod, upperNodUDP, TimeSpan.FromMilliseconds(neckTime), 10, TimeSpan.FromMilliseconds(0));
                        //            break;
                        //        case 11:
                        //            upperNodUDP = FACEBody.CurrentMotorState[(int)MotorsNames.UpperNod].PulseWidth;
                        //            if (upperNodUDP <= 1 - neckIncr)
                        //                upperNodUDP += neckIncr;
                        //            FACEBody.ExecuteSingleMovement((int)FaceTrackingControls.UpperNod, upperNodUDP, TimeSpan.FromMilliseconds(neckTime), 10, TimeSpan.FromMilliseconds(0));
                        //            break;
                        //    }
                    }

                    //wait.WaitOne();

                }
            });
            receiver.Start();
        }


        #endregion



        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            closing = true;

            YarpDisconnect();
           
        }










    }
}