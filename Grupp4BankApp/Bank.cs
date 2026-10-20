using Grupp4BankApp.Users;
using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;
using Grupp4BankApp.Manager;
using Grupp4BankApp.State;
namespace Grupp4BankApp
{
    internal class Bank
    {
        private readonly AuthenticationService authentication;

        public Bank()
        {
            authentication = new AuthenticationService(User.users);
        }

        public void RunLogin()
        {
            User? loggedInUser = null;

            while (loggedInUser == null)// while loggedinuser is null keep repeting the login method
            {
                
                loggedInUser = authentication.Login();
            }

            switch (loggedInUser.Role)
            {
                case User.UserRole.admin:
                    MenuManager.PushMenu(eStates.eAdminMenu);

                    break;
                case User.UserRole.user:
                    MenuManager.PushMenu(eStates.eUserMenu);
                    break;
                default:
                    Console.WriteLine("Är inte en användare i vårat system");
                    break;
            }

        }
    }
}
