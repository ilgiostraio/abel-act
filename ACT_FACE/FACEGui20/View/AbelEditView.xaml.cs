using Act.Lib.ControllersLibrary;
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

namespace Act.Face.FACEGui20.View
{
    public partial class AbelEditView : UserControl
    {
        FACEGui20Win parent;
        public AbelEditView()
        {
            InitializeComponent();
           
           
        }

        private void SliderController_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //parent.EditServoMotor(sender);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            parent = (FACEGui20Win) Window.GetWindow(this);

            if (parent == null)
                throw new Exception("AbelEditView UserControls should be inserted in Window before being loaded");

           

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible) { 
                parent.InitEditMode(RightSlidersPanel);
                parent.InitEditMode(LeftSlidersPanel);
            }
        }
    }
}