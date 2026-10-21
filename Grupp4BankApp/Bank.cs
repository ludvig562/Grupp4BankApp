using Grupp4BankApp.Manager;
using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
namespace Grupp4BankApp
{
    internal class Bank
    {
        private readonly AuthenticationService authentication;
        private readonly LogInMenu aLogInMenu;
        public Bank()
        {
            authentication = new AuthenticationService(User.users);
            aLogInMenu = new LogInMenu();
        }

        public void RunLogin()
        {
            User? loggedInUser = null;


            loggedInUser = authentication.GetUser();



            switch (loggedInUser.Role)
            {
                case Enums.UserRole.admin:
                    MenuManager.SelectMenu(eMenus.eAdminMenu);
                    break;
                case Enums.UserRole.customer:
                    MenuManager.SelectMenu(eMenus.eUserMenu);
                    break;
                default:
                    Console.WriteLine("Är inte en användare i vårat system");
                    break;
            }

        }
    }
}
