
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Act.Lib.Control;
using Act.Lib.Robot;

using Serilog;

namespace Act.Lib.Animator
{
    /******************************* MotorInterfaceTask Class *******************************/

    /// <summary>
    /// InterpolatorTask class represents the task which interpolates
    /// all the tasks into the heap of the AnimationEngine when it is being run.
    /// </summary>
    public class InterpolatorTask : ITask
    {

        //private static readonly ILogger _logger = Log.ForContext("SourceContext", "InterpolatorTask");


        /// <summary>
        /// 
        /// </summary>
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        /// <summary>
        /// Task frequency in milliseconds
        /// </summary>
        private TimeSpan interval;
        public int Interval
        {
            get { return (int)interval.TotalMilliseconds; }
            set { interval = TimeSpan.FromMilliseconds(value); }
        }

        private string _uuid;
        public string uuid {
            get { return _uuid; }
            set { _uuid = value; }
        }

        private string typeTask;
        public string TypeTask
        {
            get { return this.GetType().Name;  }
         
        }

        public InterpolatorTask(DateTime when, TimeSpan interv)
        {
            start = when;
            interval = interv;

            //_logger.Debug("new InterpolatorTask -> when {when} interv {interv} ms", when, interv.Milliseconds);
        }


        #region InterpolatorTask Members

        public void OnPause()
        {
        }

        public void OnResume()
        {
        }

        public void OnStop()
        {
        }

        /// <summary>
        /// Calculate the new positions for each motors based on all the 
        /// tasks which are required to be executed.
        /// </summary>
        /// <param name="delay"></param>
        public void DoAction(int delay = 0)
        {
            

            Dictionary<int, List<MotorStep>> valuesToInterp = new Dictionary<int, List<MotorStep>>(AnimatorEngine.MotorSteps);

            //_logger.Debug("Start DoAction InterpolatorTask with delay "+delay, () => valuesToInterp);

            AnimatorEngine.MotorSteps.Clear();

            if (valuesToInterp.Count != 0)
            {
                //init empty list motor position
                List<ServoMotor> motorList = new RobotMotion(RobotControl.CurrentMotorState.Count).ServoMotorsList;

                try
                {
                    foreach (KeyValuePair<int, List<MotorStep>> couple in valuesToInterp)
                    {
                        List<MotorStep> value = couple.Value.ToList();

                        float totVal = 0;
                        int totPr = 0;
                        foreach (MotorStep step in value)
                        {
                            totVal += (step.val * step.pr);
                            totPr += step.pr;
                        }
                        motorList[couple.Key].PulseWidthNormalized =
                            RobotControl.CurrentMotorState[couple.Key].PulseWidthNormalized + (totVal / totPr);
                    }
                    AnimatorEngine.SendCommand(motorList);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //AnimationEngine.SendCommand(FACEBody.DefaultMotorState);  //check correctness
                }
            }
            //else
            //    _logger.Debug("End DoAction InterpolatorTask without MotorSteps");

            // Reschedule
            AnimatorEngine.AddTask(this, DateTime.Now + interval);
        }

        #endregion
    }
}
