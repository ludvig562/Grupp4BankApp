using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class Loan
    {
        private decimal _interestRate = 0.08m; // 8% annual interest
        private Customer? _customer => AuthenticationService.CurrentUser as Customer; //gets the curent logged in user

        public decimal LoanAmount { get; set; } 
        public int LoanYears { get; set; }
        public decimal TotalLoan { get; set; }

        public Loan()
        {
        }
        public Loan(decimal loanAmount, int loanYears, decimal totalLoan)
        {
            LoanAmount = loanAmount;
            LoanYears = loanYears;
            TotalLoan = totalLoan;
        }

        public void ShowLoanMenu()// loan menu
        {
            Console.Clear();
            Console.WriteLine("Vad vill du göra?\n" +
                "[1] Lista över dina lån\n" +
                "[2] Låna pengar\n" +
                "[3] Gå tillbaka\n");
            int loanMenuChoice = 0;
            while (!int.TryParse(Console.ReadLine(), out loanMenuChoice) || loanMenuChoice < 1 || loanMenuChoice > 3)
            {
                Console.WriteLine("Du måste ange ett giltit tal!");
            }
            if (loanMenuChoice == 1)
            {
                PrintLoans();
                Console.WriteLine("\nTryck på Enter för att återgå till menyn.");
                Console.ReadLine();
                Console.Clear();
            }
            else if (loanMenuChoice == 2)
            {
                Console.Write("Hur mycket vill du låna i SEK?: ");
                int loan = 0;
                while (!int.TryParse(Console.ReadLine(), out loan) || loan <= 0)
                {
                    Console.WriteLine("Du måste ange ett giltit antal!");
                }

                if (CheckCustomerMoney(loan))
                {
                    PrintAccounts();
                    Console.WriteLine("Vilket konto ska pengarna sättas in i?: ");
                    int accountChoice = 0;
                    while (!int.TryParse(Console.ReadLine(), out accountChoice))
                    {
                        Console.WriteLine("Du måste ange ett giltit tal!");
                    }
                    MakeLoan(loan, accountChoice);
                }
                Console.WriteLine("\nTryck på Enter för att återgå till menyn.");
                Console.ReadLine();
                Console.Clear();
            }
            Console.Clear();
        }

        public void MakeLoan(decimal loan, int accountChoice)// makes the loan 
        {
            Console.Write("Ange återbetalningstid i år (1–10): ");
            int loanYears = 0;
            while(!int.TryParse(Console.ReadLine(), out loanYears) || loanYears <1 || loanYears > 10)
            {
                Console.WriteLine("Du måste ange ett nummer mellan 1-10!");
            }

            if (accountChoice < 1 || accountChoice > _customer.CheckingAccounts.Count)
            {
                Console.WriteLine("Kontot finns inte.");
                return;
            }

            BankAccount selectedAccount = _customer.CheckingAccounts[accountChoice - 1];

            selectedAccount.AccountBalance += CalculateLoanExchangeRate(selectedAccount.Currency, loan);

            Loan newLoan = new Loan(loan, loanYears, CalculateTotalInterest(loan, loanYears));

            _customer.Loans.Add(newLoan);

            Console.WriteLine($"Du valde kontot: {selectedAccount.BankAccountName}");
            Console.WriteLine($"Lånebelopp: {loan} kr");
            Console.WriteLine($"Årsränta: {_interestRate:P0}");
            Console.WriteLine($"Återbetalningstid: {loanYears} år" );
            Console.WriteLine($"Total Räntekostnad: {CalculateTotalInterest(loan, loanYears) - loan} kr");
            Console.WriteLine();
        }

        public void PrintAccounts()
        {
            Console.WriteLine("----- Dina konton -----");
            int i = 1;
            foreach (BankAccount account in _customer.CheckingAccounts)
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
        
        public bool CheckCustomerMoney(decimal loan)// Checks how much money the customer has in total
        {
            decimal moneyInAccounts = 0;
            foreach(BankAccount checkingMoney in _customer.CheckingAccounts)
            {
                moneyInAccounts += checkingMoney.AccountBalance;
            }

            foreach (BankAccount savingsMoney in _customer.SavingsAccounts)
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

        public decimal CalculateTotalInterest(decimal loan, int years)// calculates how much interest the customer will have on theer loan based on how many years they will pay
        {
            decimal yearlyAmount = loan / years;
            decimal remainingDebt = loan;
            decimal totalInterest = 0;

            for (int year = 1; year <= years; year++)
            {
                decimal yearlyInterest = remainingDebt * _interestRate;

                totalInterest += yearlyInterest;

                remainingDebt -= yearlyAmount;
            }

            return loan + totalInterest;
        }

        public void PrintLoans() // prints all the loans
        {
            if (_customer.Loans.Count == 0)
            {
                Console.WriteLine("Du har inga lån.");
                return;
            }

            Console.WriteLine("-----Dina lån-----");
            int i = 1;
            foreach (Loan loan in _customer.Loans)
            {
                Console.WriteLine($"[{i}].");
                Console.WriteLine($"Lånebelopp: {loan.LoanAmount} kr");
                Console.WriteLine($"Årsränta: {loan._interestRate:P0}");
                Console.WriteLine($"Återbetalningstid: {loan.LoanYears} år");
                Console.WriteLine($"Total skuld: {loan.TotalLoan} kr");
                Console.WriteLine();
                i++;
            }
        }

        public decimal CalculateLoanExchangeRate(Enums.CurrencyTypes currency, decimal loan)
        {
            switch (currency)
            {

                case Enums.CurrencyTypes.EUR:
                    decimal eurLoan = loan / ExchangeRates.GetRate(Enums.CurrencyTypes.EUR);
                    return eurLoan;

                case Enums.CurrencyTypes.USD:
                    decimal usdLoan = loan / ExchangeRates.GetRate(Enums.CurrencyTypes.USD);
                    return usdLoan;

                default:
                    return loan;
            }
        }

    }
}
