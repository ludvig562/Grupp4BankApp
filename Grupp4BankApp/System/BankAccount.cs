using Grupp4BankApp.Transaction;
using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Security.Principal;
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
        public decimal SavingsInterestRate { get; private set; } = 0.03m; //3% savings interest rate
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

        public void Deposit(decimal amount)
        {
            if(amount > 0)
            {
                AccountBalance += amount;
                Console.WriteLine($"\n{amount} {Currency} har satts in på kontot {BankAccountName}.\n" +
                    $"Din balans är nu: {AccountBalance} {Currency}");
            }
            else
            {
                Console.WriteLine("Ogiltigt belopp.");
            }
            
        }
        public decimal CalculateInterest()
        {
            return AccountBalance * SavingsInterestRate;
        }

        public override string ToString()
        {
            return $"Bank Account Name: {BankAccountName}\n" +
                $"Bank Account Number: {BankAccountNumber}\n" +
                $"Balance: {AccountBalance} {Currency}";
        }
    }
}
