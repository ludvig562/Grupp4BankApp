using Grupp4BankApp.State;
using Grupp4BankApp.Menus;

namespace Grupp4BankApp.Manager
{
    internal class MenuManager
    {


        private StateStack myState = new StateStack();

        private LogInMenu myLogInMenu;
        private UserMenu myUserMenu;
        private AdminMenu myAdminMenu;

        public MenuManager()
        {
            myLogInMenu = new LogInMenu(myState);
            myUserMenu = new UserMenu(myState);
            myAdminMenu = new AdminMenu(myState);
        }

        public void Update()
        {

            switch (myState.GetCurrentStateEnum())
            {
                case eStates.eLogInMenu:
                    myLogInMenu.Display();
                    break;
                case eStates.eUserMenu:
                    myUserMenu.Display();

                    break;
                case eStates.eAdminMenu:
                    myAdminMenu.Display();

                    break;
                default:
                    break;
            }
        }
        public void PushMenu(eStates menu)
        {
            myState.Push(menu);
        }

    }
}
