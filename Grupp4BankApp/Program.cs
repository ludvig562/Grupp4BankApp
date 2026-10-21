using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using static Grupp4BankApp.Users.User;
﻿using Grupp4BankApp.Manager;
using System.ComponentModel.Design;

namespace Grupp4BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User bob = new User();

            bob.Username = "bob";
            bob.Password = PasswordHasher.HashPassword("1234");
            bob.Role = UserRole.user;

            users.Add(bob.Username, bob);



            User bab = new User();

            bab.Username = "bab";
            bab.Password = PasswordHasher.HashPassword("1234");
            bab.Role = UserRole.admin;

            users.Add(bab.Username, bab);

            MenuManager.Init();
            MenuManager.SelectMenu(eMenus.eLogInMenu);
            //-------TEMPORARY TEST FOR LOGIN METHOD-------//

            //---------------------------------------------//

            Bank bank = new();
            bank.RunLogin();

            // Customer.OpenBankAccount();

        }
    }
}
