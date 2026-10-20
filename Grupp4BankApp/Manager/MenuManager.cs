using Grupp4BankApp.State;
using Grupp4BankApp.Menus;

namespace Grupp4BankApp.Manager
{
    internal static class MenuManager
    {


        static private StateStack myState = new StateStack();

        static private LogInMenu? myLogInMenu;
        static private UserMenu? myUserMenu;
        static private AdminMenu? myAdminMenu;


        public static void Init()
        {
            myLogInMenu = new LogInMenu(myState);
            myUserMenu = new UserMenu(myState);
            myAdminMenu = new AdminMenu(myState);
        }
        public static void Update()
        {

            switch (myState.GetCurrentStateEnum())
            {
                case eStates.eLogInMenu:
                    if (myLogInMenu != null)
                        myLogInMenu.Display();
                    break;
                case eStates.eUserMenu:
                    if (myUserMenu != null)
                        myUserMenu.Display();

                    break;
                case eStates.eAdminMenu:
                    if (myAdminMenu != null)
                        myAdminMenu.Display();

                    break;
                default:
                    break;
            }
        }
        public static void PushMenu(eStates menu)
        {
            myState.Push(menu);
        }

    }
}
