using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //constructor, child from User class
        public Customer(int userID, string username, string name, string email, string password) : base(userID, username, name, email, password)
        {
        }

        //list for checking accounts
        //make list related to customer only rather than one list for all customers?!
        public List<BankAccount> checkingAccounts = new List<BankAccount>();

        //list for saving accounts
        //make list related to customer only rather than one list for all customers?!
        public List<BankAccount> savingsAccounts = new List<BankAccount>();

        //add addbankaccount method here, that receives info from other methods in customermenu class?!

        //method to open new bankaccount (that gets saved in list in bankaccount.cs)
        public static void OpenBankAccount()
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
                            //choose currency + name via method
                            CurrencyTypes chosenCurrency = ChooseCurrency();
                            string bankAccountName = ChooseBankAccountName();
                            //create bank account + add to checkingaccounts list
                            BankAccount newCheckingAccount = new BankAccount("Svea", bankAccountName, chosenCurrency);
                            checkingAccounts.Add(newCheckingAccount);
                            //confirmation message that account has been created
                            Console.WriteLine($"Du har nu skapat ett konto i {chosenCurrency}. " +
                                $"Ditt bankkontonummer är: {newCheckingAccount.BankAccountNumber}");
                            accountTypeChosen = true; //end while loop
                            break;

                        case 2: //open savings account
                            //choose currency + name via method
                            chosenCurrency = ChooseCurrency();
                            bankAccountName = ChooseBankAccountName();
                            //create bank account + add to savingsaccounts list
                            BankAccount newSavingsAccount = new BankAccount("Svea", bankAccountName, chosenCurrency);
                            savingsAccounts.Add(newSavingsAccount);
                            //confirmation message that account has been created
                            Console.WriteLine($"Du har nu skapat ett sparkonto i {chosenCurrency}. " +
                                $"Ditt bankkontonummer är: {newSavingsAccount.BankAccountNumber}");
                            accountTypeChosen = true;
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

        //method for getting the bank account name
        public static string ChooseBankAccountName()
        {
            Console.WriteLine("Vad ska ditt nya konto heta?");
            return Console.ReadLine();
        }

        //method for choosing bank account currency
        public static CurrencyTypes ChooseCurrency()
        {
            Console.WriteLine("Vilken valuta vill du öppna kontot i?");
            Console.WriteLine("[1] SEK");
            Console.WriteLine("[2] EUR");
            Console.WriteLine("[3] USD");

            //create variable for chosen currency, with standard value SEK
            CurrencyTypes chosenCurrency = CurrencyTypes.SEK;

            bool currencyChosen = false;
            while (!currencyChosen)
            {
                if (int.TryParse(Console.ReadLine(), out int currencyChoice))
                {
                    //add currency from enum to chosen currency
                    switch (currencyChoice)
                    {
                        case 1:
                            chosenCurrency = CurrencyTypes.SEK;
                            currencyChosen = true;
                            break;
                        case 2:
                            chosenCurrency = CurrencyTypes.EUR;
                            currencyChosen = true;
                            break;
                        case 3:
                            chosenCurrency = CurrencyTypes.USD;
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
            //confirmation message stating a new account currency has been chosen
            Console.WriteLine($"Du har valt att ha ditt konto i {chosenCurrency}");
            return chosenCurrency;
        }
    }
}
