using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Act.Lib.Animator
{
    public enum MotorTaskType { Generic, Single, Blinking, Yes, No, Respiration }

    /** 
     * This is the base class for all tasks that can be scheduled 
     * by the library. It simply defines the expected structure of an object 
     * that is handled by the scheduler. 
     * The model chosen by the library is that of Finite State Automata (FSA): 
     * each activity is an automata that is notified as time passes and of other 
     * events. 
     */
    public interface ITask
    {
        DateTime Start { get; set; }

        int Interval { get; set; }

        string uuid { get; set; }

       

        /**
         * Notify the task that the animation has been paused.
         */
        void OnPause();
        /**
         * Notify the task that the animation has been resumed.
         */
        void OnResume();
        /**
         * Notify the task that the animation has been stopped.
         */
        void OnStop();
        /**
         * Perform a step of computation
         */
        void DoAction(int delay);
    }


   




   
    

}