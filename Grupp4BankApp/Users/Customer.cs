using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //properties: lists for bankaccounts
        public List<BankAccount> CheckingAccounts { get; set; }
        public List<BankAccount> SavingsAccounts { get; set; }

        public List<Loan> Loans { get; set; }

        //constructor, child from User class
        public Customer(string username, string name, string email, string password) : base(username, name, email, password)
        {
            Role = Enums.UserRole.customer; //automatically add the role of customer
            CheckingAccounts = new List<BankAccount>();
            SavingsAccounts = new List<BankAccount>();

            Loans = new List<Loan>();
        }

        //method for customer to update their own info
        public void UpdateCustomerInformation()
        {
            bool informationUpdated = false;

            //ask customer which info they wish to update
            Console.WriteLine("Vilken information vill du uppdatera?\n" +
                "[1] Emailaddress\n" +
                "[2] Lösenord\n");

            while (!informationUpdated)
            {
                if (int.TryParse(Console.ReadLine(), out int updateInfoMenuChoice)) //check if user input is a number
                {
                    switch (updateInfoMenuChoice)
                    {
                        case 1: //update email address
                            UpdateEmail();
                            informationUpdated = true;
                            break;
                        case 2: //update password
                            UpdatePassword();
                            informationUpdated = true;
                            break;
                        default:
                            Console.WriteLine("Oops. Du måste välja en siffra mellan 1 och 2.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Du måste ange en siffra mellan 1 och 2.");
                }
            }
        }

        //method for updating customer email address
        public void UpdateEmail()
        {
            Console.WriteLine($"Ditt nuvarande mejladdress är: {this.Email}\n");
            string newEmailAddress = Admin.GetMailAddress();
            this.Email = newEmailAddress;
            Console.WriteLine($"Du har nu bytt till följande mejladdress: {this.Email}");
        }

        //method for updating customer password
        public void UpdatePassword()
        {
            string newPassword = Admin.GetPassword();
            this.Password = PasswordHasher.HashPassword(newPassword);
            Console.WriteLine("Du har nu bytt ditt lösenord.");
        }


        // Displays all the customer's bank accounts and their balances
        public void ShowBankAccounts()
        {
            Console.Clear();
            Console.WriteLine("========== MINA BANKKONTON ==========\n");

            // Checks whether the customer has no bank accounts
            if (CheckingAccounts.Count == 0 && SavingsAccounts.Count == 0)
            {
                Console.WriteLine("Du har inga bankkonton ännu.");
            }
            else
            {
                // Displays all checking accounts
                if (CheckingAccounts.Count > 0)
                {
                    Console.WriteLine("--- VANLIGA KONTON ---\n");

                    foreach (BankAccount account in CheckingAccounts)
                    {
                        Console.WriteLine($"Namn: {account.BankAccountName}");
                        Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                        Console.WriteLine(
                            $"Saldo: {account.AccountBalance:N2} {account.Currency}");
                        Console.WriteLine("------------------------------------");
                    }
                }

                // Displays all savings accounts
                if (SavingsAccounts.Count > 0)
                {
                    Console.WriteLine("\n--- SPARKONTON ---\n");

                    foreach (BankAccount account in SavingsAccounts)
                    {
                        Console.WriteLine($"Namn: {account.BankAccountName}");
                        Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                        Console.WriteLine(
                            $"Saldo: {account.AccountBalance:N2} {account.Currency}");
                        Console.WriteLine("------------------------------------\n\n");
                    }
                }
            }

            // Waits for the customer to press Enter before returning to the menu
            //Console.WriteLine("\nTryck på Enter för att återgå till menyn.");
            //Console.ReadKey();
            //Console.Clear();
        }

        //method to open new bankaccount
        public void OpenBankAccount()
        {
            // Which type of bankaccount do they want (check or savings)
            Console.WriteLine("Vilken typ av konto vill du öppna?");
            Console.WriteLine("[1] Vanligt konto");
            Console.WriteLine("[2] Sparkonto");
            bool accountTypeChosen = false;

            while (!accountTypeChosen)
            {
                if (int.TryParse(Console.ReadLine(), out int accountType))
                {
                    // Create a BankAccount object of the correct type
                    switch (accountType)
                    {
                        case 1: //open checking account
                            OpenCheckingAccount(); //method to open checking account
                            accountTypeChosen = true; //end while loop
                            break;

                        case 2: //open savings account
                            OpenSavingsAccount(); //method to open savings account
                            accountTypeChosen = true; //end while loop
                            break;

                        default: //if user input is a number but not 1 or 2
                            Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 2!\n");
                            break;
                    }
                }
                else //if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra!");
                }
            }
        }

        //method for opening checking account
        public void OpenCheckingAccount()
        {
            Enums.CurrencyTypes chosenCurrency = ChooseCurrency(); //ask for currency
            string bankAccountName = ChooseBankAccountName(); //ask for name of bank account

            //create new bank account
            BankAccount newCheckingAccount = new BankAccount(Enums.AccountType.checkingsAccount, bankAccountName, chosenCurrency);
            //add new bank account to customer's bank account list
            this.CheckingAccounts.Add(newCheckingAccount);

            WriteConfirmationMessage(newCheckingAccount); //write confirmation message
        }

        //method for opening savings account
        public void OpenSavingsAccount()
        {
            Enums.CurrencyTypes chosenCurrency = ChooseCurrency(); //ask for currency
            string bankAccountName = ChooseBankAccountName(); //ask for name of bank account

            //create new bank account
            BankAccount newSavingsAccount = new BankAccount(Enums.AccountType.savingsAccount, bankAccountName, chosenCurrency);
            //add new bank account to customer's bank account list
            this.SavingsAccounts.Add(newSavingsAccount);

            WriteConfirmationMessage(newSavingsAccount); //write confirmation message
        }

        //method for finding the bank account chosen by the customer
        private BankAccount FindBankAccount(string accountName)
        {
            foreach (BankAccount account in CheckingAccounts)
            {
                if (account.BankAccountName == accountName)
                {
                    return account;
                }
            }
            foreach (BankAccount account in SavingsAccounts)
            {
                if (account.BankAccountName == accountName)
                {
                    return account;
                }
            }
            return null;
        }

        //method for depositing money into account
        public void DepositMoney()
        {
            bool depositCompleted = false;
            while (!depositCompleted)
            {
                ShowBankAccounts(); //show overview of all their accounts
                Console.WriteLine("På vilket konto vill du sätta in pengar?");
                string accountChosen = Console.ReadLine();
                BankAccount account = FindBankAccount(accountChosen); //find the chosen bank account
                if (account == null)
                {
                    Console.WriteLine("Det finns inget konto med det namnet.");
                    continue;
                }
                Console.WriteLine($"Hur mycket pengar vill du sätta i kontot {account.BankAccountName}?");
                //check if answer if a number and more than zero
                if (decimal.TryParse(Console.ReadLine(), out decimal depositMoneyAmount)
                    && depositMoneyAmount > 0)
                {
                    account.AccountBalance += depositMoneyAmount;
                    Console.WriteLine($"\n{depositMoneyAmount} {account.Currency} har satts in på kontot {account.BankAccountName}.\n");

                    if (account.AccountType == Enums.AccountType.savingsAccount)
                    {
                        decimal yearlyInterest = account.CalculateInterest();
                        Console.WriteLine($"Din beräknade årsränta är: {yearlyInterest} {account.Currency}.\n");
                    }
                    depositCompleted = true;
                }
                else //if answer is not a number or is below zero
                {
                    Console.WriteLine("Ogiltigt belopp. Vänligen välj ett belopp större än noll.");
                }
            }
        }


        //method for getting the bank account name
        public static string ChooseBankAccountName()
        {
            Console.WriteLine("Vad ska ditt nya konto heta?");
            string bankAccountName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(bankAccountName))
            {
                return bankAccountName;
            }
            else //if user didn't write a name for the account
            {
                do
                {
                    Console.WriteLine("Vänligen skriv ett namn för ditt konto.");
                    bankAccountName = Console.ReadLine();
                }
                while (string.IsNullOrWhiteSpace(bankAccountName));
                return bankAccountName;
            }
        }

        //method for choosing bank account currency
        public static Enums.CurrencyTypes ChooseCurrency()
        {
            Console.WriteLine("Vilken valuta vill du öppna kontot i?");
            Console.WriteLine("[1] SEK");
            Console.WriteLine("[2] EUR");
            Console.WriteLine("[3] USD");

            //create variable for chosen currency, with standard value SEK
            Enums.CurrencyTypes chosenCurrency = Enums.CurrencyTypes.SEK;

            bool currencyChosen = false;
            while (!currencyChosen)
            {
                if (int.TryParse(Console.ReadLine(), out int currencyChoice))
                {
                    //add currency from enum to chosen currency
                    switch (currencyChoice)
                    {
                        case 1:
                            chosenCurrency = Enums.CurrencyTypes.SEK;
                            currencyChosen = true;
                            break;
                        case 2:
                            chosenCurrency = Enums.CurrencyTypes.EUR;
                            currencyChosen = true;
                            break;
                        case 3:
                            chosenCurrency = Enums.CurrencyTypes.USD;
                            currencyChosen = true;
                            break;
                        default: //if user input is a number but not 1, 2 or 3
                            Console.WriteLine("Ogiltigt val. Välj en siffra mellan 1 och 3!");
                            break;
                    }
                }
                else //if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra!");
                }
            }
            return chosenCurrency;
        }

        //method for writing out a confirmation message when a new bankacount has been created
        public static void WriteConfirmationMessage(BankAccount account)
        {
            Console.WriteLine($"\nDu har nu skapat ett konto:\n" +
                                $"Namn: {account.BankAccountName}\n" +
                                $"Valuta: {account.Currency}\n" +
                                $"Bankkontonummer: {account.BankAccountNumber}\n" +
                                $"Konto saldo: {account.AccountBalance}\n");

            if (account.AccountType == Enums.AccountType.savingsAccount)
            {
                decimal yearlyInterest = account.CalculateInterest();
                Console.WriteLine($"Din beräknad årsränta är: {yearlyInterest} {account.Currency}.\n");
            }
        }


    }
}