using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Media;
using SharpDX.XInput;
using System.Timers;

using YarpManagerCS;
using Newtonsoft.Json;

namespace Act.Control.GamepadController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Event
        public static readonly RoutedEvent ButtonClickedEvent = EventManager.RegisterRoutedEvent("ButtonClickedEventHandler",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        public event RoutedEventHandler ButtonClickedEventHandler
        {
            add { AddHandler(ButtonClickedEvent, value); }
            remove { RemoveHandler(ButtonClickedEvent, value); }
        }

        public static readonly RoutedEvent TriggerClickedEvent = EventManager.RegisterRoutedEvent("TriggerClickedEventHandler",
           RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        public event RoutedEventHandler TriggerClickedEventHandler
        {
            add { AddHandler(TriggerClickedEvent, value); }
            remove { RemoveHandler(TriggerClickedEvent, value); }
        }

        public static readonly RoutedEvent AxisLeftChangedEvent = EventManager.RegisterRoutedEvent("AxisLeftChangedEventHandler",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        public event RoutedEventHandler AxisLeftChangedEventHandler
        {
            add { AddHandler(AxisLeftChangedEvent, value); }
            remove { RemoveHandler(AxisLeftChangedEvent, value); }
        }

        public static readonly RoutedEvent AxisRightChangedEvent = EventManager.RegisterRoutedEvent("AxisRightChangedEventHandler",
           RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        public event RoutedEventHandler AxisRightChangedEventHandler
        {
            add { AddHandler(AxisRightChangedEvent, value); }
            remove { RemoveHandler(AxisRightChangedEvent, value); }
        }
        #endregion
        /// <summary>
        /// Thread reader controller state
        /// </summary>
        System.Threading.Thread thrController;

        /// <summary>
        /// 
        /// </summary>
        Controller controller = null;



        /// <summary>
        /// Save previous state controller
        /// </summary>
        State previousState;


        /// <summary>
        /// Timer to hide point yellow
        /// </summary>
        System.Timers.Timer timer = new System.Timers.Timer();

        private YarpPort yarpPortButtons;
        private YarpPort yarpPortAxisLeft;
        private YarpPort yarpPortAxisRight;

        private YarpPort yarpPortTrigger;

        public MainWindow()
        {
            InitializeComponent();


            InitController();

            if (controller == null)
            {
                Console.WriteLine("No XInput controller installed");
                lblNoConnected.Visibility = Visibility.Visible;
            }
            else
            {



                thrController = new System.Threading.Thread(readController);
                thrController.Start();

                timer.Interval = 1000;
                timer.Elapsed += OnTimedEventHiddenPoint;

                InitUI();

                ThreadPool.QueueUserWorkItem(InitYarp);

                this.AxisLeftChangedEventHandler += MainWindow_AxisLeftChangedEventHandler;
                this.AxisRightChangedEventHandler += MainWindow_AxisRightChangedEventHandler;
                this.ButtonClickedEventHandler += MainWindow_ButtonClickedEventHandler;
                this.TriggerClickedEventHandler += MainWindow_TriggerClickedEventHandler;


            }
            Console.WriteLine("End XGamepadApp");
        }

        private void InitYarp(object s)
        {


            yarpPortButtons = new YarpPort();
            yarpPortButtons.openSender("/GamepadController/Buttons:o");

            yarpPortAxisLeft = new YarpPort();
            yarpPortAxisLeft.openSender("/GamepadController/Axis/Left:o");

            yarpPortAxisRight = new YarpPort();
            yarpPortAxisRight.openSender("/GamepadController/Axis/Right:o");

            yarpPortTrigger = new YarpPort();
            yarpPortTrigger.openSender("/GamepadController/Trigger:o");

            if (yarpPortTrigger.NetworkExists())
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { Ellyarp.Fill = Brushes.Green; }));
            }

        }

        private void InitUI()
        {
            Gamepad configGamepad;
            try
            {
                using (StreamReader r = new StreamReader("configGamepad.json"))
                {
                    string json = r.ReadToEnd();
                    configGamepad = JsonConvert.DeserializeObject<Gamepad>(json);
                }

                lblA.Content = configGamepad.A;
                lblB.Content = configGamepad.B;
                lblX.Content = configGamepad.X;
                lblY.Content = configGamepad.Y;


                lblDPadDown.Content = configGamepad.DPadDown;
                lblDPadLeft.Content = configGamepad.DPadLeft;
                lblDPadRight.Content = configGamepad.DPadRight;
                lblDPadUp.Content = configGamepad.DPadUp;

                lblLeftShoulder.Content = configGamepad.LeftShoulder;
                lblLeftThumb.Content = configGamepad.LeftThumb;
                lblRightShoulder.Content = configGamepad.RightShoulder;
                lblRightThumb.Content = configGamepad.RightThumb;
                lblRightTrigger.Content = configGamepad.RightTrigger;
                lblLeftTrigger.Content = configGamepad.LeftTrigger;

            }
            catch (Exception ex)
            {


            }
        }

        private void InitController()
        {
            // Initialize XInput
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            // Get 1st controller available
            foreach (var selectControler in controllers)
            {
                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }
        }
        #region event method
        private void MainWindow_TriggerClickedEventHandler(object sender, RoutedEventArgs e)
        {
            if (Ellyarp.Fill == Brushes.Green)
            {
                List<string> p = (List<string>)e.OriginalSource;

                yarpPortTrigger.sendData(JsonConvert.SerializeObject(p));
            }
        }

        private void MainWindow_ButtonClickedEventHandler(object sender, RoutedEventArgs e)
        {
            if (Ellyarp.Fill == Brushes.Green)
            {
                GamepadButtonFlags p = (GamepadButtonFlags)e.OriginalSource;

                yarpPortButtons.sendData(JsonConvert.SerializeObject(p.ToString()));
            }
        }

        private void MainWindow_AxisRightChangedEventHandler(object sender, RoutedEventArgs e)
        {
            if (Ellyarp.Fill == Brushes.Green)
            {
                Point p = (Point)e.OriginalSource;
                yarpPortAxisRight.sendData(JsonConvert.SerializeObject(p));
            }
        }

        private void MainWindow_AxisLeftChangedEventHandler(object sender, RoutedEventArgs e)
        {
            if (Ellyarp.Fill == Brushes.Green)
            {
                Point p = (Point)e.OriginalSource;
                yarpPortAxisLeft.sendData(JsonConvert.SerializeObject(p));
            }
        }

        #endregion

        public void readController()
        {
            previousState = controller.GetState();
            while (true)
            {

                if (controller.IsConnected)
                {

                    try
                    {
                        var state = controller.GetState();

                        if (previousState.PacketNumber != state.PacketNumber)
                        {
                            timer.Stop();
                            Console.WriteLine(state.Gamepad);
                            updateController(state.Gamepad);
                            lblGamapad.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                            {
                                lblGamapad.Content = state.Gamepad;
                            }));

                            previousState = state;

                        }
                        else
                        {
                            timer.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    InitController();
                }
                System.Threading.Thread.Sleep(100);
            }
        }

        public void updateController(SharpDX.XInput.Gamepad gp)
        {
            if (gp.Buttons != GamepadButtonFlags.None && gp.Buttons != previousState.Gamepad.Buttons)
            {
                #region button
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    RaiseEvent(new RoutedEventArgs(ButtonClickedEvent, gp.Buttons));
                });

                switch (gp.Buttons)
                {
                    case GamepadButtonFlags.A:
                        updateYellowPoint(522, 258);
                        break;

                    case GamepadButtonFlags.B:
                        updateYellowPoint(546, 236);

                        break;

                    case GamepadButtonFlags.X:
                        updateYellowPoint(499, 235);
                        break;

                    case GamepadButtonFlags.Y:
                        updateYellowPoint(523, 213);
                        break;

                    case GamepadButtonFlags.Back:
                        updateYellowPoint(410, 235);
                        break;

                    case GamepadButtonFlags.DPadDown:
                        updateYellowPoint(389, 311);
                        break;

                    case GamepadButtonFlags.DPadLeft:
                        updateYellowPoint(366, 291);
                        break;

                    case GamepadButtonFlags.DPadRight:
                        updateYellowPoint(411, 291);
                        break;

                    case GamepadButtonFlags.DPadUp:
                        updateYellowPoint(389, 269);
                        break;

                    case GamepadButtonFlags.LeftShoulder:
                        updateYellowPoint(368, 177, 10);
                        break;

                    case GamepadButtonFlags.LeftThumb:
                        updateYellowPoint(347, 234, 10);
                        System.Threading.Thread.Sleep(1000);
                        updateYellowPoint(347, 234);

                        break;

                    case GamepadButtonFlags.RightShoulder:
                        updateYellowPoint(503, 177, 10);
                        break;

                    case GamepadButtonFlags.RightThumb:
                        updateYellowPoint(482, 287, 10);
                        System.Threading.Thread.Sleep(1000);
                        updateYellowPoint(482, 287);


                        break;

                    case GamepadButtonFlags.Start:
                        updateYellowPoint(459, 235);
                        break;

                }
                #endregion
            }

            if (gp.LeftThumbX != previousState.Gamepad.LeftThumbX || gp.LeftThumbY != previousState.Gamepad.LeftThumbY)
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    RaiseEvent(new RoutedEventArgs(AxisLeftChangedEvent, new Point(gp.LeftThumbX, gp.LeftThumbY)));
                });

                if (gp.LeftThumbX > 20000 && gp.LeftThumbY > 20000)
                    updateYellowPoint(355, 226);
                else if (gp.LeftThumbX < -20000 && gp.LeftThumbY > 20000)
                    updateYellowPoint(338, 226);
                else if (gp.LeftThumbX < -20000 && gp.LeftThumbY < -20000)
                    updateYellowPoint(338, 242);
                else if (gp.LeftThumbX > 20000 && gp.LeftThumbY < -20000)
                    updateYellowPoint(355, 242);
                else if (gp.LeftThumbX > 25000)
                    updateYellowPoint(358, 234);
                else if (gp.LeftThumbX < -25000)
                    updateYellowPoint(335, 234);
                else if (gp.LeftThumbY > 25000)
                    updateYellowPoint(347, 224);
                else if (gp.LeftThumbY < -25000)
                    updateYellowPoint(347, 246);

            }

            if (gp.RightThumbX != previousState.Gamepad.RightThumbX || gp.RightThumbY != previousState.Gamepad.RightThumbY)
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    RaiseEvent(new RoutedEventArgs(AxisRightChangedEvent, new Point(gp.RightThumbX, gp.RightThumbY)));
                });

                if (gp.RightThumbX > 20000 && gp.RightThumbY > 20000)
                    updateYellowPoint(486, 278);
                else if (gp.RightThumbX < -20000 && gp.RightThumbY > 20000)
                    updateYellowPoint(472, 278);
                else if (gp.RightThumbX < -20000 && gp.RightThumbY < -20000)
                    updateYellowPoint(472, 293);
                else if (gp.RightThumbX > 20000 && gp.RightThumbY < -20000)
                    updateYellowPoint(491, 293);
                else if (gp.RightThumbX > 25000)
                    updateYellowPoint(492, 287);
                else if (gp.RightThumbX < -25000)
                    updateYellowPoint(467, 287);
                else if (gp.RightThumbY > 25000)
                    updateYellowPoint(480, 275);
                else if (gp.RightThumbY < -25000)
                    updateYellowPoint(480, 299);
            }

            if (gp.LeftTrigger != previousState.Gamepad.LeftTrigger)
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    List<string> date = new List<string>() { "left", ((int)gp.LeftTrigger).ToString() };
                    RaiseEvent(new RoutedEventArgs(TriggerClickedEvent, date));
                });

                updateYellowPoint(338, 170, 10);

            }

            if (gp.RightTrigger != previousState.Gamepad.RightTrigger)
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    List<string> date = new List<string>() { "right", ((int)gp.RightTrigger).ToString() };
                    RaiseEvent(new RoutedEventArgs(TriggerClickedEvent, date));
                });

                updateYellowPoint(533, 170, 10);


            }
        }

        public void updateYellowPoint(int left, int top, int size = 20)
        {

            pointYel.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                pointYel.Visibility = Visibility.Visible;

                Canvas.SetLeft(pointYel, left);
                Canvas.SetTop(pointYel, top);
                pointYel.Width = size;
                pointYel.Height = size;
            }));


        }

        public void OnTimedEventHiddenPoint(Object source, ElapsedEventArgs e)
        {
            pointYel.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                pointYel.Visibility = Visibility.Hidden;
            }));
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);



            if (timer != null)
                timer.Stop();


            if (yarpPortAxisLeft != null)
                yarpPortAxisLeft.Close();

            if (yarpPortButtons != null)
                yarpPortButtons.Close();

            if (yarpPortTrigger != null)
                yarpPortTrigger.Close();

            if (yarpPortAxisRight != null)
                yarpPortAxisRight.Close();



            if (thrController != null)
                thrController.Abort();


            Close();
        }
    }

}