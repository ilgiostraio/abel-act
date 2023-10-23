
using TerminalFace.Pages;
using EasyConsole;
namespace TerminalFace
{
    class TerminalMenu : Program
    {
        public TerminalMenu() 
            :base("EasyConsole Demo", breadcrumbHeader: true)
        {

            AddPage(new MainPage(this));
            AddPage(new ECS(this));
            AddPage(new LookAt(this));
            AddPage(new Script(this));
            //AddPage(new Page1B(this));
            //AddPage(new Page2(this));
            //AddPage(new InputPage(this));

            SetPage<MainPage>();
        }

      
    }
}
