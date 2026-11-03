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
        private Customer? _customer => AuthenticationService.CurrentUser as Customer; // Gets the currently logged-in user as a Customer

        private decimal _interestRate; // Stores the interest rate for this loan

        // Properties that store information about the loan
        public decimal LoanAmount { get; set; }
        public int LoanYears { get; set; }
        public decimal TotalLoan { get; set; }

        
        public Loan()
        {
        }
        // Constructor that sets the loan information
        public Loan(decimal loanAmount, int loanYears, decimal totalLoan, decimal interestRate)
        {
            LoanAmount = loanAmount;
            LoanYears = loanYears;
            TotalLoan = totalLoan;
            _interestRate = interestRate;
        }

        // Displays the loan menu and handles the user's choices
        public void ShowLoanMenu()
        {
            bool showLoanMenu = true;
            while (showLoanMenu)
            {
                Console.Clear();
                Console.WriteLine("Vad vill du göra?\n" +
                    "[1] Lista över dina lån\n" +
                    "[2] Låna pengar\n" +
                    "[3] Gå tillbaka till huvudmenyn");
                int loanMenuChoice = 0;
                // Makes sure the user enters a number between 1 and 3
                while (!int.TryParse(Console.ReadLine(), out loanMenuChoice) || loanMenuChoice < 1 || loanMenuChoice > 3)
                {
                    Console.WriteLine("Du måste ange ett nummer mellan 1 och 3!");
                }
                if (loanMenuChoice == 1)
                {
                    // Displays all loans belonging to the customer
                    PrintLoans();
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (loanMenuChoice == 2)
                {
                    int loan = 0;
                    Console.Clear();
                    Console.WriteLine("Hur mycket vill du låna i SEK?\n" +
                        "(Eller ange 0 för att avsluta lånförfrågan och gå tillbaka till lånmeny.)");

                    // Validates the amount and checks if the customer is allowed to borrow it
                    while (!int.TryParse(Console.ReadLine(), out loan) ||
                           (loan != 0 && (loan < 0 || !CheckCustomerMoney(loan))))
                    {
                        Console.WriteLine("Ange ett nytt lånebelopp\n" +
                            "(Eller ange 0 för att avsluta lånförfrågan och gå tillbaka till lånmeny.)");
                    }

                    // Returns to the loan menu if the user enters 0
                    if (loan == 0)
                    {
                        continue;
                    }

                    // Shows the customer's checking accounts
                    PrintAccounts();

                    Console.WriteLine("Vilket konto ska pengarna sättas in i?\n" +
                        "(Eller ange 0 för att avsluta lånförfrågan och gå tillbaka till lånmeny.)");
                    string accountNumber = Console.ReadLine() ?? "";

                    // Cancels the loan request if the user enters 0
                    if (accountNumber == "0")
                    {
                        continue;
                    }

                    // Starts the process of creating the loan
                    MakeLoan(loan, accountNumber);

                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    // Closes the loan menu and returns to the main menu
                    showLoanMenu = false;
                }
                Console.Clear();
            }
        }

        // Creates a loan and deposits the money into the selected account
        public void MakeLoan(decimal loan, string accountNumber)
        {
            BankAccount? selectedAccount = null;

            // Searches for the account matching the entered account number
            foreach (BankAccount account in _customer.CheckingAccounts)
            {
                if (account.BankAccountNumber.ToString() == accountNumber)
                {
                    selectedAccount = account;
                    break;
                }
            }

            // Stops if the account number does not exist
            if (selectedAccount == null)
            {
                Console.WriteLine("Kontot finns inte.");
                return;
            }

            Console.Write("Ange återbetalningstid i år (1–10): ");
            int loanYears = 0;

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

            // Creates a new loan object containing all loan information
            Loan newLoan = new Loan(
                loan,
                loanYears,
                totalLoan,
                interestRate
            );

            // Adds the new loan to the customer's list of loans
            _customer.Loans.Add(newLoan);

            // Displays information about the approved loan
            Console.Clear();
            Console.WriteLine($"Du valde kontot: {selectedAccount.BankAccountName}");
            Console.WriteLine($"Lånebelopp: {loan:N2} kr");// skriver ut loan med 2 decimaler
            Console.WriteLine($"Kreditvärdighet: {_customer.CreditRating}");
            Console.WriteLine($"Grundränta: {LoanInterestRules.GetBaseInterest(_customer.CreditRating):P2}");// skriver ut med procent tecken 
            Console.WriteLine($"Ränta justering för lånetid: {LoanInterestRules.GetTermInterest(loanYears):P2}");
            Console.WriteLine($"Slutlig årsränta: {interestRate:P2}");
            Console.WriteLine($"Återbetalningstid: {loanYears} år");
            Console.WriteLine($"Total räntekostnad: {totalLoan - loan:N2} kr");
            Console.WriteLine($"Total återbetalning: {totalLoan:N2} kr");
        }

        // Displays all checking accounts belonging to the customer
        public void PrintAccounts()
        {
            Console.WriteLine("\n----- Dina konton -----\n");
            foreach (BankAccount account in _customer.CheckingAccounts)
            {
                Console.WriteLine($"Namn: {account.BankAccountName}");
                Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                Console.WriteLine($"KontoSaldo: {account.AccountBalance}");
                Console.WriteLine($"Valuta: {account.Currency}\n");
            }
        }

        // Checks if the customer can borrow up to five times their total balance
        public bool CheckCustomerMoney(decimal loan)
        {
            decimal moneyInAccounts = 0;

            // Adds all checking account balances after converting them to SEK
            foreach (BankAccount account in _customer.CheckingAccounts)
            {
                decimal exchangeRate = ExchangeRates.GetRate(account.Currency);
                moneyInAccounts += account.AccountBalance * exchangeRate;
            }

            foreach (BankAccount account in _customer.SavingsAccounts)
            {
                decimal exchangeRate = ExchangeRates.GetRate(account.Currency);
                moneyInAccounts += account.AccountBalance * exchangeRate;
            }

            // Rejects the loan if it exceeds five times the customer's total money
            if (loan > moneyInAccounts * 5)
            {
                Console.WriteLine(
                    $"Du kan inte låna så mycket, max 5 gånger det du har i vår bank.\n" +
                    $"Du har totalt: {moneyInAccounts:N2} SEK");

                return false;
            }

            return true;
        }


        // Calculates the total repayment amount including interest for all years
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

        // Displays all loans belonging to the currently logged-in customer
        public void PrintLoans() 
        {
            if (_customer.Loans.Count == 0)
            {
                Console.WriteLine("Du har inga lån.");
                return;
            }

            Console.WriteLine("-----Dina lån-----");
            int i = 1;
            // Goes through the customer's loans and displays their information
            foreach (Loan loan in _customer.Loans)
            {
                Console.WriteLine($"[{i}].");
                Console.WriteLine($"Lånebelopp: {loan.LoanAmount} kr");
                Console.WriteLine($"Årsränta: {loan._interestRate:P0}");
                Console.WriteLine($"Återbetalningstid: {loan.LoanYears} år");
                Console.WriteLine($"Total skuld: {loan.TotalLoan:N3} kr\n");
             
                i++;
            }
        }

        // Converts a loan amount from SEK to the selected account's currency
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
