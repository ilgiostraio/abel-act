using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog;
using Serilog.Events;

using Act.Lib.Control;
using Act.Lib.Robot;
using System.Linq.Expressions;

namespace Act.Lib.Animator
{
    /******************************* MotorTask Class *******************************/

    /// <summary>
    /// MotorTask class represents the movement of a motor
    /// from a motor configuration to another
    /// </summary>
    public class MotorTask : ITask, ICloneable
    {

       // private static readonly ILogger _logger = Log.ForContext("SourceContext", "MotorTask");


        private MotorTaskType type;
        public MotorTaskType Type
        {
            get { return type; }
            set { type = value; }
        }

        private string _uuid;
        public string uuid
        {
            get { return _uuid; }
            set { _uuid = value; }
        }


        private string typeTask;
        public string TypeTask
        {
            get { return this.GetType().Name;  }

        }

        private List<ServoMotor> startPosition;// non usato
        public List<ServoMotor> StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        private List<ServoMotor> endPosition;
        public List<ServoMotor> EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        private TimeSpan interval;
        public int Interval
        {
            get { return (int)interval.TotalMilliseconds; }
            set { interval = TimeSpan.FromMilliseconds(value); }
        }

        private TimeSpan duration;
        public int Duration
        {
            get { return (int)duration.TotalMilliseconds; }
            set { duration = TimeSpan.FromMilliseconds(value); }
        }

        private int numTimes;
        public int NumTimes
        {
            get { return numTimes; }
            set { numTimes = value; }
        }

        private int frequency;
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        private int priority;
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        private int steps;
        public int Steps
        {
            get { return steps; }
        }


        public MotorTask() { }

      
        //private int restoreSteps = 0;

        public MotorTask(MotorTaskType tType, RobotMotion motion, DateTime when, int times, int freq)
        {
            uuid = Guid.NewGuid().ToString();

            type = tType;
            endPosition = motion.ServoMotorsList;
            start = when;
            duration = TimeSpan.FromMilliseconds(motion.Duration);
            interval = RobotControl.MotorInterval;
            steps = (int)(duration.TotalMilliseconds / interval.TotalMilliseconds);
            numTimes = times;
            frequency = freq;
            priority = motion.Priority;


            //new LogEvent(DateTimeOffset.Now, level, exception, parsedTemplate, boundProperties)

            //AddProperty().Debug("New MotorTask");


            //sendMetrics(BaseProperty(), this);

            //_logger.Debug("dzf", () => this);

            //restoreSteps = steps;
        }
       




        //private ILogger BaseProperty() 
        //{
        //   ILogger logger=  _logger.AddProperty(() => type)
        //        .AddProperty(() => uuid)
        //        .AddProperty(() => type)
        //        .AddProperty(() => start)
        //        .AddProperty(() => duration)
        //        .AddProperty(() => interval)
        //        .AddProperty(() => steps)
        //        .AddProperty(() => numTimes)
        //        .AddProperty(() => frequency)
        //        .AddProperty(() => priority);

          

        //    return logger;
        //    //return logger;
        //}

        private static async void sendMetrics(ILogger logger, MotorTask mt)
        {
            await Task.Run(() =>
            {


                List<ServoMotor> lsm = mt.endPosition.FindAll(a => a.PulseWidthNormalized != -1).ToList();

                foreach (ServoMotor s in lsm)
                {
                    ServoMotor sa = (ServoMotor)s.Clone();
                    sa.PulseWidthNormalized = 0;

                    MotorTask m = (MotorTask)mt.Clone();
                    m.start = m.start.AddMilliseconds(mt.duration.Milliseconds);


                    logger.ForContext("endPosition", sa, true)
                          .Debug("New MotorTask");
                    

                    logger.AddProperty(() => m.start)
                          .ForContext("endPosition", s, true)
                          .Debug("New MotorTask");
                }
            });
        }

        

        #region MotorTask Members

        public void OnPause()
        {
            //restoreSteps = steps;
        }

        public void OnResume()
        {
            //steps = restoreSteps;
        }

        public void OnStop()
        {
        }

        /// <summary>
        /// A task is divided in subtasks based on the number of steps of the task.
        /// DoAction calculates the amount of movement that each motors must perform at each step.
        /// </summary>
        /// <param name="delay"></param>
        public void DoAction(int delay)
        {
            float val = 0;

            //_logger.Debug("Start DoAction MotorTask");

            startPosition = RobotControl.CurrentMotorState;

            //_logger.Verbose("Get CurrentMotorState", ()=>startPosition);

            // Calculate the amount of movement that each motor must perform at the current step
            // and send it to the AnimationEngine
            for (int i = 0; i < endPosition.Count; i++)
            {
                //current[i] must be different from -1 (motor not working)
                if (endPosition[i].PulseWidthNormalized != -1 && startPosition[i].PulseWidthNormalized != -1)
                {
                    if (steps == 0)
                        val = (endPosition[i].PulseWidthNormalized - startPosition[i].PulseWidthNormalized);
                    else
                        val = (endPosition[i].PulseWidthNormalized - startPosition[i].PulseWidthNormalized) / steps;

                    if (Math.Round(val, 4) != 0)
                    {
                        AddMotor(i, val, priority);
                    }
                }
            }
            steps--;

            // If the task is not completed, it is rescheduled into the heap
            if (steps > 0)
            {
               //_logger.Verbose("Necessary other steps for this MotorTask ", () => this);
                AnimatorEngine.AddTask(this, DateTime.Now + interval);
            }
            else
            {
                if (numTimes > 0)
                {
                   
                    numTimes--;
                    //wait = TimeSpan.FromMilliseconds(frequency);
                }

                if (steps == 0 && numTimes != 0)
                {
                    //_logger.Verbose("Necessary other loop for this MotorTask ", () => this);
                    
                    steps = (int)(duration.TotalMilliseconds / interval.TotalMilliseconds);
                    //Random r = new Random();
                    //double factor = r.NextDouble() + 0.5;
                    //TimeSpan wait = TimeSpan.FromMilliseconds((int)((60 / frequency) * 1000) * factor); //value between (wait/2) and (1.5*wait)

                    TimeSpan wait = TimeSpan.FromMilliseconds(frequency);
                    AnimatorEngine.AddTask(this, DateTime.Now + duration + wait);
                }
            }
        }


        /// <summary>
        /// Add a motor to the dictionary with a value and a priority.
        /// This dictionary will be used by the Interpolator task to merge the collected movements:
        /// all data in the dictionary will become a MotorTask to be executed.
        /// </summary>
        /// <param name="id"> The motor identifier. </param>
        /// <param name="val"> The motor value. </param>
        /// <param name="pr"> The motor priority (not used at the moment). </param>
        private static void AddMotor(int id, float val, int pr)
        {
            MotorStep step = new MotorStep();
            step.val = val;
            step.pr = pr;

            if (AnimatorEngine.MotorSteps.Keys.Contains(id))
                AnimatorEngine.MotorSteps[id].Add(step);
            else
            {
                List<MotorStep> values = new List<MotorStep>();
                values.Add(step);
                AnimatorEngine.MotorSteps.Add(id, values);
            }

            //_logger.Information("MotorTask->AddMotor()", () => id, () => step);
        }
        #endregion

        public object Clone()
        {
            return new MotorTask
            {
                type = this.type,
                uuid = this.uuid,
                typeTask = this.typeTask,
                startPosition = this.startPosition,
                endPosition = this.endPosition,
                start = this.start,
                interval = this.interval,
                duration = this.duration,
                numTimes = this.numTimes,
                frequency = this.frequency,
                priority = this.priority,
                steps = this.steps


            };
        }
    }


   


}
