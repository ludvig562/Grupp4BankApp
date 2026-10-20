using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using static Grupp4BankApp.Users.User;
﻿using Grupp4BankApp.Manager;
using Grupp4BankApp.State;
using System.ComponentModel.Design;

namespace Grupp4BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuManager.Init();
#if DEBUG
            //-------TEMPORARY TEST FOR LOGIN METHOD-------//
            User bob = new User();

            bob.Username = "bob";
            bob.Password = PasswordHasher.HashPassword("1234");
            bob.Role = UserRole.user;

            users.Add(bob.Username, bob);
            //---------------------------------------------//
#endif
            MenuManager.Update();

            Bank bank = new();
            bank.RunLogin();




        }
    }
}
