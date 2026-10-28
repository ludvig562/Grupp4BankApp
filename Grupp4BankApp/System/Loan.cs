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
        private decimal _interestRate;
        private Customer? _customer => AuthenticationService.CurrentUser as Customer; //gets the curent logged in user

        public decimal LoanAmount { get; set; } 
        public int LoanYears { get; set; }
        public decimal TotalLoan { get; set; }

        public Loan()
        {
        }
        public Loan(decimal loanAmount, int loanYears, decimal totalLoan, decimal interestRate)
        {
            LoanAmount = loanAmount;
            LoanYears = loanYears;
            TotalLoan = totalLoan;
            _interestRate = interestRate;
        }

        public void ShowLoanMenu()// loan menu
        {
            Console.Clear();
            Console.WriteLine("Vad vill du göra?\n" +
                "[1] Lista över dina lån\n" +
                "[2] Låna Pengar\n" +
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
                    Console.Write("Ange kontonummer: ");
                    string accountNumber = Console.ReadLine() ?? "";

                    MakeLoan(loan, accountNumber);
                }
                Console.WriteLine("\nTryck på Enter för att återgå till menyn.");
                Console.ReadLine();
                Console.Clear();
            }
            Console.Clear();
        }

        public void MakeLoan(decimal loan, string accountNumber)
        {
            BankAccount? selectedAccount = null;

            foreach (BankAccount account in _customer.CheckingAccounts)
            {
                if (account.BankAccountNumber.ToString() == accountNumber)
                {
                    selectedAccount = account;
                    break;
                }
            }

            if (selectedAccount == null)
            {
                Console.WriteLine("Kontot finns inte.");
                return;
            }

            Console.Write("Ange återbetalningstid i år (1-10): ");

            int loanYears;

            while (!int.TryParse(Console.ReadLine(), out loanYears) || loanYears < 1 || loanYears > 10)
            {
                Console.WriteLine("Du måste ange ett nummer mellan 1-10!");
            }

            // Get interest rate using both factors
            decimal interestRate = LoanInterestRules.CalculateInterestRate(_customer.CreditRating, loanYears);

            // Calculate total loan cost
            decimal totalLoan = CalculateTotalInterest(loan, loanYears, interestRate);

            // Deposit money into selected account
            selectedAccount.AccountBalance += CalculateLoanExchangeRate(selectedAccount.Currency, loan);

            Loan newLoan = new Loan(
                loan,
                loanYears,
                totalLoan,
                interestRate
            );

            _customer.Loans.Add(newLoan);

            Console.WriteLine($"\nDu valde kontot: {selectedAccount.BankAccountName}");
            Console.WriteLine($"Lånebelopp: {loan:N2} kr");
            Console.WriteLine($"Kreditvärdighet: {_customer.CreditRating}");
            Console.WriteLine($"Grundränta: {LoanInterestRules.GetBaseInterest(_customer.CreditRating):P2}");
            Console.WriteLine($"Ränta justering för lånetid: {LoanInterestRules.GetTermInterest(loanYears):P2}");
            Console.WriteLine($"Slutlig årsränta: {interestRate:P2}");
            Console.WriteLine($"Återbetalningstid: {loanYears} år");
            Console.WriteLine($"Total räntekostnad: {totalLoan - loan:N2} kr");
            Console.WriteLine($"Total återbetalning: {totalLoan:N2} kr");
        }

        public void PrintAccounts()
        {
            Console.WriteLine("----- Dina konton -----");
            foreach (BankAccount account in _customer.CheckingAccounts)
            {
                Console.WriteLine($"Namn: {account.BankAccountName}");
                Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                Console.WriteLine($"KontoSaldo: {account.AccountBalance}");
                Console.WriteLine($"Valuta: {account.Currency}");
                Console.WriteLine();
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

        public decimal CalculateTotalInterest(decimal loan, int years, decimal interestRate)
        {
            decimal yearlyAmount = loan / years;
            decimal remainingDebt = loan;
            decimal totalInterest = 0;

            for (int year = 1; year <= years; year++)
            {
                decimal yearlyInterest = remainingDebt * interestRate;

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
