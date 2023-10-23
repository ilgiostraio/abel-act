using EasyConsole;

namespace TerminalFace.Pages
{
    class ECS : Page
    {
        public ECS(Program program)
            : base("ECS", program)
        {
           
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine("ECS");

            decimal v = Input.ReadDecimal("Please enter value of valence (between 0 and 1):", min: 0, max: 1);
            decimal a = Input.ReadDecimal("Please enter value of arousal (between 0 and 1):", min: 0, max: 1);


            Runner.sendECS(v, a);

            EasyConsole.Input.ReadString("Press [Enter] to navigate home");

            Program.NavigateHome();
        }
    }

}
