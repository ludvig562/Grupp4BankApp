using Figgle.Fonts;
using Grupp4BankApp.Manager;
using Grupp4BankApp.State;

namespace Grupp4BankApp.Menus
{
    internal class LogInMenu
    {
        StateStack myState;
        public LogInMenu(StateStack aState)
        {
            myState = aState;
        }
        public void BankLogo()
        {
            Console.WriteLine(FiggleFonts.Starwars.Render("N E T 26 Bank"));
        }
        public void Display()
        {
            BankLogo();


        }
    }
}
