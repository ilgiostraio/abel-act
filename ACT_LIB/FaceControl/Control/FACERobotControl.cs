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

namespace Act.Lib.Control
{


    public class FACERobotControl
    {
        public const int NUMBER_OF_MOTORS = 32;


        public FACERobotControl() 
        {
            RobotControl.NumberOfMotors = NUMBER_OF_MOTORS;
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

            RobotControl.TypeRobot = TypeRobot.FACE;
        }


        public enum MotorsNames
        {
            Jaw = 0, 
            FrownLeft = 1, 
            Empty = 2, 
            EELeft = 3, 
            SmileLeft = 4, 
            LipUpperCenter = 5, 
            BrowOuterLeft = 6, 
            BrowInnerLeft = 7,
            SquintLeft = 8, 
            SneerLeft = 9, 
            LipUpperLeft = 10, 
            EyeTurnLeft = 11, 
            LipLowerLeft = 12, 
            EyesUpDown = 13, 
            UpperNod = 14,
            LowerNod = 15, 
            LipLowerCenter = 16, 
            SneerRight = 17, 
            EERight = 18, 
            FrownRight = 19, 
            SmileRight = 20, 
            EyeLidsLower = 21,
            EyeLidsUpper = 22, 
            BrowOuterRight = 23, 
            BrowInnerRight = 24, 
            BrowCenter = 25, 
            LipLowerRight = 26, 
            LipUpperRight = 27,
            Turn = 28, 
            SquintRight = 29, 
            EyeTurnRight = 30, 
            Tilt = 31
        };

        public enum FaceControls
        {

            Jaw = 0, 
            FrownLeft = 1, 
            Empty = 2, 
            EELeft = 3, 
            SmileLeft = 4, 
            LipUpperCenter = 5, 
            BrowOuterLeft = 6, 
            BrowInnerLeft = 7,
            SquintLeft = 8, 
            SneerLeft = 9, 
            LipUpperLeft = 10, 
            LipLowerLeft = 12, 
            EyesUpDown = 13, 
            LipLowerCenter = 16, 
            SneerRight = 17, 
            EERight = 18,
            FrownRight = 19, 
            SmileRight = 20, 
            BrowOuterRight = 23, 
            BrowInnerRight = 24, 
            BrowCenter = 25, 
            LipLowerRight = 26,
            LipUpperRight = 27, 
            SquintRight = 29
        };

        public enum NeckControls
        {
            UpperNod = 14, 
            LowerNod = 15, 
            Turn = 28, 
            Tilt = 31
        };

        public enum EyesControls
        {
            EyeTurnLeft = 11, 
            EyeLidsLower = 21, 
            EyeLidsUpper = 22, 
            EyeTurnRight = 30
        };

        public enum FaceTrackingControls 
        { 
            EyeTurnLeft = 11, 
            EyesUpDown = 13, 
            UpperNod = 14, 
            Turn = 28, 
            EyeTurnRight = 30, 
            Tilt = 31 
        };

        public enum BlinkingControls 
        { 
            EyeLidsLower = 21, 
            EyeLidsUpper = 22 
        };




    }
    
}