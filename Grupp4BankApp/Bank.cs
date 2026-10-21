using Grupp4BankApp.Manager;
using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
namespace Grupp4BankApp
{
    internal class Bank
    {
        //private readonly AuthenticationService authentication = new AuthenticationService(User.users);

        //public void Run()// runs the whold bank first it gose thru login then depending on role it opens admin or user menu
        //{
        //    while (true)
        //    {
        //        User? user = new LogInMenu(authentication).Display();

        //        if (user == null)
        //        {
        //            return;
        //        }

        //        if (user.Role == User.UserRole.admin)
        //        { 
        //            AdminMenu.RunAdminMenu();
        //        }
        //        else
        //        {
        //            new CustomerMenu().Display();
        //        }

        //        authentication.Logout(); 
        //    }
        //}
    }
}
