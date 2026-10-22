using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class Loan
    {
        private Customer? _customer => AuthenticationService.CurrentUser as Customer;
        public void MakeLoan(decimal loan, int accountChoise)
        {
            if (accountChoise < 1 || accountChoise > _customer.checkingAccounts.Count)
            {
                Console.WriteLine("Kontot finns inte.");
                return;
            }

            BankAccount selectedAccount = _customer.checkingAccounts[accountChoise - 1];

            Console.WriteLine($"Du valde kontot: {selectedAccount.BankAccountName}");
            Console.WriteLine($"Lånebelopp: {loan} kr");
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
                Console.WriteLine($"Valuta: {account.Currency}");
                Console.WriteLine();
                i++;
            }
        }
        //----------------------------------------------------------------------//
    }
}
