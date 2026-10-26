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

            BankAccount newCheckingAccount = new BankAccount(Enums.AccountType.checkingsAccount, "Konto", Enums.CurrencyTypes.SEK);
            BankAccount newSavingAcc = new BankAccount(Enums.AccountType.savingsAccount, "Sparkonto", Enums.CurrencyTypes.SEK);

            bob.checkingAccounts.Add(newCheckingAccount);
            bob.savingsAccounts.Add(newSavingAcc);

            newCheckingAccount.AccountBalance = 1000;
            newSavingAcc.AccountBalance = 500;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            Customer bobelina = new Customer("bobelina", "bobelina", "test@gmail.com", PasswordHasher.HashPassword("1234"));
            users.Add(bobelina.Username, bobelina);

            BankAccount newSecondCheckignACcount = new BankAccount(Enums.AccountType.checkingsAccount,"Konto",Enums.CurrencyTypes.SEK);
            BankAccount newsecondSaveAccount = new BankAccount(Enums.AccountType.savingsAccount, "Konto", Enums.CurrencyTypes.SEK);

            bobelina.checkingAccounts.Add(newSecondCheckignACcount);
            bobelina.savingsAccounts.Add(newsecondSaveAccount);
            newSecondCheckignACcount.AccountBalance = 1000;
            newsecondSaveAccount.AccountBalance = 500;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
