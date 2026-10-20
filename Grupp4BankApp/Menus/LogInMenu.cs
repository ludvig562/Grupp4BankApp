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
        public void Display()
        {
            Console.WriteLine("This Is log in menu");
            myState.Push(eStates.eUserMenu);
            

        }
    }
}
