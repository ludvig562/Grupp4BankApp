using Grupp4BankApp.State;

namespace Grupp4BankApp.Menus
{
    internal class UserMenu
    {
        StateStack state;
        public UserMenu(StateStack aState)
        {
            state = aState;
        }
        public void Display()
        {
            Console.WriteLine("This Is UserMenu");
            state.Pop();
        }

        

    }
}
