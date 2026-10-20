using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //constructor, child from User class
        public Customer(int userID, string name, string email, string phonenumber, string password) : base(userID, name, email, phonenumber, password)
        {
        }

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
                            ChooseCurrency();
                            break;
                        case 2: //open savings account
                            ChooseCurrency();
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

        public static void ChooseCurrency()
        {
            // Ask user which currency they want the account to be in.
            Console.WriteLine("Vilken valuta vill du öppna kontot i?");
            Console.WriteLine("[1] SEK");
            Console.WriteLine("[2] EUR");
            Console.WriteLine("[3] USD");

            //create variable for chosen currency, with standard value SEK
            Currency.CurrencyTypes chosenCurrency = Currency.CurrencyTypes.SEK;

            bool currencyChosen = false;
            while (!currencyChosen)
            {
                if (int.TryParse(Console.ReadLine(), out int currencyChoice))
                {
                    //add currency from enum to chosen currency
                    switch (currencyChoice)
                    {
                        case 1:
                            chosenCurrency = Currency.CurrencyTypes.SEK;
                            currencyChosen = true;
                            break;
                        case 2:
                            chosenCurrency = Currency.CurrencyTypes.EUR;
                            currencyChosen = true;
                            break;
                        case 3:
                            chosenCurrency = Currency.CurrencyTypes.USD;
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
        }
    }
}
