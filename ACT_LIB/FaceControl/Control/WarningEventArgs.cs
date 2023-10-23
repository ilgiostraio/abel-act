using Act.Lib.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Act.Lib.Control
{
    public class WarningEventArgs : EventArgs
    {
        public WarningEventArgs(RobotException fe)
        {
            fException = fe;
        }

        public RobotException fException
        {
            get;
            private set;
        }
    }

}
