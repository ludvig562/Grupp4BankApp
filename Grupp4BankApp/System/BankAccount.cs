using Grupp4BankApp.Transaction;
using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.System
{

    internal class BankAccount
    {
        //properties
        private static int _nextBankAccountNumber = 1000; //starting number for bank accounts
        public int BankAccountNumber { get; private set; }
        public string BankAccountName { get; set; } 
        public decimal AccountBalance { get; set; }
        public decimal SavingsInterestRate { get; set; } = 0.03m; //3% savings interest rate
        public Enums.CurrencyTypes Currency { get; private set; }
        public Enums.AccountType AccountType { get; private set; }
        public string Owner { get; set; }
        public List<TransactionLogg> TransactionsLogg { get; set; } = new List<TransactionLogg>();

        //constructor
        public BankAccount(Enums.AccountType accountType, string bankAccountName, Enums.CurrencyTypes currency)
        {
            AccountType = accountType;
            BankAccountName = bankAccountName;
            Currency = currency;
            BankAccountNumber = _nextBankAccountNumber++; //ensuring that each bank account number is automatically different
        }

        public override string ToString()
        {
            return $"Bank Account Name: {BankAccountName}\n" +
                $"Bank Account Number: {BankAccountNumber}\n" +
                $"Balance: {AccountBalance} {Currency}";
        }
    }
}
