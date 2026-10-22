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
            //create a customer
            Customer bob = new Customer("bob", "bob", "test@gmail.com", PasswordHasher.HashPassword("1234"));
            users.Add(bob.Username, bob);

            //bob.Username = "bob";
            //bob.Password = PasswordHasher.HashPassword("1234");
            //bob.Role = Enums.UserRole.customer;


            //create an admin
            Admin bab = new Admin("bab", "bab", "test@gmail.com", PasswordHasher.HashPassword("1234"));
            users.Add(bab.Username, bab);

            //bab.Username = "bab";
            //bab.Password = PasswordHasher.HashPassword("1234");
            //bab.Role = Enums.UserRole.admin;


            //---------------------------------------------//

            //Bank bank = new();
            //bank.Run();
            MenuManager.Init();
            MenuManager.Run();

            //MenuManager.SelectMenu(eMenus.eLogInMenu);
            // Customer.OpenBankAccount();

        }
    }
}
