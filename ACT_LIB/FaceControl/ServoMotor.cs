using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Act.Lib.ServoController;

namespace Act.Lib
{
    [Serializable()]
    public class ServoMotor: ICloneable
    {
        [XmlIgnoreAttribute()]
        internal const int MIN_TIME_VALUE = 0; // Min Time in mS for the entire move
        [XmlIgnoreAttribute()]
        internal const int MAX_TIME_VALUE = 65535; // Max Time in mS for the entire move

       

        private string serialSC;
        private int pt;
        private string servoName;
        private int ch;
        private float pwNormalized;

        private float minValue;
        private float maxValue;

        #region
        /// <summary>
        /// Serial servo controller
        /// </summary>
        public string SerialSC
        {
            get { return serialSC; }
            set { serialSC = value; }
        }

        /// <summary>
        /// Port number in decimal of servo controllore, 0-n
        /// </summary>
        public int PortSC
        {
            get { return pt; }
            set { pt = value; }
        }
        /// <summary>
        /// Servo motor name
        /// </summary>
        public string Name
        {
            get { return servoName; }
            set { servoName = value; }
        }

        /// <summary>
        /// Channel number in decimal, 0-31
        /// </summary>
        public int Channel
        {
            get { return ch; }
            set { ch = value; }
        }

        /// <summary>
        /// Pulse width in microseconds
        /// </summary>
        public float PulseWidthNormalized
        {
            get { return pwNormalized; }
            set {

                if((value>=0 && value<=1) || value==-1.0)
                    pwNormalized = value;
                else
                    throw new ArgumentOutOfRangeException(value.ToString(), "Error PulseWidth of ServoMotor " + this.Name); 
            }
        }
        [XmlIgnoreAttribute()]
        public int PulseWidth
        {
            get { return MappingOnMinMaxInterval(pwNormalized); }
           
        }

        /// <summary>
        /// Minimum value of the servo motor between [0,1]
        /// </summary>
        public float MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        /// <summary>
        /// Maximum value of the servo motor between [0,1]
        /// </summary>
        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>

        public ServoMotor() { }


        /// <summary>
        /// Initializes a new instance of the ServoMove class.
        /// </summary>
        /// <param name="name"> Servo motor name. </param>
        /// <param name="channel"> Channel number in decimal, 0-31. </param>
        /// <param name="pulse"> Pulse width in microseconds. </param>
        /// <param name="min"> Minimum value of the servo motor between [0,1]. </param>
        /// <param name="max"> Maximum value of the servo motor between [0,1]. </param>
        /// <param name="serial">Serial servo controller</param>
        /// <param name="port">Port Servo Controller connectio servo motor</param>
        public ServoMotor(string name, int channel, float pulse, float min, float max, string serial, int port)
        {
            servoName = name;
            ch = channel;
            pwNormalized = pulse;
            minValue = min;
            maxValue = max;
            serialSC = serial;
            pt = port;
        }


        /// <summary>
        /// Initializes a new instance of the ServoMove class.
        /// </summary>
        /// <param name="channel"> Channel number in decimal, 0-31. </param>
        /// <param name="pulse"> Pulse width in microseconds. </param>
        /// <param name="serial">Serial servo controller</param>
        public ServoMotor(int channel, float pulse,string serial,int port)
        {
            ch = channel;
            pwNormalized = pulse;
            serialSC = serial;
            pt = port;
        }

        public object Clone()
        {
            return new ServoMotor
            {
                servoName =this.servoName,
                ch = this.ch,
                pwNormalized = this.pwNormalized,
                minValue = this.minValue,
                maxValue = this.maxValue,
                serialSC = this.serialSC,
                pt = this.pt,
            };
        }
        #endregion


        #region Generate Command  




        /// <summary>
        /// Generate a command for the current servo motor using the normalized value.
        /// </summary>
        /// <returns>A stream of data containing the formatted command.</returns>


        public void SendPosition(int? time)
        {
            CommandPololu c = new CommandPololu(Name, SerialSC, PortSC, Channel, MappingOnMinMaxInterval());

            if (time == null)
                c.SetPosition();
            else
                c.SetPosition(time);
        }


        /// <summary>
        /// Send a raw value ([500,2500]) to the servo motor.
        /// </summary>
        /// <param name="ch">Number of the channel</param>
        /// <param name="port">POrt Servo Controller</param>
        /// <param name="rawVal">Amount of the movement in the range [500,2500] us</param>
        /// <param name="serial">Serial Servo Controller</param>


        #endregion


        #region Conversions

        /// <summary>
        /// Convert the pulse width from microseconds [500, 2500] to the range [0,1]. 
        /// </summary>
        /// <returns>The converted value</returns>

        public float MappingOnUnitaryInterval(int val)  //Used in defaultConfig 
        {
            // 0.5 : x = (max-min)/2 : (val-min) ==> x = (val - min) / (max - min)
            float res = (val - minValue) / (maxValue - minValue);
            return res;
        }




        /// <summary>
        /// Convert the pulse width from the range [0,1] to microseconds [500, 2500] 
        /// according to updated Min and Max values.
        /// </summary>
        /// <returns>The converted value</returns>
     
        public int MappingOnMinMaxInterval(float val) //to send command on serial port [500-2500]
        {
            int res = (int)((((maxValue - minValue) * val)) + minValue);
            return res;
        }

        /// <summary>
        /// Convert the pulse width from the range [0,1] to microseconds [500, 2500] 
        /// according to updated Min and Max values.
        /// </summary>
        /// <returns>The converted value</returns>
        public int MappingOnMinMaxInterval() //to send command on serial port [500-2500]
        {
            int res = (int)((((maxValue - minValue) * pwNormalized)) + minValue);
            return res;
        }

        #endregion


        #region Utils
 
        internal void UpdateRawValue(int val)
        {
            pwNormalized = MappingOnUnitaryInterval(val);
        }

        #endregion

    }
}