using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyConsole;

namespace TerminalFace.Pages
{
    class Script :Page
    {
        public Script(Program program)
            : base("Script", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine("Script");

            object input = EasyConsole.Input.ReadString("Please enter a string:");
            Output.WriteLine("You wrote: {0}", input);

            Runner.sendScript(input.ToString());

            EasyConsole.Input.ReadString("Press [Enter] to navigate home");

            Program.NavigateHome();
        }
    }
}
