using Grupp4BankApp.State;

namespace Grupp4BankApp.Menus
{
    internal class AdminMenu
    {
        StateStack myState;

        public AdminMenu(StateStack aState)
        {
            myState = aState;
        }

        public void Display()
        {
            Console.WriteLine("This is Admin Menu");
        }
    }
}
