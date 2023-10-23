using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Act.Control.GamepadController
{
    public class Gamepad
    {

        public string DPadUp { get; set; }
        public string DPadDown { get; set; }
        public string DPadLeft { get; set; }
        public string DPadRight { get; set; }
        public string Start { get; set; }
        public string Back { get; set; }
        public string LeftThumb { get; set; }
        public string RightThumb { get; set; }
        public string LeftShoulder { get; set; }
        public string RightShoulder { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string X { get; set; }
        public string Y { get; set; }

        public int LeftTrigger { get; set; }
        public int RightTrigger { get; set; }

    }
}
