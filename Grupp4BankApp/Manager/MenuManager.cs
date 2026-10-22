using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;

namespace Grupp4BankApp.Manager
{
    enum eMenus
    {
        eLogInMenu,
        eUserMenu,
        eAdminMenu,

    }
    internal static class MenuManager
    {


        // static private StateStack myState = new StateStack();

        //----------//
        private static AuthenticationService authentication;
        //----------//
        static private LogInMenu? myLogInMenu;
        static private CustomerMenu? myUserMenu;
        static private AdminMenu? myAdminMenu;

        public static void Init()
        {
            myLogInMenu = new LogInMenu();
            myUserMenu = new CustomerMenu();
            myAdminMenu = new AdminMenu();

            //-----//
            authentication = new AuthenticationService(User.users);
            //-----//
        }

        //-----------------------------------------------------------//
        public static void Run()
        {
            while (true)
            {
                // login returns the user who logged in
                User? user = myLogInMenu.Display();

                // exit the program if login was cancelled
                if (user == null)
                {
                    return;
                }

                // open the correct menu based on the user's role
                if (user.Role == User.UserRole.admin)
                {
                    SelectMenu(eMenus.eAdminMenu);
                }
                else if (user.Role == User.UserRole.user)
                {
                    SelectMenu(eMenus.eUserMenu);
                }

                // clear the logged-in user after leaving the menu
                authentication.Logout();
            }
        }
        //-----------------------------------------------------------//

        public static void SelectMenu(eMenus aState)
        {

            switch (aState)
            {
                case eMenus.eLogInMenu:
                    if (myLogInMenu != null)
                        myLogInMenu.Display();
                    break;
                case eMenus.eUserMenu:
                    if (myUserMenu != null)
                        myUserMenu.Display();

                    break;
                case eMenus.eAdminMenu:
                    if (myAdminMenu != null)
                        myAdminMenu.Display();

                    break;
                default:
                    break;
            }
        }

        //public static void Update()
        //{

        //    switch (myState.GetCurrentStateEnum())
        //    {
        //        case eMenus.eLogInMenu:
        //            if (myLogInMenu != null)
        //                myLogInMenu.Display();
        //            break;
        //        case eMenus.eUserMenu:
        //            if (myUserMenu != null)
        //                myUserMenu.Display();

        //            break;
        //        case eMenus.eAdminMenu:
        //            if (myAdminMenu != null)
        //                myAdminMenu.Display();

        //            break;
        //        default:
        //            break;
        //    }
        //}
        //public static void PushMenu(eMenus menu)
        //{
        //    myState.Push(menu);
        //    Update();

        //}

    }
}
