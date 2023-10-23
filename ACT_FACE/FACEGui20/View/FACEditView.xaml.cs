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
    public partial class FACEditView : UserControl
    {
        FACEGui20Win parent;
        public FACEditView()
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
                throw new Exception("FACEditView UserControls should be inserted in Window before being loaded");

            parent.InitEditMode(RightSlidersPanel);
            parent.InitEditMode(LeftSlidersPanel);

        }
    }
}