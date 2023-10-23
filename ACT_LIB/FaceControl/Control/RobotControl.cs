using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Act.Lib.Animator;
using Pololu.Usc;
using Pololu.UsbWrapper;
using System.Diagnostics;
using Act.Lib.Robot;
using Act.Lib;

namespace Act.Lib.Control
{
    public static class RobotControl
    {

        /// <summary>
        /// Minimum value of the servo motor.
        /// </summary>
        private static int numberOfMotors;
        public static int NumberOfMotors
        {
            get { return numberOfMotors; }
            set { numberOfMotors = value; }
        }
        
        /// 
        /// </summary>
        private static TypeRobot typeRobot;
        public static TypeRobot TypeRobot
        {
            get { return typeRobot; }
            set { typeRobot = value; }
        }


        #region Members
        /// <summary>
        /// Minimum value of the servo motor.
        /// </summary>
        private static int minLimit;
        public static int MinLimit
        {
            get { return minLimit; }
            set { minLimit = value; }
        }

        /// <summary>
        /// Maximum value of the servo motor.
        /// </summary>
        private static int maxLimit;
        public static int MaxLimit
        {
            get { return maxLimit; }
            set { maxLimit = value; }
        }

        private static TimeSpan motorInterval;
        public static TimeSpan MotorInterval
        {
            get { return motorInterval; }
            set { motorInterval = value; }
        }

        private static List<ServoMotor> currentMotorState;
        public static List<ServoMotor> CurrentMotorState
        {
            get { return currentMotorState; }
        }

        private static List<ServoMotor> defaultMotorState;
        public static List<ServoMotor> DefaultMotorState
        {
            get { return defaultMotorState; }
        }

        private static int eyeState;//= -1; //0=open, 1=close
        public static int EyeState
        {
            get { return eyeState; }
            set { eyeState = value; }
        }

        #endregion

        #region Constants from interface

        private static float kEyesTurn;
        public static float KEyesTurn
        {
            get { return kEyesTurn; }
            set { kEyesTurn = value; }
        }

        private static int blinkingRate; // blink/sec
        public static int BlinkingRate
        {
            get { return blinkingRate; }
            set { blinkingRate = value; }
        }

        private static int closedEyesTime = 100; // milliseconds
        public static int ClosedEyesTime
        {
            get { return closedEyesTime; }
            set { closedEyesTime = value; }
        }

        private static int speedEyesTime;// milliseconds
        public static int SpeedEyesTime
        {
            get { return speedEyesTime; }
            set { speedEyesTime = value; }
        }


        private static int respirationRate;// = 8; // blink/sec
        public static int RespirationRate
        {
            get { return respirationRate; }
            set { respirationRate = value; }
        }

        private static int respirationSpeed = 3500; // milliseconds
        public static int RespirationSpeed
        {
            get { return respirationSpeed; }
            set { respirationSpeed = value; }
        }



        #endregion

        #region Perception

        private static Percept perception;

        internal static Percept GetPerception()
        {
            return perception;
        }

        internal static void ResetBody()
        {
            perception = null;
        }

        #endregion

        #region Execute Movement

        /// <summary>
        /// Execute a movement saved in a xml file.
        /// </summary>
        /// <param name="filename">The name of the xml file to be loaded</param>
        /// <param name="priority">Priority of the movement</param>
        public static void ExecuteFile(string filename, int priority)
        {
            RobotMotion motion = RobotMotion.LoadFromXmlFormat(filename);
            //motion.Duration = 1500;  // DA CANCELLARE
            DateTime when = DateTime.Now.Add(TimeSpan.FromMilliseconds(motion.DelayTime));
            MotorTask m = new MotorTask(MotorTaskType.Generic, motion, when, 1, 0);
            AnimatorEngine.AddTask(m, m.Start);
        }

        /// <summary>
        /// Execute a movement.
        /// </summary>
        /// <param name="smg">Motor configuration to be executed</param>
        /// <param name="priority">Priority of the movement</param>
        /// <param name="when">Start time of the movement</param>
        public static void ExecuteMotion(RobotMotion motion)
        {
            DateTime when = DateTime.Now.Add(TimeSpan.FromMilliseconds(motion.DelayTime));
            MotorTask m = new MotorTask(MotorTaskType.Generic, motion, when, 1, 0);
            AnimatorEngine.AddTask(m, when);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        public static void ExecuteAnimation(RobotAnimation animation)
        {
            List<RobotMotion> motions = animation.FACEMotionsList;

            foreach (RobotMotion m in motions)
            {
                DateTime when = DateTime.Now.Add(TimeSpan.FromMilliseconds(m.DelayTime));
                MotorTask task = new MotorTask(MotorTaskType.Generic, m, when, animation.RepeatTimes, animation.RepeatFrequency);
                AnimatorEngine.AddTask(task, task.Start);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        public static void ExecuteAnimation(RobotAnimation animation, MotorTaskType motorType)
        {
            DateTime when = DateTime.Now;
            MotorTask task;

            switch (motorType)
            {
                case MotorTaskType.Blinking:

                    when = DateTime.Now;

                    //Chiusura
                    animation.FACEMotionsList[0].Duration = speedEyesTime; //velocità di chiusura
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[0].DelayTime));
                    task = new MotorTask(motorType, animation.FACEMotionsList[0], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);

                    //Apertura
                    animation.FACEMotionsList[1].Duration = speedEyesTime; //velocità di apertura
                    animation.FACEMotionsList[1].DelayTime = closedEyesTime;
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[1].DelayTime + animation.FACEMotionsList[0].Duration));
                    task = new MotorTask(motorType, animation.FACEMotionsList[1], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);

                    //DateTime when = DateTime.Now;
                    //animation.FACEMotionsList[0].Duration = speedEyesTime; //velocità di chiusura
                    //animation.FACEMotionsList[1].Duration = speedEyesTime; //velocità di apertura
                    //animation.FACEMotionsList[1].DelayTime = closedEyesTime;

                    //for (int i = 0; i < animation.FACEMotionsList.Count; i++)
                    //{
                    //    if (i > 0)
                    //    {
                    //        when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[i].DelayTime + animation.FACEMotionsList[i - 1].Duration));
                    //    }
                    //    else
                    //    {
                    //        when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[i].DelayTime));
                    //    }
                    //    MotorTask task = new MotorTask(motorType, animation.FACEMotionsList[i], when, animation.RepeatTimes, animation.RepeatFrequency);
                    //    AnimationEngine.AddTask(task, task.Start);
                    //}
                    break;

                case MotorTaskType.Respiration:

                    when = DateTime.Now;

                    //Chiusura
                    animation.FACEMotionsList[0].Duration = respirationSpeed; //velocità di chiusura
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[0].DelayTime));
                    task = new MotorTask(motorType, animation.FACEMotionsList[0], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);

                    //Apertura
                    animation.FACEMotionsList[1].Duration = respirationSpeed; //velocità di apertura
                    animation.FACEMotionsList[1].DelayTime = 0;
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[1].DelayTime + animation.FACEMotionsList[0].Duration));
                    task = new MotorTask(motorType, animation.FACEMotionsList[1], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);
                    break;

                case MotorTaskType.Yes:

                    when = DateTime.Now;

                    //Alza la testa
                    //animation.FACEMotionsList[0].Duration = 1000; //velocità di chiusura
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[0].DelayTime));
                    task = new MotorTask(motorType, animation.FACEMotionsList[0], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);

                    //Abbassa la testa
                    //animation.FACEMotionsList[1].Duration = 1000; //velocità di apertura
                    //animation.FACEMotionsList[1].DelayTime = 1000;
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[1].DelayTime + animation.FACEMotionsList[0].Duration));
                    task = new MotorTask(motorType, animation.FACEMotionsList[1], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);

                    //Rimette la testa a posto
                    //animation.FACEMotionsList[2].Duration = 1500; //velocità di apertura
                    //animation.FACEMotionsList[2].DelayTime = 2000;
                    when = DateTime.Now.Add(TimeSpan.FromMilliseconds(animation.FACEMotionsList[1].DelayTime + animation.FACEMotionsList[2].Duration));
                    task = new MotorTask(motorType, animation.FACEMotionsList[2], when, animation.RepeatTimes, animation.RepeatFrequency);
                    AnimatorEngine.AddTask(task, task.Start);
                    break;
            }
        }

        /// <summary>
        /// Execute a single motor movement.
        /// </summary>
        /// <param name="ch">Number of the channel</param>
        /// <param name="pulse">Amount of the movement between 0 and 1</param>
        /// <param name="time">Duration of the movement</param>
        /// <param name="priority">Priority of the movement</param>
        /// <param name="when">Start time of the movement</param>
        public static void ExecuteSingleMovement(int ch, float pulse, TimeSpan duration, int priority, TimeSpan delay)
        {
            RobotMotion motion = new RobotMotion(numberOfMotors);
            motion.Duration = (int)duration.TotalMilliseconds;
            motion.Priority = priority;
            motion.DelayTime = (int)delay.TotalMilliseconds;
            motion.ServoMotorsList[ch].PulseWidthNormalized = pulse;

            DateTime when = DateTime.Now.Add(TimeSpan.FromMilliseconds(motion.DelayTime));

            MotorTask m = new MotorTask(MotorTaskType.Single, motion, when, 0, 1);
            AnimatorEngine.AddTask(m, m.Start);
        }


      
        /// <summary>
        /// Send a raw value ([500,2500]) to the servo motor.
        /// </summary>
        /// <param name="ch">Number of the channel</param>
        /// <param name="port">POrt Servo Controller</param>
        /// <param name="rawVal">Amount of the movement in the range [500,2500] us</param>
        /// <param name="serial">Serial Servo Controller</param>
        public static void SendRawCommand(int ch, int port, int rawVal, string serial, int time) // Used only by FACEConfig: without animator
        {

            try
            {
                Usc usc = null;
                foreach (DeviceListItem d in Usc.getConnectedDevices())
                {
                    if (d.serialNumber == serial)
                    {
                        usc = new Usc(d);
                        ServoStatus[] servos;
                        usc.getVariables(out servos);
                        int position = servos[port].position;

                        int diffPos = Math.Abs(position - (int)(rawVal / 0.25));
                        int speed = 0;
                        if ((diffPos) * 10 >= time)
                        {
                            speed = (int)((diffPos * 10) / time);

                        }

                        usc.setSpeed((byte)port, (ushort)speed);
                        usc.setTarget((byte)port, (ushort)(rawVal / (0.25))); //the pulse width to transmit in units of quarter-microseconds 
                        Debug.WriteLine("Connected to #" + serial + ".");
                        usc.Dispose();
                        usc = null;
                    }

                }

                //If the command is sent correctly, the servo motor pulse is updated
                currentMotorState[ch].UpdateRawValue(rawVal); // CHECK FOR CHANGING


            }
            catch
            {
                throw new RobotException("Command not sent. Some errors occured.");
            }

            //else
            //{
            //    throw new FACException("There are not opened ports.");
            //}
        }
        #endregion


        #region Load/Save configurations


        public static void LoadServoMotor(List<ServoMotor> listServoMotor)
        {
            try
            {
                defaultMotorState = listServoMotor;
                currentMotorState = defaultMotorState;

            }
            catch
            {
                throw;
            }
        }

        public static List<ServoMotor> LoadConfigFile(string filename)
        {
            try
            {
                defaultMotorState = (RobotMotion.LoadFromXmlFormat(filename)).ServoMotorsList;
                currentMotorState = defaultMotorState;
                return currentMotorState;
            }
            catch
            {
                throw;
            }
        }

        public static void SaveConfigFile(object objGraph, string fileName)
        {
            try
            {
                RobotMotion.SaveAsXmlFormat(objGraph, fileName);
                defaultMotorState = objGraph as List<ServoMotor>;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Eyes Blinking

        /// <summary>
        /// Start blinking routine.
        /// </summary>
        public static void StartBlinking()
        {
            RobotAnimation animation = RobotAnimation.LoadFromXmlFormat("Animations\\" + typeRobot + "\\Blinking.xml");
            animation.RepeatFrequency = (int)((60 / RobotControl.BlinkingRate) * 1000);
            RobotControl.ExecuteAnimation(animation, MotorTaskType.Blinking);
        }

        /// <summary>
        /// Stop blinking routine.
        /// </summary>
        public static void StopBlinking()
        {
            AnimatorEngine.RemoveAllTasks(MotorTaskType.Blinking);
        }

        #endregion

        #region Respiration

        /// <summary>
        /// Start respiration routine.
        /// </summary>
        public static void StartRespiration()
        {
            RobotAnimation animation = RobotAnimation.LoadFromXmlFormat("Animations\\"+ typeRobot + "\\Respiration.xml");
            animation.RepeatFrequency = (int)((60 / RobotControl.RespirationRate) * 1000);
            RobotControl.ExecuteAnimation(animation, MotorTaskType.Respiration);
        }

        /// <summary>
        /// Stop respiration routine.
        /// </summary>
        public static void StopRespiration()
        {
            AnimatorEngine.RemoveAllTasks(MotorTaskType.Respiration);
        }

        #endregion

        #region Yes/No movement

        public static void StartYesMovement()
        {
            RobotAnimation animation = RobotAnimation.LoadFromXmlFormat("Animations\\" + typeRobot + "\\Yes.xml");
            RobotControl.ExecuteAnimation(animation, MotorTaskType.Yes);
        }

        public static void StopYesMovement()
        {
            AnimatorEngine.RemoveAllTasks(MotorTaskType.Yes);
        }

        public static void StartNoMovement()
        {
            RobotAnimation animation = RobotAnimation.LoadFromXmlFormat("Animations\\" + typeRobot + "\\No.xml");
            RobotControl.ExecuteAnimation(animation, MotorTaskType.No);
        }

        public static void StopNoMovement()
        {
            AnimatorEngine.RemoveAllTasks(MotorTaskType.No);
        }
        #endregion

    }
}
