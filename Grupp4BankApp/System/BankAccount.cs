using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.System
{

    internal class BankAccount
    {
        //properties
        private static int _nextBankAccountNumber = 100000; //starting number for bank accounts
        public int BankAccountNumber { get; private set; }
        public string BankAccountName { get; set; } 
        CurrencyTypes Currency { get; set; }
        AccountType AccountType { get; set; }


        //private Dictionary<string, User> _users;

        //constructor
        public BankAccount(AccountType accountType, string bankAccountName, CurrencyTypes currency)
        {
            AccountType = accountType;
            BankAccountName = bankAccountName;
            Currency = currency;
            BankAccountNumber = _nextBankAccountNumber++; //ensuring that each bank account number is automatically different
        }
    }
}
