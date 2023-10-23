using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Globalization;
using System.Threading;

using ControllersLibrary;
using YarpManagerCS;
using Newtonsoft.Json;

namespace Act.Face.FACEGui20
{
    /// <summary>
    /// Interaction logic for ECSWin.xaml
    /// </summary>
    public partial class LookAtWin : Window
    {
        //The first parameter specifies the name of the event that clients will use to add handlers to the event. 
        //This name has to be unique within the new control.Next we specify the type of routing, bubble in this case, 
        //and the type of the event handler and the type of object that can fire the event. The event handler delegate, 
        //RoutedEventHandler in this case, specifies the signature of any method that is to be added to the invocation list.

        public static readonly RoutedEvent LookAtEvent = 
            EventManager.RegisterRoutedEvent("LookAtEventHandler", 
                RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler), 
                typeof(LookAtWin));

        public event RoutedEventHandler LookAtEventHandler
        {
            add { AddHandler(LookAtEvent, value); }
            remove { RemoveHandler(LookAtEvent, value); }
        }

        private Point currentLookAt;
        public Point CurrentLookAt
        {
            get { return currentLookAt; }
            set { SetLookAt(value); }
        }


        //private YarpPort yarpPortLookAtEyes;

        private YarpPort yarpPortRecever;
        string receivedLookAt = "";
        CancellationTokenSource cts = new CancellationTokenSource();

        double speed = 0.01;

        public LookAtWin()
        {
            InitializeComponent();

            currentLookAt = new Point(0, 0);

            //timerAnimation = new DispatcherTimer();
            //timerAnimation.Tick += new EventHandler(timerAnimation_Tick);

            ThreadPool.QueueUserWorkItem(InitYarp);

        }




        private void MouseClickOnCanvas(object sender, MouseButtonEventArgs e)
        {
            Point mouseposition = new Point((Mouse.GetPosition(ECSCanvas).X / ECSCanvas.ActualWidth), (Mouse.GetPosition(ECSCanvas).Y / ECSCanvas.ActualHeight));
            SetLookAt(mouseposition);
        }


    

        #region Yarp
        private void InitYarp(object s)
        {

            //yarpPortLookAtEyes = new YarpPort();
            yarpPortRecever = new YarpPort();
            yarpPortRecever.openReceiver("/GamepadController/Axis/Left:o", "/LookAt/Axis/Left:i");
            //yarpPortLookAtEyes.openReceiver("/AttentionModule/LookAtEyes:o", "/FACEGui20/yarpPortLookAtEyes:i");

            //if (yarpPortTrigger.NetworkExists())
            //{
            //    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { Ellyarp.Fill = Brushes.Green; }));
            //}

            ThreadPool.QueueUserWorkItem(tok =>
            {
                CancellationToken token = (CancellationToken)tok;
                while (!token.IsCancellationRequested)
                {
                    ReceiveDataLookAt();
                }
            }, cts.Token);




        }

        void ReceiveDataLookAt()
        {


            yarpPortRecever.receivedData(out receivedLookAt);
            //yarpPortLookAtEyes.receivedData(out receivedLookAt);
            if (receivedLookAt != null && receivedLookAt != "")
            {
                //Debug.WriteLine(receivedSetMotors);
                try
                {
                    receivedLookAt = receivedLookAt.Replace(@"\", "").Replace("\"", "");
                    Point p = new Point();
                    p.X = Convert.ToDouble(receivedLookAt.Split(',')[0]);
                    p.Y = Convert.ToDouble(receivedLookAt.Split(',')[1]);// JsonConvert.DeserializeObject<Point>(receivedECS);
                    Point current = currentLookAt;

                    if (p.X > 20000 && p.Y > 20000)
                    {
                        current.X += speed;
                        current.Y -= speed;
                    }
                    else if (p.X < -20000 && p.Y > 20000)
                    {
                        current.X -= speed;
                        current.Y -= speed;
                    }
                    else if (p.X < -20000 && p.Y < -20000)
                    {
                        current.X -= speed;
                        current.Y += speed;
                    }
                    else if (p.X > 20000 && p.Y < -20000)
                    {
                        current.X += speed;
                        current.Y += speed;
                    }
                    else if (p.X > 25000)
                    {
                        current.X += speed;

                    }
                    else if (p.X < -25000)
                    {
                        current.X -= speed;

                    }
                    else if (p.Y > 25000)
                    {

                        current.Y -= speed;
                    }
                    else if (p.Y < -25000)
                    {

                        current.Y += speed;
                    }

                    if (current.Y > 1)
                        current.Y = 1;
                    else if (current.Y < 0)
                        current.Y = 0;

                    if (current.X > 1)
                        current.X = 1;
                    else if (current.X < 0)
                        current.X = 0;


                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                               new Action(delegate ()
                               {
                                   SetLookAt(current);

                               }));

                }
                catch (Exception exc)
                {
                    Debug.WriteLine("Error XML Set motors: " + exc.Message);
                }



            }
        }
        #endregion

        private void SetLookAt(Point position)
        {
            currentLookAt = position;
            DrawCurrentLookAtpoint(currentLookAt);
            RaiseEvent(new RoutedEventArgs(LookAtWin.LookAtEvent, currentLookAt));
        }

        #region Tools Draw

        private void MouseOnCanvas(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }


        public void LoadLookAtPoint(string label, Point position)
        {
            DrawStoredLookAtpoint(label, position);
        }

        private void DrawStoredLookAtpoint(string label, Point expression)
        {

            double x = (expression.X) * (ECSCanvas.ActualWidth);
            double y = (expression.Y) * (ECSCanvas.ActualHeight);

            Ellipse p = new Ellipse();
            p.Fill = Brushes.ForestGreen;
            p.StrokeThickness = 1;
            p.Stroke = Brushes.Black;
            p.Width = 12;
            p.Height = 12;
            p.Margin = new Thickness(x - 6, y - 6, 0, 0);
            ECSCanvas.Children.Add(p);

            Label l = new Label();
            l.Content = label;
            l.Foreground = Brushes.Black;


            l.Margin = new Thickness(x, y + 10, 0, 0);



            ECSCanvas.Children.Add(l);
        }

        private void DrawCurrentLookAtpoint(Point point)
        {
            double x = (point.X) * (ECSCanvas.ActualWidth);
            double y = ((point.Y) * (ECSCanvas.ActualHeight));

            //double y = ECSCanvas.ActualHeight - ((point.Y + 1) * (ECSCanvas.ActualHeight));

            CurrentECSLabel.Margin = new Thickness(x, y + 5, 0, 0);
            CurrentECSLabel.Content = ("(" + (decimal.Round((decimal)point.X, 2)).ToString() + ", " + (decimal.Round((decimal)point.Y, 2)).ToString() + ")");
            Position_Star.Margin = new Thickness(x - 6, y - 6, 0, 0);
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            cts.Cancel();

            if (yarpPortRecever.PortExists("/GamepadController/Axis/Left:o"))
                yarpPortRecever.Disconect("/LookAt/Axis/Left:i", "/GamepadController/Axis/Left:o");

            //timer.Stop();

            if (yarpPortRecever != null)
                yarpPortRecever.Close();

            //if (yarpPortLookAtEyes != null)
            //    yarpPortLookAtEyes.Close();
        }
    }
}
///// <summary>
///// 
///// </summary>
//private DispatcherTimer timerAnimation;
///// <summary>
///// Length of a step 
///// </summary>
//private Point LengthStep = new Point();

///// <summary>
///// Desired number of steps to complete movement
///// </summary>
//private int desiredsteps = 1000;
//private int stepcounter = 0;

//private void Speed_TextChanged(object sender, TextChangedEventArgs e)
//{
//    try
//    {
//        double new_speed = Convert.ToDouble(txtSpeed.Text, NumberFormatInfo.InvariantInfo);
//        if (new_speed > 0 || speed <= 1)
//        {
//            speed = new_speed;
//        }
//        else
//            txtSpeed.Text = String.Format(speed.ToString(CultureInfo.InvariantCulture));

//    }
//    catch
//    {

//        txtSpeed.Text = String.Format(speed.ToString(CultureInfo.InvariantCulture));
//    }
//}

//private void StartAnimation(Point newposition, int speed)
//{
//    timerAnimation.Interval = new TimeSpan(speed);
//    LengthStep.X = (newposition.X - currentLookAt.X) / desiredsteps;
//    LengthStep.Y = (newposition.Y - currentLookAt.Y) / desiredsteps;
//    stepcounter = 0;
//    timerAnimation.Start();
//}

//private void timerAnimation_Tick(object sender, EventArgs e)
//{
//    if (stepcounter < desiredsteps)
//    {
//        currentLookAt.X = currentLookAt.X + LengthStep.X;
//        currentLookAt.Y = currentLookAt.Y + LengthStep.Y;
//        SetLookAt(currentLookAt);
//        stepcounter++;
//    }
//}