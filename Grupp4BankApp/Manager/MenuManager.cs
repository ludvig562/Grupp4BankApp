using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;

namespace Grupp4BankApp.Manager
{
    
    internal static class MenuManager
    {
        private static AuthenticationService authentication;
        static private LogInMenu? myLogInMenu;
        static private CustomerMenu? myUserMenu;
        static private AdminMenu? myAdminMenu;

        public static void Init()
        {
            myLogInMenu = new LogInMenu();
            myUserMenu = new CustomerMenu();
            myAdminMenu = new AdminMenu();

            authentication = new AuthenticationService(User.users);
        }

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
                if (user.Role == Enums.UserRole.admin)
                {
                    SelectMenu(Enums.Menus.AdminMenu);
                }
                else if (user.Role == Enums.UserRole.customer)
                {
                    SelectMenu(Enums.Menus.UserMenu);
                }

                // clear the logged-in user after leaving the menu
                authentication.Logout();
            }
        }

        public static void SelectMenu(Enums.Menus aState)
        {

            switch (aState)
            {
                case Enums.Menus.LogInMenu:
                    if (myLogInMenu != null)
                        myLogInMenu.Display();
                    break;
                case Enums.Menus.UserMenu:
                    if (myUserMenu != null)
                        myUserMenu.Display();

                    break;
                case Enums.Menus.AdminMenu:
                    if (myAdminMenu != null)
                        myAdminMenu.Display();

                    break;
                default:
                    break;
            }
        }
    }
}
