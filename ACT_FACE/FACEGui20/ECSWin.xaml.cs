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

using System.Threading;

using Act.Lib.ControllersLibrary;
using YarpManagerCS;
using Newtonsoft.Json;

namespace Act.Face.FACEGui20
{
    /// <summary>
    /// Interaction logic for ECSWin.xaml
    /// </summary>
    public partial class ECSWin : Window
    {

        private YarpPort yarpPortRecever;
        string receivedECS = "";
        public ECSController ecscontroller
        {
            get { return ecsController; }
        }

        double speed = 0.01;
        CancellationTokenSource cts = new CancellationTokenSource();


      

        public ECSWin()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(InitYarp);

        }

        private void InitYarp(object s)
        {

            yarpPortRecever = new YarpPort();
            yarpPortRecever.openReceiver("/GamepadController/Axis/Right:o", "/ECSWin/Axis/Right:i");



            ThreadPool.QueueUserWorkItem(tok =>
            {
                CancellationToken token = (CancellationToken)tok;
                while (!token.IsCancellationRequested)
                {
                    ReceiveECS();
                }
            }, cts.Token);


        }



        void ReceiveECS()
        {



            yarpPortRecever.receivedData(out receivedECS);
            if (receivedECS != null && receivedECS != "")
            {
                //Debug.WriteLine(receivedSetMotors);
                try
                {
                    receivedECS = receivedECS.Replace(@"\", "").Replace("\"", "");
                    string x = receivedECS.Split(',')[0];
                    Point p = new Point();
                    p.X = Convert.ToDouble(receivedECS.Split(',')[0]);
                    p.Y = Convert.ToDouble(receivedECS.Split(',')[1]);// JsonConvert.DeserializeObject<Point>(receivedECS);
                    Point current = ecscontroller.CurrentECS;

                    if (p.X > 20000 && p.Y > 20000)
                    {
                        current.X += speed;
                        current.Y += speed;
                    }
                    else if (p.X < -20000 && p.Y > 20000)
                    {
                        current.X -= speed;
                        current.Y += speed;
                    }
                    else if (p.X < -20000 && p.Y < -20000)
                    {
                        current.X -= speed;
                        current.Y -= speed;
                    }
                    else if (p.X > 20000 && p.Y < -20000)
                    {
                        current.X += speed;
                        current.Y -= speed;
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

                        current.Y += speed;
                    }
                    else if (p.Y < -25000)
                    {

                        current.Y -= speed;
                    }


                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                               new Action(delegate ()
                               {
                                   ecscontroller.SetECS(current);

                               }));





                }
                catch (Exception exc)
                {
                    Debug.WriteLine("Error XML Set motors: " + exc.Message);
                }


            }

        }

     

        private void Window_Closed(object sender, EventArgs e)
        {
            cts.Cancel();

            if (yarpPortRecever.PortExists("/GamepadController/Axis/Right:o"))
                yarpPortRecever.Disconect("/ECSWin/Axis/Right:i", "/GamepadController/Axis/Right:o");

            if (yarpPortRecever != null)
                yarpPortRecever.Close();
        }
    }
}
