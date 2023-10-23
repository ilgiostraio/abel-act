using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Act.Lib.ServoController
{
    [Serializable()]

    public abstract class Command
    {
        /// <summary>
        /// Servo motor name
        /// </summary>
        private string servoName;
        public string Name
        {
            get { return servoName; }
            set { servoName = value; }
        }

        /// <summary>
        /// Channel number in decimal, 0-31
        /// </summary>
        private int ch;
        public int Channel
        {
            get { return ch; }
            set { ch = value; }
        }

        /// <summary>
        /// Pulse width in microseconds
        /// </summary>
        private int pw;
        public int PulseWidth
        {
            get { return pw; }
            set { pw = value; }
        }

        private string sc;
        public string SerialSC
        {
            get { return sc; }
            set { sc = value; }
        }

        private int pt;
        public int PortSC
        {
            get { return pt; }
            set { pt = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Command() { }

        /// <summary>
        /// Initializes a new instance of the ServoMove class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="channel"></param>
        /// <param name="pulse"></param>
        /// <param name="serial"></param>

        public Command(string name, string serial,int port, int channel, int pulse)
        {
            servoName = name;
            ch = channel;
            pw = pulse;
            sc = serial;
            pt = port;
        }

        public abstract void SetPosition();

        public abstract void SetPosition(int? time);

    }
}
