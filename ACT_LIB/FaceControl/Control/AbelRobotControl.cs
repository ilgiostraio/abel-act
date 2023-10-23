using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;


using Act.Lib.Animator;
using Act.Lib;

namespace Act.Lib.Control
{


    public class AbelRobotControl
    {
        public const int NUMBER_OF_MOTORS = 22;


        public AbelRobotControl() 
        {
            RobotControl.NumberOfMotors = 22;
            RobotControl.MinLimit = 500;
            RobotControl.MaxLimit = 2500;
            RobotControl.MotorInterval = new TimeSpan(400000); // 4 milliseconds
            RobotControl.EyeState = -1; //0=open, 1=close

            RobotControl.KEyesTurn = 0.05f;
            RobotControl.BlinkingRate = 10; // blink/sec
            RobotControl.ClosedEyesTime = 100; // milliseconds
            RobotControl.SpeedEyesTime = 100; // milliseconds
            RobotControl.RespirationRate = 8;
            RobotControl.RespirationSpeed = 3500; // milliseconds

            RobotControl.TypeRobot = TypeRobot.Abel;
        /// <summary>




    }

    public enum MotorsNames
        {
            EyelidLowerLeft = 1,
            EyelidTopLeft = 2,
            EyelidLowerRight = 3,
            EyelidTopRight = 4,
            EyeUDLeft = 5,
            EyeUPRight = 6,
            EyeIOLeft = 7,

            EyeIORight = 8,
            SideBrowLeft = 9,
            MiddleBrowLeft = 10,
            SideBrowRight = 11,
            MiddleBrowRight = 12,
            Jaw = 13,
            MouthCornerUDLeft = 14,
            MouthCornerIOLeft = 15,
            MouthCornerUDRight = 16,
            MouthCornerIORight = 17,
            CheekLeft = 18,
            CheekRight = 19,
            LipTopLeft = 20,
            LipTopRight = 21,
            Chin = 22
            
        };

        public enum FaceControls
        {
            EyelidLowerLeft = 1,
            EyelidTopLeft = 2,
            EyelidLowerRight = 3,
            EyelidTopRight = 4,
            EyeUDLeft = 5,
            EyeUPRight = 6,
            EyeIOLeft = 7,

            EyeIORight = 8,
            SideBrowLeft = 9,
            MiddleBrowLeft = 10,
            SideBrowRight = 11,
            MiddleBrowRight = 12,
            Jaw = 13,
            MouthCornerUDLeft = 14,
            MouthCornerIOLeft = 15,
            MouthCornerUDRight = 16,
            MouthCornerIORight = 17,
            CheekLeft = 18,
            CheekRight = 19,
            LipTopLeft = 20,
            LipTopRight = 21,
            Chin = 22
        };

       

        public enum EyesControls
        {
            EyelidLowerLeft = 1,
            EyelidTopLeft = 2,
            EyelidLowerRight = 3,
            EyelidTopRight = 4,
            EyeUDLeft = 5,
            EyeUPRight = 6,
            EyeIOLeft = 7,

            EyeIORight = 8,
        };

        public enum FaceTrackingControls
        {
            EyeUDLeft = 5,
            EyeUPRight = 6,
            EyeIOLeft = 7,

            EyeIORight = 8
        };
        public enum BlinkingControls
        {
            EyelidLowerLeft = 1,
            EyelidTopLeft = 2,
            EyelidLowerRight = 3,
            EyelidTopRight = 4,
        };







    }
    
}