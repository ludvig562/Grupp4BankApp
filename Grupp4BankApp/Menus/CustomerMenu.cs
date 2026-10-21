using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using System.Security.Cryptography.X509Certificates;
namespace Grupp4BankApp.Menus
{
    internal class CustomerMenu
    {
        List<char> myAnimation = new List<char>();
        public CustomerMenu()
        {
            myAnimation.DefaultIfEmpty();
        }
        public void LogInEvent()
        {
            string loadingIn = "Logging in";
            string dots = ". . .";


            for (int j = 0; j < 3; j++)
            {
                Console.Clear();

                Console.WriteLine(FiggleFonts.Larry3d.Render(loadingIn));
                Thread.Sleep(400);
                for (int i = 0; i < dots.Length; i++)
                {
                    Console.Clear();

                    string temp = dots.Substring(0, i + 1);
                    //Larry3d
                    //Standard

                    Console.WriteLine(FiggleFonts.Larry3d.Render(loadingIn + " " + temp));
                    //  Console.WriteLine(FiggleFonts.Standard.Render(loadingIn + " " + temp));

                    Thread.Sleep(200);
                }
            }

        }
        public void Display()
        {
            LogInEvent();
            Console.Clear();

            ShowCustomerMenu();
        }

        public void ShowCustomerMenu()
        {

            while (true)
            {
                Console.WriteLine("Vad vill du göra?\n" +
                    "[1] Lista alla konto\n" +
                    "[2] Öppna nya konto\n" +
                    "[3] Sätt in pengar\n" +
                    "[4] Överför pengar\n" +
                    "[5] Låna Pengar\n" +
                    "[6] Transaktioner\n" +
                    "[7] Logga ut\n");
                if (int.TryParse(Console.ReadLine(), out int customerMenuChoice))
                {
                    switch (customerMenuChoice)
                    {
                        case 1:
                            break;
                        case 2:
                            //check if User is a Customer - call getuser method from authenticationservice class
                            if (AuthenticationService.GetLoggedInUser() is Customer customer)
                            {
                                customer.OpenBankAccount(); //open bank account for the logged in Customer
                            }
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Du måste skriva mellan 1 - 7");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Du måste skriva en siffra");
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
            //confirmation message stating a new account currency has been chosen
            Console.WriteLine($"Du har valt att ha ditt konto i {chosenCurrency}");
            return chosenCurrency;
        }

        //method for writing out a confirmation message when a new bankacount has been created
        public void WriteConfirmationMessage(BankAccount account, Enums.CurrencyTypes chosenCurrency)
        {
            Console.WriteLine($"Du har nu skapat ett konto:\n" +
                                $"Namn: {account.BankAccountName}\n" +
                                $"Valuta: {chosenCurrency}\n" +
                                $"Bankkontonummer: {account.BankAccountNumber}\n");
        }
    }
}