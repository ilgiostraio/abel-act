using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;

using System.Globalization;

using System.Configuration;
using System.Threading;
using System.Timers;

using Act.Lib;
using Act.Lib.Animator;
using Act.Lib.Control;
using Act.Lib.FaceControl;
using Act.Lib.Robot;
using Act.Lib.ServoController;

using Sense.Lib.FACELibrary;
using YarpManagerCS;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Act.Control.HUBRobotControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private YarpPort yarpPortLookAt;
        private YarpPort yarpPortSetECS;
        private YarpPort yarpPortSetFacialExpression;
        private YarpPort yarpPortReflexes;
        private YarpPort yarpPortSpeech;

        private YarpPort yarpPortSetFacialMotors;

        private YarpPort yarpPortFeedbackXML;
        private YarpPort yarpPortFeedbackJSON;
        private YarpPort yarpPortFeedbackBOTTLE;
        private YarpPort yarpPortFeedback;

        CancellationTokenSource ctsYarp;

        private string colYarp;// Ellipse color Yarp
        private string colLookAt;// Ellipse color Yarp Attention Module
        private string colECS;// Ellipse color Yarp Expression
        private string colExp;
        private string colRef;
        private string colSpe;
        private string colMot;

        private string lookAt_out = ConfigurationManager.AppSettings["YarpPortLookAt_OUT"].ToString();
        private string lookAt_in = ConfigurationManager.AppSettings["YarpPortLookAt_IN"].ToString();

        private string ecs_in = ConfigurationManager.AppSettings["YarpPortECS_IN"].ToString();
        private string ecs_out = ConfigurationManager.AppSettings["YarpPortECS_OUT"].ToString();

        //private string eyes_in = ConfigurationManager.AppSettings["YarpPortEyes_IN"].ToString();
        //private string eyes_out = ConfigurationManager.AppSettings["YarpPortEyes_OUT"].ToString();

        private string facialexpression_in = ConfigurationManager.AppSettings["YarpPortFacialExpression_IN"].ToString();
        private string facialexpression_out = ConfigurationManager.AppSettings["YarpPortFacialExpression_OUT"].ToString();

        private string reflexes_in = ConfigurationManager.AppSettings["YarpPortReflexes_IN"].ToString();
        private string reflexes_out = ConfigurationManager.AppSettings["YarpPortReflexes_OUT"].ToString();

        private string speech_in = ConfigurationManager.AppSettings["YarpPortSpeech_IN"].ToString();
        private string speech_out = ConfigurationManager.AppSettings["YarpPortSpeech_OUT"].ToString();

        /*--------------------------------------------------------------------------------------------------------------------*/
        private string feedbackXML_out = ConfigurationManager.AppSettings["YarpPortFeedbackXML_OUT"].ToString();
        private string feedbackJSON_out = ConfigurationManager.AppSettings["YarpPortFeedbackJSON_OUT"].ToString();
        private string feedbackBOTTLE_out = ConfigurationManager.AppSettings["YarpPortFeedbackBOTTLE_OUT"].ToString();
        private string feedback_in = ConfigurationManager.AppSettings["YarpPortFeedback_IN"].ToString();
        private string feedback_out = ConfigurationManager.AppSettings["YarpPortFeedback_OUT"].ToString();
        /*--------------------------------------------------------------------------------------------------------------------*/


        private string setFacialMotors_out = ConfigurationManager.AppSettings["YarpPortSetFacialMotors_OUT"].ToString();
        private string setFacialMotors_in = ConfigurationManager.AppSettings["YarpPortSetFacialMotors_IN"].ToString();

        string receivedLookAtData = "";
        string receivedSpeechData = "";

        string receivedECSData = "";
        string receivedFacialExpression = "";
        string receivedReflexes = "";
        string receivedFeedback = "";

        private System.Timers.Timer TimerCheckStatusYarp;

        //private List<ServoMotor> currentSmState;
        private ECS ecs;

        RobotMotion motionLookAt;
        RobotMotion motionECS;
        FaceExpression exp;
        List<ServoMotor> listMotorsFacialExpression;
        List<ServoMotor> listMotorsFeedback;
        List<float> ListFeedbackOnlyPositions;

        float old_PosX = 0;
        float old_PosY = 0;

        public enum reflexes { None, Yes, No, OpenEyes, CloseEyes };

        private YarpMonitor monitor = null;
        private Winner SubjectWin = new Winner();

        private TypeRobot typeRobot = TypeRobot.FACE;

        public MainWindow()
        {
            var dllDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/lib";
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);

            InitializeComponent();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            loadRobotConfig();
            InitYarp();

        }

      

        private void loadRobotConfig()
        {
            if (typeRobot == TypeRobot.FACE)
            {
                FACERobotControl face = new FACERobotControl(); //serve a precompilare il robot controll
                ecs = ECS.LoadFromXmlFormat("ECS_FACE.xml");
                lblTypeRobot.Text = "FACE";
            }
            else
            {
                AbelRobotControl abel = new AbelRobotControl();
                ecs = ECS.LoadFromXmlFormat("ECS_Abel.xml");
                Console.WriteLine(ecs);
                lblTypeRobot.Text = "Abel";
            }
            
            foreach (ECSMotor ecsM in ecs.ECSMotorList)
            {
                ecsM.FillMap();
            }


            listMotorsFeedback = new List<ServoMotor>();
            listMotorsFacialExpression = new List<ServoMotor>();
           
            ListFeedbackOnlyPositions = new List<float>();
        }

        private void InitYarp()
        {
            //if (typeRobot == TypeRobot.Abel)
             if (typeRobot == TypeRobot.FACE)
            {
                yarpPortLookAt = new YarpPort();
                yarpPortLookAt.openReceiver(lookAt_out, lookAt_in);
                //yarpPortLookAt.openReceiver(eyes_out, eyes_in);
            }

            yarpPortSetECS = new YarpPort();
            yarpPortSetECS.openReceiver(ecs_out, ecs_in);

            yarpPortSetFacialExpression = new YarpPort();
            yarpPortSetFacialExpression.openReceiver(facialexpression_out, facialexpression_in);

            yarpPortReflexes = new YarpPort();
            yarpPortReflexes.openReceiver(reflexes_out, reflexes_in);

            yarpPortSpeech = new YarpPort();
            yarpPortSpeech.openReceiver(speech_out, speech_in);

            yarpPortFeedbackXML = new YarpPort();
            yarpPortFeedbackXML.openSender(feedbackXML_out);

            yarpPortFeedbackJSON = new YarpPort();
            yarpPortFeedbackJSON.openSender(feedbackJSON_out);

            yarpPortFeedbackBOTTLE = new YarpPort();
            yarpPortFeedbackBOTTLE.openSender(feedbackBOTTLE_out);

            yarpPortFeedback = new YarpPort();
            yarpPortFeedback.openReceiver(feedback_out, feedback_in);

            yarpPortSetFacialMotors = new YarpPort();
            yarpPortSetFacialMotors.openSender(setFacialMotors_out);


            // controllo se la connessione con le porte sono attive(unico metodo funzionante)
            colYarp = "red";

            colLookAt = "red";
            colECS = "red";
            colExp = "red";
            colRef = "red";
            colMot = "red";
            colSpe = "red";



            lblLookAt.Content=lookAt_out;
            lblExp.Content=ecs_out;
            lblSetFace.Content = facialexpression_out;
            lblRef.Content = reflexes_out;

            lblFeedXML.Content = feedbackXML_out;
            lblFeedJson.Content = feedbackJSON_out;
            lblFeedBottle.Content = feedbackBOTTLE_out;
            lblMot.Content = setFacialMotors_out;
            lblSpeech.Content = speech_out;

            TimerCheckStatusYarp = new System.Timers.Timer();
            TimerCheckStatusYarp.Elapsed += new ElapsedEventHandler(CheckStatusYarp);
            TimerCheckStatusYarp.Interval = (1000) * (5);
            TimerCheckStatusYarp.Enabled = true;
            TimerCheckStatusYarp.Start();

            CheckStatusYarp(null , null);

            ctsYarp = new CancellationTokenSource();

            ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveDataFeedback), ctsYarp.Token);

            //ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveDataEyes), ctsYarp.Token);

            ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveDataECS), ctsYarp.Token);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveDataFacialExpression), ctsYarp.Token);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiverDataSpeech), ctsYarp.Token);


            //if (typeRobot == TypeRobot.Abel)
            if (typeRobot == TypeRobot.FACE)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiverDataLookAt), ctsYarp.Token);
            }    
            
      



        }

        void ReceiverDataLookAt(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                yarpPortLookAt.receivedData(out receivedLookAtData);

                if (receivedLookAtData != null && receivedLookAtData != "")
                {

                    try
                    {
                        SubjectWin = ComUtils.XmlUtils.Deserialize<Winner>(receivedLookAtData);
                        LookAt(SubjectWin.spinX, SubjectWin.spinY);
                        //LookAtMe(SubjectWin.spinX, SubjectWin.spinY);

                        receivedLookAtData = null;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Error XML Winner: " + exc.Message);
                    }

                }
            }
        }

        void ReceiveDataECS(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                yarpPortSetECS.receivedData(out receivedECSData);
                if (receivedECSData != null && receivedECSData != "")
                {

                    try
                    {
                        int animationDurationMs = -1;
                        Debug.WriteLine("[ROBOT_CONTROL] Ricevuto XML:"+receivedECSData);
                        string xmlnonxml = receivedECSData.Substring(1, receivedECSData.Length - 2);
                        if (xmlnonxml.EndsWith("@"))
                        {
                            Debug.WriteLine("[ROBOT_CONTROL] XML finisce con @");
                            int lastHyphenIndex = xmlnonxml.Length - 1;
                            int secondLastHyphenIndex = xmlnonxml.LastIndexOf('@', lastHyphenIndex - 1);


                            if (secondLastHyphenIndex != -1)
                            {
                                string timeString = xmlnonxml.Substring(secondLastHyphenIndex + 1, lastHyphenIndex - (secondLastHyphenIndex + 1));
                                if (int.TryParse(timeString, out int parsedTime))
                                {
                                    animationDurationMs = parsedTime;
                                    xmlnonxml = xmlnonxml.Substring(0, secondLastHyphenIndex);
                                    Debug.WriteLine($"Parsed custom time: {animationDurationMs}ms.");
                                }
                            }
                        }

                        receivedECSData = '"' + xmlnonxml + '"';
                        Debug.WriteLine("[ROBOT_CONTROL] Deserializzo XML:" + receivedECSData);
                        exp = ComUtils.XmlUtils.Deserialize<FaceExpression>(receivedECSData);  // bombardino
                        SetFacialExpression(exp.valence, exp.arousal, animationDurationMs);

                        receivedECSData = null;
                        exp = null;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Error XML Set expression: " + exc.Message);
                    }

                }
            }
        }

        void ReceiveDataFacialExpression(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                yarpPortSetFacialExpression.receivedData(out receivedFacialExpression);
                if (receivedFacialExpression != null && receivedFacialExpression != "")//&& yarpExpressionOn)
                {
                    try
                    {

                       
                        listMotorsFacialExpression = ComUtils.XmlUtils.Deserialize<List<ServoMotor>>(receivedFacialExpression);

           
                        if (listMotorsFacialExpression.FindAll(a => (a.PulseWidthNormalized >= 0 && a.PulseWidthNormalized <= 1) || a.PulseWidthNormalized == -1.0f).Count != RobotControl.NumberOfMotors)
                        {
                            MessageBox.Show("Error PulseWidthNormalized ");//of ServoMotor " + serv.Name + " PulseWidthNormalized:" + serv.PulseWidthNormalized);
                            return;
                        }
                        else if (listMotorsFacialExpression.FindAll(a => (a.PulseWidthNormalized <= 0.25 || a.PulseWidthNormalized >= 0.75) || a.Name == "Jaw").Count == 1)
                        {
                            MessageBox.Show("Error PulseWidthNormalized of ServoMotor Jaw ");// + serv.Name + " PulseWidthNormalized:" + serv.PulseWidthNormalized);
                            return;
                        }
                        //Bottle
                        //listMotors.AddRange(currentSmState);
                        //string[] res= receivedSetMotors.Split(' ');

                        //for (int i = 0; i <= res.Length; i++)
                        //{
                        //    if ((float.Parse(res[i]) >= 0 && float.Parse(res[i]) <= 1) || float.Parse(res[i]) == -1.0)
                        //        listMotors[i].PulseWidthNormalized = float.Parse(res[i]);
                        //    else
                        //    {
                        //        return;
                        //    }
                        //}

                        yarpPortSetFacialMotors.sendData(ComUtils.XmlUtils.Serialize<List<ServoMotor>>(listMotorsFacialExpression));

                        receivedFacialExpression = null;

                        listMotorsFacialExpression.Clear();

                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Error XML Set motors: " + exc.Message);
                    }

                }
            }
        }

        void ReceiveDataReflexes(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                yarpPortReflexes.receivedData(out receivedReflexes);
                if (receivedReflexes != null && receivedReflexes != "")//&& yarpExpressionOn)
                {
                    try
                    {
                        if (Enum.IsDefined(typeof(reflexes), receivedReflexes))
                            yarpPortSetFacialMotors.sendData(receivedReflexes);

                        receivedReflexes = null;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Error Reflexes: " + exc.Message);
                    }

                }
            }
        }

        void ReceiverDataSpeech(object obj)
        {

            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                yarpPortSpeech.receivedData(out receivedSpeechData);
                if (receivedSpeechData != null && receivedSpeechData != "")
                {
                    try
                    {


                        List<ServoMotor> listMotors = RobotControl.DefaultMotorState;
                        listMotors.Where(w => Math.Abs(w.PulseWidthNormalized - -1.0f) > 0).ToList().ForEach(s => s.PulseWidthNormalized = -1);
                        receivedSpeechData = receivedSpeechData.Replace("(", "").Replace(")", "");

                        List<string> res= receivedSpeechData.Split(' ').ToList();


                        int timesample = Convert.ToInt32(Convert.ToDouble(res[0], new CultureInfo("en-US")));

                        res.RemoveAt(0);

                        foreach(string  i in res)
                        {
                            if ((float.Parse(i, new CultureInfo("en-US")) >= 0 && float.Parse(i, new CultureInfo("en-US")) <= 1) || Math.Abs(float.Parse(i, new CultureInfo("en-US")) - -1.0) < 0)
                            {
                                listMotors.Find(a => a.Name == "Jaw").PulseWidthNormalized = 1.0f - float.Parse(i, new CultureInfo("en-US"));
                                yarpPortSetFacialMotors.sendData(ComUtils.XmlUtils.Serialize<List<ServoMotor>>(listMotors));

                                System.Threading.Thread.Sleep(timesample);

                            }
                            else
                            {
                                return;
                            }
                        }




                        receivedSpeechData = null;

                        listMotors.Clear();

                    }
                    catch (Exception exc)
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                        {
                            lblLogDebug.Content = "Error ReceiverDataSpeech:" + exc.Message;
                        }));
                        Console.WriteLine("Error XML Set motors: " + exc.Message);
                    }

                }
            }
        }

        void ReceiveDataFeedback(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                yarpPortFeedback.receivedData(out receivedFeedback);
                if (receivedFeedback != null && receivedFeedback != "")
                {
                    try
                    {

                        lock (this)
                        {

                            listMotorsFeedback = ComUtils.XmlUtils.Deserialize<List<ServoMotor>>(receivedFeedback);

                            if (RobotControl.CurrentMotorState== null || RobotControl.CurrentMotorState.Count==0 || RobotControl.CurrentMotorState.Count != listMotorsFeedback.Count)
                            {
                                RobotControl.LoadServoMotor(listMotorsFeedback);
                                motionLookAt = new RobotMotion(RobotControl.CurrentMotorState.Count);
                                motionECS = new RobotMotion(RobotControl.CurrentMotorState.Count);
                            }

                    
                            yarpPortFeedbackXML.sendData(ComUtils.XmlUtils.Serialize<List<ServoMotor>>(listMotorsFeedback));
                            yarpPortFeedbackJSON.sendData(JsonConvert.SerializeObject(listMotorsFeedback));

                            receivedFeedback = null;
                            ListFeedbackOnlyPositions.Clear();
                        }
                    }
                    catch (Exception exc)
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                        {
                            lblLogDebug.Content = "Error ReceiveDataFeedback Feedback:" + exc.Message;
                        }));
                        Console.WriteLine("Error ReceiveDataFeedback Feedback: " + exc.Message);
                    }

                }
            }
        }

        private void SetFacialExpression(float pleasure, float arousal, int animationDurationMs = -1)
        {
            try
            {
                if (motionECS == null)
                    return;

                motionECS.ServoMotorsList.Where(w => w.PulseWidthNormalized != -1).ToList().ForEach(s => s.PulseWidthNormalized = -1);

                foreach (ECSMotor m in ecs.ECSMotorList)
                {
                    float f = ecs.GetECSValue(m.Channel, pleasure, arousal);
                    motionECS.ServoMotorsList[m.Channel].PulseWidthNormalized = f;
                    Console.WriteLine(f);
                }

                lock (this)
                {
                    string notxml = ComUtils.XmlUtils.Serialize<List<ServoMotor>>(motionECS.ServoMotorsList);
                    if(animationDurationMs > 0)
                    {
                        notxml += "@" + animationDurationMs.ToString() + "@";
                    }
                    yarpPortSetFacialMotors.sendData(notxml);
                }

               
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LookAt(float x, float y)
        {
            try
            {
                if (motionLookAt == null)
                    return;

                int lookAtDuration = 120; //the lookat function works every 110msec so it can't act longer requests
                float minAmplitudeX = 0.002f, minAmplitudeY = 0.002f, maxAmplitudeX = 1.0f, maxAmplitudeY = 1.0f;
                lock (this)
                {
                    old_PosX = listMotorsFeedback[(int)MotorsNames.Turn].PulseWidthNormalized;
                    old_PosY = listMotorsFeedback[(int)MotorsNames.UpperNod].PulseWidthNormalized;

                }
                motionLookAt.ServoMotorsList.Where(w => w.PulseWidthNormalized != -1.0f).ToList().ForEach(s => s.PulseWidthNormalized = -1);
                motionLookAt.Duration = lookAtDuration;
                motionLookAt.Priority = 10;

                motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = old_PosX;
                motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = old_PosY;


                if (Math.Abs(x - old_PosX) > minAmplitudeX || Math.Abs(y - old_PosY) > minAmplitudeY)
                {
                    if (Math.Abs(x - old_PosX) > minAmplitudeX)
                    {
                        if (Math.Abs(x - old_PosX) < maxAmplitudeX)
                            motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = x;
                        else
                        {
                            if (x - old_PosX > 0)
                                motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = old_PosX + maxAmplitudeX;
                            else
                                motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = old_PosX - maxAmplitudeX;
                        }
                    }

                    if (Math.Abs(y - old_PosY) > minAmplitudeY)
                    {
                        if (Math.Abs(x - old_PosY) < maxAmplitudeY)
                            motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = y;
                        else
                        {
                            if (y - old_PosY > 0)
                                motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = old_PosY + maxAmplitudeY;
                            else
                                motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = old_PosY - maxAmplitudeY;
                        }
                    }

                    //motionLookAt.ServoMotorsList[(int)MotorsNames.Tilt].PulseWidthNormalized = 1 - motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized;
                    lock (this)
                    {
                        yarpPortSetFacialMotors.sendData(ComUtils.XmlUtils.Serialize<List<ServoMotor>>(motionLookAt.ServoMotorsList));

                    }
                   // Console.WriteLine("Turn " + motionToTest.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized + " old Position: " + old_PosX.ToString() + " New Position: " + x.ToString());
                    // Console.WriteLine("UpperNod " + motionToTest.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized + " old Position: " + old_PosY.ToString() + " New Position: " + y.ToString());


                    if (motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized < 0)
                        Console.WriteLine("Error Turn " + motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized + " old Position: " + old_PosX.ToString() + " New Position: " + x.ToString());
                    else if (motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized < 0)
                        Console.WriteLine("Error UpperNod " + motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized + " old Position: " + old_PosY.ToString() + " New Position: " + y.ToString());



                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void LookAtMe(float x, float y)
        //{
        //    try
        //    {
        //        if (motionLookAt == null)
        //            return;

        //        int lookAtDuration = 120; //the lookat function works every 110msec so it can't act longer requests
        //        float minAmplitudeX = 0.002f, minAmplitudeY = 0.002f, maxAmplitudeX = 1.0f, maxAmplitudeY = 1.0f;
        //        lock (this)
        //        {
        //            old_PosX = listMotorsFeedback[(int)MotorsNames.Turn].PulseWidthNormalized;
        //            old_PosY = listMotorsFeedback[(int)MotorsNames.UpperNod].PulseWidthNormalized;
                    
                    
        //            //old_PosX = RobotControl.CurrentMotorState[(int)AbelRobotControl.MotorsNames.EyeUPRight].PulseWidthNormalized;


        //        }
        //        motionLookAt.ServoMotorsList.Where(w => w.PulseWidthNormalized != -1.0f).ToList().ForEach(s => s.PulseWidthNormalized = -1);
        //        motionLookAt.Duration = lookAtDuration;
        //        motionLookAt.Priority = 10;

        //        motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = old_PosX;
        //        motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = old_PosY;

        //        if (Math.Abs(x - old_PosX) > minAmplitudeX || Math.Abs(y - old_PosY) > minAmplitudeY)
        //        {
        //            if (Math.Abs(x - old_PosX) > minAmplitudeX)
        //            {
        //                if (Math.Abs(x - old_PosX) < maxAmplitudeX)
        //                    motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = x;
        //                else
        //                {
        //                    if (x - old_PosX > 0)
        //                        motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = old_PosX + maxAmplitudeX;
        //                    else
        //                        motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized = old_PosX - maxAmplitudeX;
        //                }
        //            }

        //            if (Math.Abs(y - old_PosY) > minAmplitudeY)
        //            {
        //                if (Math.Abs(x - old_PosY) < maxAmplitudeY)
        //                    motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = y;
        //                else
        //                {
        //                    if (y - old_PosY > 0)
        //                        motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = old_PosY + maxAmplitudeY;
        //                    else
        //                        motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized = old_PosY - maxAmplitudeY;
        //                }
        //            }

        //            //motionLookAt.ServoMotorsList[(int)MotorsNames.Tilt].PulseWidthNormalized = 1 - motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized;
        //            lock (this)
        //            {
        //                yarpPortSetFacialMotors.sendData(ComUtils.XmlUtils.Serialize<List<ServoMotor>>(motionLookAt.ServoMotorsList));

        //            }
        //            // Console.WriteLine("Turn " + motionToTest.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized + " old Position: " + old_PosX.ToString() + " New Position: " + x.ToString());
        //            // Console.WriteLine("UpperNod " + motionToTest.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized + " old Position: " + old_PosY.ToString() + " New Position: " + y.ToString());


        //            if (motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized < 0)
        //                Console.WriteLine("Error Turn " + motionLookAt.ServoMotorsList[(int)MotorsNames.Turn].PulseWidthNormalized + " old Position: " + old_PosX.ToString() + " New Position: " + x.ToString());
        //            else if (motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized < 0)
        //                Console.WriteLine("Error UpperNod " + motionLookAt.ServoMotorsList[(int)MotorsNames.UpperNod].PulseWidthNormalized + " old Position: " + old_PosY.ToString() + " New Position: " + y.ToString());



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void CheckStatusYarp(object source, ElapsedEventArgs e)
        {
            #region lookAt_out
            bool status = yarpPortLookAt.PortExists(lookAt_out);

            if (typeRobot == TypeRobot.FACE)
            {
                
                if (status && colLookAt == "red")
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                        {
                            EllLookAt.Fill = Brushes.Green;
                        }));
                    colLookAt = "green";
                }
                else if (!status && colLookAt == "green")
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { EllLookAt.Fill = Brushes.Red; }));
                    colLookAt = "red";
                }
            }
            else if(colLookAt == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { EllLookAt.Fill = Brushes.Red; }));
                colLookAt = "red";
            }
            #endregion

            #region ecs_out
            status = yarpPortSetECS.PortExists(ecs_out);
            if (status && colECS == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { Ellexp.Fill = Brushes.Green; }));
                colECS = "green";
            }
            else if (!status && colECS == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { Ellexp.Fill = Brushes.Red; }));
                colECS = "red";
            }
            #endregion

            #region facialexpression_out
            status = yarpPortSetFacialExpression.PortExists(facialexpression_out);
            if (status && colExp == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { EllSetFace.Fill = Brushes.Green; }));
                colExp = "green";
            }
            else if (!status && colExp == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { EllSetFace.Fill = Brushes.Red; }));
                colExp = "red";
            }
            #endregion

            #region reflexes_out
            status = false;// yarpPortSetFacialExpression.PortExists(reflexes_out);
            if (status && colRef == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { EllRef.Fill = Brushes.Green; }));
                colRef = "green";
            }
            else if (!status && colRef == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { EllRef.Fill = Brushes.Red; }));
                colRef = "red";
            }
            #endregion

            #region setFacialMotors_in
            status = yarpPortSetFacialExpression.PortExists(setFacialMotors_in);
            if (status && colMot == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { EllMot.Fill = Brushes.Green; }));
                colMot = "green";
            }
            else if (!status && colMot == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { EllMot.Fill = Brushes.Red; }));
                colMot = "red";
            }
            #endregion

            #region speech
            status = yarpPortSetECS.PortExists(speech_out);
            if (status && colSpe == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { EllSpeech.Fill = Brushes.Green; }));
                colSpe = "green";
            }
            else if (!status && colSpe == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { EllSpeech.Fill = Brushes.Red; }));
                colSpe = "red";
            }
            #endregion

            #region check Network
            if (yarpPortLookAt.NetworkExists() && colYarp == "red")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() 
                { 
                    Ellyarp.Fill = Brushes.Green;
                    EllFeedXML.Fill = Brushes.Green;
                    EllFeedJson.Fill = Brushes.Green;
                    EllFeedBottle.Fill = Brushes.Green;
                }));
                colYarp = "green";
            }
            else if (!yarpPortLookAt.NetworkExists() && colYarp == "green")
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() 
                    { 
                        Ellyarp.Fill = Brushes.Red;
                        EllFeedXML.Fill = Brushes.Red;
                        EllFeedJson.Fill = Brushes.Red;
                        EllFeedBottle.Fill = Brushes.Red;
                    
                    }));
                colYarp = "red";
            }

       

            #endregion
        }

      

        public reflexes GetReflexName(int i)
        {
            string name = Enum.GetName(typeof(reflexes), i);
            if (null == name) throw new Exception();
            return (reflexes)Enum.Parse(typeof(reflexes), name);
        }

        private void YarpDisconnect()
        {


            if (TimerCheckStatusYarp != null)
            {
                TimerCheckStatusYarp.Elapsed -= new ElapsedEventHandler(CheckStatusYarp);
                TimerCheckStatusYarp.Stop();
            }


            //if (yarpPortLookAt != null)
            //    yarpPortLookAt.Disconect();


            //if (yarpPortSetECS != null)
            //    yarpPortSetECS.Disconect();

            //if (yarpPortSpeech != null)
            //    yarpPortSpeech.Disconect();

            //if (yarpPortSetFacialExpression != null)
            //    yarpPortSetFacialExpression.Disconect();

            //if (yarpPortFeedbackJSON != null)
            //    yarpPortFeedbackJSON.Disconect();

            //if (yarpPortFeedbackXML != null)
            //    yarpPortFeedbackXML.Disconect();

            //if (yarpPortFeedbackBOTTLE != null)
            //    yarpPortFeedbackBOTTLE.Disconect();

            //if (yarpPortFeedback != null)
            //    yarpPortFeedback.Disconect();

        }


        private void YarpClose()
        {

            ctsYarp.Cancel();

            if (TimerCheckStatusYarp != null)
            { 
                TimerCheckStatusYarp.Elapsed -= new ElapsedEventHandler(CheckStatusYarp);
                TimerCheckStatusYarp.Stop();
            }

            if (yarpPortLookAt != null)
                yarpPortLookAt.Close();
            

            if (yarpPortSetECS != null)
                yarpPortSetECS.Close();

            if (yarpPortSpeech != null)
                yarpPortSpeech.Close();

            if (yarpPortSetFacialExpression != null)
                yarpPortSetFacialExpression.Close();

            if (yarpPortFeedbackJSON != null)
                yarpPortFeedbackJSON.Close();

            if (yarpPortFeedbackXML != null)
                yarpPortFeedbackXML.Close();

            if (yarpPortFeedbackBOTTLE != null)
                yarpPortFeedbackBOTTLE.Close();

            if (yarpPortFeedback != null)
                yarpPortFeedback.Close();

            if (yarpPortSetFacialMotors != null)
                yarpPortSetFacialMotors.Close();

        }

        private void Window_Closing(object sender, EventArgs e)
        {
            YarpClose();

            ctsYarp.Dispose();
        }

        private void lbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label lbl = sender as Label;

            string port = lbl.Content.ToString();
            string portName="";

    
            if (port == lookAt_in && colLookAt != "red")
                portName = lookAt_out;
            else if (port == ecs_in && colECS != "red")
                portName = ecs_out;
            else if (port == facialexpression_in && colExp != "red")
                portName = facialexpression_out;
            else if (port == reflexes_in && colRef != "red")
                portName = reflexes_out;
            else if (port.Last() == 'o')
                portName = port;


            if (portName == "")
                MessageBox.Show("Output Port not connected");
            else
            {
                monitor = new YarpMonitor(portName);
                monitor.ShowDialog();

                portName = "";
                monitor = null;
            }
        }


        private void robot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (System.Windows.Application.Current.MainWindow.IsInitialized == false)
                return;


            switch (((ComboBox)e.OriginalSource).SelectedValue)
            {
                case "FACE":
                    typeRobot = TypeRobot.FACE;
                    break;
                default:
                    typeRobot = TypeRobot.Abel;
                    break;
            }

            ctsYarp.Cancel();

            YarpClose();

            loadRobotConfig();

            InitYarp();



        }

    }
}
