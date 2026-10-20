using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class BankAccount
    {
        //properties
        public int BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        CurrencyTypes Currency { get; set; }
        public string Owner { get; set; }

        //private Dictionary<string, User> _users;

        //constructor
        public BankAccount(string owner, int bankAccountNumber, string bankAccountName, CurrencyTypes currency)
        {
            Owner = owner;
            BankAccountNumber = bankAccountNumber;
            BankAccountName = bankAccountName;
            Currency = currency;
        }
    }
}
