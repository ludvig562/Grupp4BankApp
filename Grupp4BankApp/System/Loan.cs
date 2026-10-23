using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class Loan
    {
        private decimal _interestRate = 0.08m; // 8% annual interest
        private Customer? _customer => AuthenticationService.CurrentUser as Customer;
        public void MakeLoan(decimal loan, int accountChoice)
        {
            if (accountChoice < 1 || accountChoice > _customer.checkingAccounts.Count)
            {
                Console.WriteLine("Kontot finns inte.");
                return;
            }

            BankAccount selectedAccount = _customer.checkingAccounts[accountChoice - 1];

            decimal yearlyInterest = loan * _interestRate;

            selectedAccount.AccountBalance += loan;

            Console.WriteLine($"Du valde kontot: {selectedAccount.BankAccountName}");
            Console.WriteLine($"Lånebelopp: {loan} kr");
            Console.WriteLine($"Årsränta: {_interestRate:P0}");
            Console.WriteLine($"Räntekostnad per år: {yearlyInterest} kr");
            Console.WriteLine();
        }

        //------illfällig prin metod tills att yevgeniy fixar print konto------// 
        public void PrintAccounts()
        {
            Console.WriteLine("----- Dina konton -----");
            int i = 1;
            foreach (BankAccount account in _customer.checkingAccounts)
            {
                Console.WriteLine($"[{i}].");
                Console.WriteLine($"Namn: {account.BankAccountName}");
                Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                Console.WriteLine($"KontoSaldo: {account.AccountBalance}");
                Console.WriteLine($"Valuta: {account.Currency}");
                Console.WriteLine();
                i++;
            }
        }
        //----------------------------------------------------------------------//
    }
}
