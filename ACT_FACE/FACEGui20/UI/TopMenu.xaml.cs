using System;
using System.Collections.Generic;
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
    public partial class TopMenu: UserControl
    {
        FACEGui20Win parent;

        public static readonly RoutedEvent RobotChangedEvent =
          EventManager.RegisterRoutedEvent("RobotChangedEvent",
              RoutingStrategy.Bubble,
              typeof(RoutedEventHandler),
              typeof(TopMenu));


        //definisco un delegate che si occupare di definire 
        //i metodi da invocare al verificare di un determinato evento  (esposto nel xaml)
        public event RoutedEventHandler RobotSelectionChangedEvent
        {
            add { AddHandler(RobotChangedEvent, value); }
            remove { RemoveHandler(RobotChangedEvent, value); }
        }

        public TopMenu()
        {
            InitializeComponent();
           
           
        }

       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            parent = (FACEGui20Win) Window.GetWindow(this);

            if (parent == null)
                throw new Exception("TopMenu UserControls should be inserted in Window before being loaded");


        }

        private void robot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (System.Windows.Application.Current.MainWindow.IsInitialized == false)
                return;
            
            RaiseEvent(new RoutedEventArgs(TopMenu.RobotChangedEvent, ((ComboBox)sender)));


        }


    }
}