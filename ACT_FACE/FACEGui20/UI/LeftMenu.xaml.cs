using Act.Lib;
using Act.Lib.Control;
using Act.Lib.ControllersLibrary;
using Act.Lib.Robot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class LeftMenu : UserControl
    {
        private FACEGui20Win parent;

        private ECSWin ecsWin;
        private LookAtWin lookAtWin;

        //creo/registro l'evento in EventManager
        public static readonly RoutedEvent NewLookAtEvent =
            EventManager.RegisterRoutedEvent("NewLookAtEvent", 
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), 
                typeof(LeftMenu));

        public static readonly RoutedEvent NewEcsEvent =
           EventManager.RegisterRoutedEvent("NewEcsEvent",
               RoutingStrategy.Bubble,
               typeof(RoutedEventHandler),
               typeof(LeftMenu));


        //definisco un delegate che si occupare di definire 
        //i metodi da invocare al verificare di un determinato evento  (esposto nel xaml)
        public event RoutedEventHandler LookAtMouseClickOnCanvas
        {
            add { AddHandler(NewLookAtEvent, value); }
            remove { RemoveHandler(NewLookAtEvent, value); }
        }

        public event RoutedEventHandler ECSMouseClickOnCanvas
        {
            add { AddHandler(NewEcsEvent, value); }
            remove { RemoveHandler(NewEcsEvent, value); }
        }

        public LeftMenu()
        {
            InitializeComponent();
           
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            parent = (FACEGui20Win) Window.GetWindow(this);

            if (parent == null)
                throw new Exception("LeftMenu UserControls should be inserted in Window before being loaded");
            else
                parent.Closing += OnClosing;

        }

        private void resetViewMode()
        {
            System.Windows.Controls.Image img = null;

            switch (parent.visualMode)
            {
                case Mode.View:
                    parent.ViewGrid.Visibility = Visibility.Hidden;
                    img = ViewButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/view.png")));
                    break;
                case Mode.Edit:
                    parent.EditGrid.Visibility = Visibility.Hidden;
                    img = EditButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/edit.png")));
                    break;
                case Mode.Net:
                    parent.NetGrid.Visibility = Visibility.Hidden;
                    img = NetButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/net.png")));
                    break;
                case Mode.ECS:
                    break;
                case Mode.Gamepad:
                    break;
                case Mode.Test:
                    parent.TestGrid.Visibility = Visibility.Hidden;
                    //img = RecognitionTestButton.Content as System.Windows.Controls.Image;
                    //img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/test.png")));
                    break;
            }
        }

        private void updateVisualMode_Click(object sender, RoutedEventArgs e)
        {
            resetViewMode();

            Button btn = (Button)sender;

            ViewButton.Background = null;
            EditButton.Background = null;
            NetButton.Background = null;
            RecognitionTestButton.Background = null;

            System.Windows.Controls.Image img = null;
            parent.ViewGrid.Visibility = Visibility.Hidden;
            parent.EditGrid.Visibility = Visibility.Hidden;
            parent.EditGridAbel.Visibility = Visibility.Hidden;
            parent.EditGrid.Visibility = Visibility.Hidden;
            parent.NetGrid.Visibility = Visibility.Hidden;
            parent.TestGrid.Visibility = Visibility.Hidden;


            switch (btn.Uid)
            {
                case "View":
                    parent.ViewGrid.Visibility = Visibility.Visible;
                    parent.visualMode = Mode.View;
                    ViewButton.Background = Brushes.Gray;
                    img = ViewButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/viewPressed.png")));

                    break;
                case "Edit":
                    if(parent.tRobot == TypeRobot.FACE)
                        parent.EditGrid.Visibility = Visibility.Visible;
                    else
                        parent.EditGridAbel.Visibility = Visibility.Visible;
                    parent.visualMode = Mode.Edit;
                    parent.UpdateSliders(RobotControl.CurrentMotorState);
                    EditButton.Background = Brushes.Gray;
                    img = EditButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/editPressed.png")));

                    //parent.NewButton.IsEnabled = true;
                    //parent.LoadButton.IsEnabled = true;
                    break;
                case "Net":
                    parent.NetGrid.Visibility = Visibility.Visible;
                    parent.visualMode = Mode.Net;

                    NetButton.Background = Brushes.Gray;

                    img = NetButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/netPressed.png")));
                    break;
                case "Test":
                    parent.TestGrid.Visibility = Visibility.Visible;
                    parent.visualMode = Mode.Test;

                    RecognitionTestButton.Background = Brushes.Gray;


                    img = RecognitionTestButton.Content as System.Windows.Controls.Image;
                    img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/testPressed.png")));

                    break;
            }
        }

        #region ecs
        private void ECSButton_Click(object sender, RoutedEventArgs e)
        {

            if (ecsWin == null)
            {
                ecsWin = new ECSWin();
                ecsWin.Closed += new EventHandler(ecsWin_Closed);
                ecsWin.ecscontroller.NewECSEventHandler += new RoutedEventHandler(ECS_MouseClickOnCanvas);

                foreach (RobotMotion motion in parent.ecsMotions)
                {
                    ecsWin.ecscontroller.LoadECSPoint(motion.Name, new Point(motion.ECSCoord.Pleasure, motion.ECSCoord.Arousal));
                }

                ecsWin.Show();

                ecsWin.ecsController.SetECS(ecsWin.ecsController.CurrentECS);


            }
            else
                ecsWin.Activate();
        }

        private void ECS_MouseClickOnCanvas(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LeftMenu.NewEcsEvent,((ECSController)sender).CurrentECS));
        }

        private void ecsWin_Closed(object sender, EventArgs e)
        {
            ecsWin = null;
        }
        #endregion


        #region lookAt
        private void LookAtButton_Click(object sender, RoutedEventArgs e)
        {

            if (lookAtWin == null)
            {
                lookAtWin = new LookAtWin();
                lookAtWin.Closed += new EventHandler(lookAtWin_Closed);
                lookAtWin.LookAtEventHandler += new RoutedEventHandler(LookAt_MouseClickOnCanvas);

                lookAtWin.Show();
                lookAtWin.LoadLookAtPoint("Lorenzo", new Point(0.8, 0.48));
                lookAtWin.LoadLookAtPoint("PC", new Point(0.48, 0.88));
                lookAtWin.LoadLookAtPoint("Platea", new Point(0.06, 0.76));
                lookAtWin.LoadLookAtPoint("Ride", new Point(0.3, 0));



            }
            else
                lookAtWin.Activate();
        }

        private void LookAt_MouseClickOnCanvas(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LeftMenu.NewLookAtEvent, ((LookAtWin)sender).CurrentLookAt));
        }

        private void lookAtWin_Closed(object sender, EventArgs e)
        {
            lookAtWin = null;
        }

        #endregion

        #region gamepad
        private GamepadConfig gamepadConfig = null;
        private void GamepadButton_Click(object sender, RoutedEventArgs e)
        {

            if (gamepadConfig == null)
            {
                gamepadConfig = new GamepadConfig(parent);
                gamepadConfig.Closed += new EventHandler(gamepadWin_Closed);
                gamepadConfig.Show();
            }
            else
                gamepadConfig.Activate();
        }

        private void gamepadWin_Closed(object sender, EventArgs e)
        {
            //System.Windows.Controls.Image img = GamepadButton.Content as System.Windows.Controls.Image;
            //img.Source = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Images/Buttons/FaceGamepad.png")));
            gamepadConfig = null;
        }

        #endregion

        //private void ConfigButton_Click(object sender, RoutedEventArgs e)
        //{
        //    FACEConfigWin fc = new FACEConfigWin();
        //    fc.ShowDialog();

        //    RobotControl.LoadConfigFile(config);
        //    currentSmState = RobotControl.CurrentMotorState;

        //    //InitEditMode(RightSlidersPanel);
        //    //InitEditMode(LeftSlidersPanel);
        //}

        #region Status monitor

        //private Thread statusMonitor;
        //private StatusMonitor.MainWindow statusWin;

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {

            //statusWin = new StatusMonitor.MainWindow();
            //statusWin.Show();

        }

        #endregion

        /// <summary>
        /// Close window event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cancelEventArgs"></param>
        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if(gamepadConfig!=null)
                gamepadConfig.Close();

            if(ecsWin!=null)
                ecsWin.Close();

            if (lookAtWin != null)
                lookAtWin.Close();

        }

    }
}