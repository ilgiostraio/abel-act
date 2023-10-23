using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Pololu.UsbWrapper;
using Pololu.Usc;
using Pololu.Usc.Bytecode;
using System.Threading.Tasks;

using Serilog;
using Serilog.Context;
using SerilogMetrics;


using Act.Lib.Animator;
using Act.Lib;
using Act.Lib.Control;
using Act.Lib.Robot;

namespace Act.Lib.Animator
{
  

    public struct MotorStep
    {
        public float val { get; set; }
        public int pr { get; set; }
        
    }

    public static class AnimatorEngine
    {
       // private static readonly ILogger _logger = Log.ForContext("SourceContext", "AnimatorEngine");

        // Create an AutoReset EventWaitHandle
        private static EventWaitHandle ewHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        public static EventWaitHandle EWHandle
        {
            get { return ewHandle; }
        }

        // 
        private static Dictionary<int, List<MotorStep>> motorSteps = new Dictionary<int, List<MotorStep>>();
        public static Dictionary<int, List<MotorStep>> MotorSteps
        {
            get { return motorSteps; }
            set { motorSteps = value; }
        }


        // Heap used to store tasks ordered by due time.
        private static AnimatorHeap tasksHeap = new AnimatorHeap();

        /// InterpolatorTask represents the task which interpolates all the tasks 
        /// into the heap of the AnimationEngine when it is being run.
        /// It is rescheduled into the heap when it finishes its task.
        private static InterpolatorTask interp;

        // Waiting time of the thread.
        private static TimeSpan waitTime;

        // When the timer is started, it is used to record time.
        private static DateTime start = new DateTime();

        // (32x9byte + tempo) x 8bit at 115.200 kbs richiedono 20.208 milliseconds quindi non si può inviare più velocmente di 1 ogni 25ms
        private static TimeSpan interpolatorInterval = new TimeSpan(30000); // 1 milliseconds 
        // ---> controllare: fino al 25/02/2013 era 100000, cambiato a 400000 come Interval Motor in FACEBody


        private static bool running = false;
        public static bool Running
        {
            get { return running; }
            set { running = value; }
        }

        
        public static void StartAnimation()
        {
            //_logger.Information("StartAnimator with interpolatorInterval of {interpolatorInterval} ms", interpolatorInterval.Milliseconds);

            start = DateTime.Now;
            interp = new InterpolatorTask(DateTime.Now, interpolatorInterval);

            AddTask(interp, DateTime.Now);
           //interp.DoAction(0);
            running = true;
            TimerTick();


        }

        public static void StopAnimation()
        {
            running = false;
            //_logger.Information("StopAnimation");
        }

        

        /// <summary>
        /// Schedule a task for execution at a given time from the start of the timeline.
        /// </summary>
        /// <param name="mTask"> The task to be executed after t time elapsed. </param>
        /// <param name="t"> The expiring absolute time of the task. </param>
        public static void AddTask(ITask mTask, DateTime t)
        {
            // mTask expires in (elem.Due) milliseconds
            HeapElement elem = new HeapElement();
            elem.Task = mTask;
            elem.Due = t - start;  // The expiring relative time of the task from the start of the timer
            Guid.NewGuid();

            TimeSpan topTime;

            try
            {
                Monitor.Enter(tasksHeap); //Fornisce un meccanismo che sincronizza l'accesso agli oggetti.
                tasksHeap.Insert(elem);
                topTime = tasksHeap.Top().Due;
            }
            finally
            {
               // _logger.Information("AddTask in tasksHeap " + elem.Task.GetType().Name, () => elem);
                Monitor.Exit(tasksHeap);
            }

            if (/*waitTime.Milliseconds != 0 &&*/ topTime <= elem.Due)
            {
                // restart thread
                waitTime = TimeSpan.FromMilliseconds(0); //<----------
                //ewh.Set();
                if(mTask.GetType().Name != "InterpolatorTask")
                    ewHandle.Set();
            }
            /*
            // if there is no timeout, calculate if and for how long time it is suspended
            if (waitTime.Milliseconds == 0)
            {
                //waitTime = tasksHeap.Top().Due - GetTime();
                waitTime = GetTime() - tasksHeap.Top().Due;
                //waitHandle.WaitOne(waitTime);
                //runAnimation.Join(waitTime);
                ewh.WaitOne(waitTime);
            }
            */


           
        }


      

        /// <summary>
        /// Stop the execution of a particular type of task by removing 
        /// all instances of it from the scheduler.
        /// </summary>
        /// <param name="tType"> The task type to be removed. </param>
        /// <returns></returns>
        /// 
        public static bool RemoveAllTasks(MotorTaskType tType)
        {
            //_logger.Information("RemoveAllTasks {0}", tType);
            try
            {
                List<ITask> toRemove = new List<ITask>();
                Monitor.Enter(tasksHeap);

                foreach (HeapElement elem in tasksHeap.Data)
                {
                    if (elem.Task == null)
                        toRemove.Add(elem.Task);
                    else
                    {
                        if (elem.Task.GetType() == typeof(MotorTask) && (elem.Task as MotorTask).Type == tType)
                        {
                            toRemove.Add(elem.Task);
                        }
                    }
                }
                foreach (ITask t in toRemove)
                {
                    tasksHeap.RemoveTask(t);
                }
            }
            catch(Exception ex)
            {
                Monitor.Exit(tasksHeap);

                //_logger.Error(ex, "RemoveAllTasks {0}",tType);

                return false;
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(tasksHeap);
            }
            return true;
        }
       


        public static void TimerTick()
        {

           // _logger.Information("Start TimerTick currTime:{0}", GetTime());

            while (running)
            {
                
                TimeSpan currTime = GetTime();// TimeSpan.FromMilliseconds(100);

                try
                {
                    Monitor.Enter(tasksHeap);

                    //if there are some expired tasks, the heap executes them
                    while (tasksHeap.Count() > 0 && currTime >= tasksHeap.Top().Due)
                    {
                        
                        var el = tasksHeap.Remove();

                        //visto che un loop infinto in caso di InterpolatorTask, per il momento non mi interessa questo log
                        if (el.Task.GetType().Name != "InterpolatorTask")
                        {
                            //_logger
                            //    .ForContext("TypeTask", el.Task.GetType().Name)
                            //    .Debug("Found a task", () => el);

                            //_logger.Debug("delay of {delay} ms in {method}", (currTime - el.Due).Milliseconds, "TimerTick");

                            el.Task.DoAction((currTime - el.Due).Milliseconds);   //.Milliseconds o .TotalMilliseconds ??????? 
                        }
                        else
                            el.Task.DoAction(0);

                       


                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    Monitor.Exit(tasksHeap);
                }


                // if there are other tasks (not expired)
                if (tasksHeap.Count() > 0)
                {



                    // se è uguale a 1 si tratta di InterpolatorTask, per il momento non mi interessa questo log
                    if (tasksHeap.Count() > 1)
                    {
                        try
                        {
                            Monitor.Enter(tasksHeap);
                            //_logger.Debug("There are other tasks", () => tasksHeap);
                        }
                        finally
                        {
                            // Ensure that the lock is released.
                            Monitor.Exit(tasksHeap);
                        }

                        /******* TO CONTROL THE CORRECTNESS *********/
                        // richiama la funzione dopo tot millisecondi = elemento più vicino da eseguire - currTime
                        //AnimeJTimerRunning = setTimeout('AnimeJTimerTick()', t.Heap.Top().Due - currt);

                        currTime = GetTime();
                        // it waits for a time 
                        if (tasksHeap.Top().Due >= currTime)
                        {
                            waitTime = tasksHeap.Top().Due - currTime; // questo lo messo io 


                            // se è uguale a 1 si tratta di InterpolatorTask, per il momento non mi interessa questo log
                            //if (tasksHeap.Count() > 1)
                                //_logger.Debug("TimerTick Wait {0}", waitTime);

                            ewHandle.WaitOne(waitTime);
                        }
                    }
                    else
                    {
                        //_logger.Debug("wait event");
                        ewHandle.WaitOne();
                    }
                    /******* TO CONTROL THE CORRECTNESS *********/
                }
                else
                {
                    ewHandle.WaitOne();
                }
            }
        }

        // Return the timer time, a number from start time to the current time.
        private static TimeSpan GetTime()
        {
            return DateTime.Now - start;
        }

        #region Send Commands

        //Used by animator (Interpolator task)
        //internal static void SendCommand(List<ServoMotor> motorsList)
        //{
        //    if (ComPorts.OpenedPort)
        //    {
        //        MemoryStream memStream = null;
        //        try
        //        {
        //            switch (ComPorts.DeviceType)
        //            {
        //                case StdType.SSC32:
        //                    StringBuilder sb = new StringBuilder();

        //                    //At this point each servo motor value must be != -1 (controlled in MotorTask).
        //                    //If some servo motor value equals -1, it means that the servo motor
        //                    //is not configured (those servo motors equal -1 also in the default configuration).
        //                    for (int i = 0; i < motorsList.Count; i++)
        //                    {
        //                        //Only motor values != -1 are sent on the serial port
        //                        if (motorsList.ElementAt(i).PulseWidth != -1)
        //                        {
        //                            memStream = motorsList.ElementAt(i).GenerateCommand();
        //                            sb.Append(new StreamReader(memStream).ReadToEnd());
        //                        }
        //                    }
        //                    sb.Append(String.Format("T{0} \r", (int)interpolatorInterval.TotalMilliseconds));
        //                    ComPorts.SelectedPort.WriteLine(sb.ToString());
        //                    System.Diagnostics.Debug.WriteLine(sb.ToString());

        //                    //If the command is sent correctly, FACEBody.CurrentState is updated
        //                    for (int j = 0; j < motorsList.Count; j++)
        //                    {
        //                        if (motorsList[j].PulseWidth != -1)
        //                            FACEBody.CurrentMotorState[j].PulseWidth = motorsList[j].PulseWidth;
        //                    }
        //                    ComPorts.SelectedPort.DiscardOutBuffer();
        //                    break;
        //            }
        //        }
        //        catch
        //        {
        //            throw new FACException("Command not sent. Some errors occured.");
        //        }
        //    }
        //    else
        //    {
        //        throw new FACException("There are not opened ports.");
        //    }
        //}

        //internal static void SendCommand(List<ServoMotor> motorsList)
        //{
        //    int port = 0;
        //    string serial = "";
        //    int rawVal = 0;
        //    DeviceListItem dl = null;

        //    int chanel = 100;
        //    Usc usc = null;
        //    try{

        //        //At this point each servo motor value must be != -1 (controlled in MotorTask).
        //        //If some servo motor value equals -1, it means that the servo motor
        //        //is not configured (those servo motors equal -1 also in the default configuration).

        //        List<ServoMotor> sm = motorsList.FindAll(a => a.PulseWidthNormalized != -1);
        //        sm.OrderBy(o => o.SerialSC);

        //        List<DeviceListItem> listPololu = Usc.getConnectedDevices();

        //        foreach (ServoMotor se in sm) 
        //        {
        //            port = se.PortSC;
        //            serial = se.SerialSC;
        //            rawVal = se.MappingOnMinMaxInterval();
        //            chanel = se.Channel;

        //            if (dl != null && dl.serialNumber != serial) // se sono già connesso e non devo usare più la pololu mi disconetto
        //            {
        //                usc.Dispose();
        //                usc = null;
        //                dl = null;
                       
        //            }

        //            if(dl==null)// se non sono connesso a nessuna pololu
        //            {
        //                dl = listPololu.Find(a => a.serialNumber == serial);

        //                if (dl == null && chanel != 18 && chanel != 3)//non vengono usati
        //                    throw new Exception("Not Found serial of Pololu " + serial);
        //                else if (chanel == 18 || chanel == 3)
        //                    continue;
        //                else
        //                    usc = new Usc(dl);
                      
        //            }



        //            ServoStatus[] servos;
        //            usc.getVariables(out servos);
        //            //if(chanel==14 ||chanel==28)
        //            //    usc.setSpeed((byte)port, 11);
        //            //else usc.setSpeed
        //            int position = servos[port].position;

        //            int diffPos = Math.Abs(position - (int)(rawVal / 0.25));
        //            if (diffPos != 0)
        //                usc.setTarget((byte)port, (ushort)(rawVal / (0.25)));

        //        }

        //        if (usc != null)
        //        {
        //            usc.Dispose();
        //            usc = null;
        //            dl = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new FACException("Command not sent. Some errors occured.");
        //        Debug.WriteLine("Connected to #" + serial + " position:" + (ushort)(rawVal / (0.25)) + " servo:" + chanel + " time:" + DateTime.Now.ToString("ss.fff"));

        //        throw new FACException("Send Command Pololu Error:" + ex.Message);

        //    }

        //        //for (int i = 0; i < motorsList.Count; i++)
        //        //{
        //        //    //Only motor values != -1 are sent on the serial port
        //        //    if (motorsList.ElementAt(i).PulseWidthNormalized != -1)
        //        //    {
        //        //        port = motorsList.ElementAt(i).PortSC;
        //        //        serial = motorsList.ElementAt(i).SerialSC;
        //        //        rawVal =motorsList.ElementAt(i).MappingOnMinMaxInterval();
        //        //        chanel = motorsList.ElementAt(i).Channel;
        //        //        foreach (DeviceListItem d in Usc.getConnectedDevices())
        //        //        {
        //        //            if (d.serialNumber == serial)
        //        //            {
        //        //                usc = new Usc(d);
        //        //                ServoStatus[] servos;
        //        //                usc.getVariables(out servos);
        //        //                int position = servos[port].position;

        //        //                int diffPos = Math.Abs(position - (int)(rawVal / 0.25));
        //        //                int speed = 0;
        //        //                //if ((diffPos) * 10 >= interpolatorInterval.Milliseconds)
        //        //                //{
        //        //                //    speed = (int)((diffPos * 10) / interpolatorInterval.Milliseconds);

        //        //                //}
                                
        //        //                if (diffPos != 0)
        //        //                {
        //        //                    //usc.setSpeed((byte)port, (ushort)speed);
        //        //                    //usc.setSpeed((byte)port, (ushort)(int)interpolatorInterval.TotalMilliseconds);
        //        //                    usc.setTarget((byte)port, (ushort)(rawVal / (0.25))); //the pulse width to transmit in units of quarter-microseconds 
        //        //                    //Debug.WriteLine("Connected to #" + serial + " position:" + (ushort)(rawVal / (0.25)) + " servo:" + motorsList.ElementAt(i).Channel+" time:"+ DateTime.Now.ToString("ss.fff"));
        //        //                }
        //        //                usc.Dispose();
        //        //                usc = null;
        //        //            }
                           
        //        //         }
        //        //    }
        //        //}

        //        //If the command is sent correctly, FACEBody.CurrentState is updated
        //        for (int j = 0; j < motorsList.Count; j++)
        //        {
        //            if (motorsList[j].PulseWidthNormalized != -1)
        //                FACEBody.CurrentMotorState[j].PulseWidthNormalized = motorsList[j].PulseWidthNormalized;
        //        }
                           
                   
           

        //}

        internal static void SendCommand(List<ServoMotor> motorsList)
        {

          


            try
            {

                //At this point each servo motor value must be != -1 (controlled in MotorTask).
                //If some servo motor value equals -1, it means that the servo motor
                //is not configured (those servo motors equal -1 also in the default configuration).

                List<ServoMotor> smList = motorsList.FindAll(a => a.PulseWidthNormalized != -1);
                smList.OrderBy(o => o.SerialSC);

                //_logger.Debug("SendCommand", () => smList);

                foreach (ServoMotor sm in smList)
                    sm.SendPosition(null);


            }
            catch (Exception ex)
            {
                //throw new FACException("Command not sent. Some errors occured.");
               // Debug.WriteLine("Connected to #" + se.Name + " position:" + (ushort)(se.MappingOnMinMaxInterval() / (0.25)) + " servo:" + se.Channel + " time:" + DateTime.Now.ToString("ss.fff"));

                throw new RobotException("Send Command Pololu Error:" + ex.Message);

            }

          
             
            //If the command is sent correctly, FACEBody.CurrentState is updated
            for (int j = 0; j < motorsList.Count; j++)
            {
                if (motorsList[j].PulseWidthNormalized != -1)
                    RobotControl.CurrentMotorState[j].PulseWidthNormalized = motorsList[j].PulseWidthNormalized;
            }

            //_logger.Information("Update position", () => RobotControl.CurrentMotorState);


        }

        #endregion

    }
}
