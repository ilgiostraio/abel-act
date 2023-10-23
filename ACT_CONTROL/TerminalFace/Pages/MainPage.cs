using EasyConsole;


namespace TerminalFace.Pages
{

    
        class MainPage : MenuPage
        {
            public MainPage(Program program)
                : base("Main Page", program,
                    new Option("ECS", () => program.NavigateTo<ECS>()),
                    new Option("LookAt", () => program.NavigateTo<LookAt>()),
                    new Option("Script", () => program.NavigateTo<Script>()))
                   
            {
            }
        }
    
}
