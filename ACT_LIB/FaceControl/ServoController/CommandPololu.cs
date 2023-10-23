using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.IO;
using Pololu.UsbWrapper;
using Pololu.Usc;
using Act.Lib.Robot;

namespace Act.Lib.ServoController
{
    [Serializable]
    public class CommandPololu : Command
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandPololu() { }

        /// <summary>
        /// Initializes a new instance of the CommandPololu class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="channel"></param>
        /// <param name="pulse"></param>
        public CommandPololu(string name, string serial, int port, int channel, int pulse) : base(name, serial, port, channel, pulse) { }

        
        private static List<DeviceListItem> servoControllers = Usc.getConnectedDevices();

        public static List<string> ListServoControllers = servoControllers.Select(a => a.serialNumber).ToList();

        /// <summary>
        /// Format the command to send on serial port
        /// </summary>
        /// <returns></returns>
        public override void SetPosition()
        {

            Usc usc = null;
            try
            {
                if (String.IsNullOrEmpty(SerialSC))
                {
                    Debug.WriteLine("Error Config: SerialSC is empty -> #" + Name + " position:" + (ushort)(PulseWidth / (0.25)) + " servo:" + Channel + " pololu:" + SerialSC + " port:" + PortSC + " time:" + DateTime.Now.ToString("ss.fff"));

                    return;
                }

                var device = servoControllers.Find(a => a.serialNumber == this.SerialSC);

                if (device == null)
                    throw new Exception("Not Found serial of Pololu " + SerialSC);

                usc = new Usc(device);

                if(usc==null)
                    throw new Exception("Not Connected to Pololu " + device.serialNumber);

                ServoStatus[] servos;
                usc.getVariables(out servos);

               

                int position = servos[this.PortSC].position;
                Debug.WriteLine(PulseWidth);

                int diffPos = Math.Abs(position - (int)(PulseWidth / 0.25));
                if (diffPos != 0)
                    usc.setTarget((byte)PortSC, (ushort)(PulseWidth / (0.25)));
                
                
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Connected to #" + Name + " position:" + (ushort)(PulseWidth / (0.25)) + " servo:" + Channel + " pololu:" + SerialSC + " port:" + PortSC + " time:" + DateTime.Now.ToString("ss.fff"));

                throw new RobotException("Send Command Pololu Error:" + ex.Message);
            }
            finally 
            {
                if (usc!=null)
                {
                    usc.Dispose();
                    usc = null;
                }
            }


        }

        /// <summary>
        /// Send a raw value ([500,2500]) to the servo motor.
        /// </summary>
        /// <param name="ch">Number of the channel</param>
        /// <param name="port">POrt Servo Controller</param>
        /// <param name="rawVal">Amount of the movement in the range [500,2500] us</param>
        /// <param name="serial">Serial Servo Controller</param>
        public override void SetPosition(int? time) // Used only by FACEConfig: without animator
        {

            Usc usc = null;
            try
            {
                var device = servoControllers.Find(a => a.serialNumber == this.SerialSC);

                if (device == null)
                    throw new Exception("Not Found serial of Pololu " + SerialSC);


                usc = new Usc(device);
                ServoStatus[] servos;
                usc.getVariables(out servos);

                int position = servos[PortSC].position;

                int diffPos = Math.Abs(position - (int)(PulseWidth / 0.25));
                int speed = 0;
                if ((diffPos) * 10 >= time)
                {
                    speed = (int)((diffPos * 10) / time);

                }

                usc.setSpeed((byte)PortSC, (ushort)speed);
                usc.setTarget((byte)PortSC, (ushort)(PulseWidth / (0.25))); //the pulse width to transmit in units of quarter-microseconds 
                usc.Dispose();
                usc = null;
                   

              


            }
            catch
            {
                throw new RobotException("Command not sent. Some errors occured.");
            }
            finally { usc.Dispose(); }

            //else
            //{
            //    throw new FACException("There are not opened ports.");
            //}
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: #{1} P{2} \n", Name, Channel, PulseWidth);
            return sb.ToString();
        }

    }
}