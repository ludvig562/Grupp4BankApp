using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //properties: lists for bankaccounts
        public List<BankAccount> checkingAccounts { get; set; }
        public List<BankAccount> savingsAccounts { get; set; }

        public List<Loan> Loans { get; set; }

        //constructor, child from User class
        public Customer(string username, string name, string email, string password) : base(username, name, email, password)
        {
            Role = Enums.UserRole.customer; //automatically add the role of customer
            checkingAccounts = new List<BankAccount>();
            savingsAccounts = new List<BankAccount>();

            Loans = new List<Loan>();
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
            this.checkingAccounts.Add(newCheckingAccount); 

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
            this.savingsAccounts.Add(newSavingsAccount);

            WriteConfirmationMessage(newSavingsAccount); //write confirmation message
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
        }
    }
}