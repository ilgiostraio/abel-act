using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Act.Lib
{
    public enum TypeRobot { FACE ,  Abel };

    public enum StdType { /*MiniSSC = 0,*/ SSC32 };

    

    /// <summary>
    /// View: send expression
    /// Edit: create new expression
    /// Config: set max/min --> OLD
    /// Net: UDP or YARP connection
    /// ECS: ECS model
    /// Gamepad: control robot with joystick
    /// </summary>
    public enum Mode { View, Edit, Net, ECS, Gamepad, Test };

    //public enum FACEActions
    //{
    //    None = -1, Neutral = 0, Happiness = 1, Anger = 2, Fear = 3, Sadness = 4, Disgust = 5, Amazement = 6, Reset = 7,
    //    StartBlinking = 8, StopBlinking = 9, StartFaceTracking = 10, StopFaceTracking = 11, Yes = 12, No = 13, Afraid = 14, Surprise = 15
    //};
    public enum FACEActions
    {
        None = -1, Anger = 1, Disgust = 2, Fear = 3, Happiness = 4, Neutral = 5, Sadness = 6, Surprise = 7, Reset = 8
    };

    //public enum Expressions
    //{
    //    Neutral = 32, Anger, Disgust, Fear, Happiness, Sadness, Surprise
    //};

    //public enum BrainModules { FaceTrack = 0, Blink = 1 }


    public static class Enums
    {
        private static Dictionary<string, string> standardType;


        public static void InitializeStdMap()
        {
            standardType = new Dictionary<string, string>();

            Array names = Enum.GetNames(typeof(StdType));

            foreach (string enumVal in names)
                standardType.Add(enumVal, enumVal);
        }

        public static string[] getStdType()
        {
            InitializeStdMap();

            List<string> types = new List<string>();
            foreach (var pair in standardType)
            {
                types.Add(pair.Value);
            }

            return types.ToArray();
        }

    }
}