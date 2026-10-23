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

        public bool CheckCustomerMoney(decimal loan)
        {
            decimal moneyInAccounts = 0;
            foreach(BankAccount checkingMoney in _customer.checkingAccounts)
            {
                moneyInAccounts += checkingMoney.AccountBalance;
            }

            foreach (BankAccount savingsMoney in _customer.savingsAccounts)
            {
                moneyInAccounts += savingsMoney.AccountBalance;
            }

            if(loan > moneyInAccounts * 5)
            {
                Console.WriteLine($"Du kan inte låna så mycket max 5 gånger det du har i våran bank\nDu har totalt: {moneyInAccounts} kr");
                return false;
            }
            return true;
        }
    }
}
