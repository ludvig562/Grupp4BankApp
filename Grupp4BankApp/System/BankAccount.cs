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
        public decimal AccountBalance { get; set; }
        public decimal InterestRate { get; set; }
        public Enums.CurrencyTypes Currency { get; private set; }
        public Enums.AccountType AccountType { get; set; }

        //constructor
        public BankAccount(Enums.AccountType accountType, string bankAccountName, Enums.CurrencyTypes currency)
        {
            AccountType = accountType;
            BankAccountName = bankAccountName;
            Currency = currency;
            BankAccountNumber = _nextBankAccountNumber++; //ensuring that each bank account number is automatically different
        }
    }
}
