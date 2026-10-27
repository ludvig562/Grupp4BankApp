using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Transaction;
using Grupp4BankApp.Users;
using Spectre.Console;
using System.Security.Cryptography.X509Certificates;
namespace Grupp4BankApp.Menus
{
    internal class CustomerMenu
    {
        private Customer? currentUser;
        AuthenticationService Auth = new();
        Loan loans = new();
        TransactionSystem transactionSystem = new TransactionSystem();
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
            // LogInEvent();
            Console.Clear();

            RunCustomerMenu();
        }

        public void RunCustomerMenu()
        {
            currentUser = AuthenticationService.CurrentUser as Customer;

            bool customerMenuRunning = true;

            while (customerMenuRunning)
            {
                var picked = AnsiConsole.Prompt(new SelectionPrompt<string>()
                 .Title("vad vill du [skyblue1]göra[/]?")
                 .AddChoices("Lista alla konto", "Öppna nya konto", "Sätt in pengar", "Överför pengar", "Låna pengar","Transaktioner","Logga ut"));
              
                    switch (picked)
                    {
                        case "Lista alla konto":
                            // Gets the logged-in customer and displays their bank accounts
                            if (AuthenticationService.CurrentUser is Customer customer)
                            {
                                customer.ShowBankAccounts();
                            }
                            else
                            {
                                Console.WriteLine("Ingen kund är inloggad.");
                            }

                            break;
                        case "Öppna nya konto":
                            currentUser.OpenBankAccount(); //customer opens bank account
                            break;
                        case "Sätt in pengar":
                            break;
                        case "Överför pengar":
                            Console.Clear();
                            transactionSystem.TransferMoney();
                            break;
                        case "Låna pengar":
                            loans.ShowLoanMenu(); // opens loan menu
                            break;
                        case "Transaktioner":
                            Console.Clear();
                            transactionSystem.TransactionLog();
                            break;
                        case "Logga ut":
                            Console.Clear();
                            Console.WriteLine("Du har loggats ut ");
                            Auth.Logout();
                            return;
                        default:
                            Console.WriteLine("Du måste skriva en siffra mellan 1 - 7");
                            break;
                    }
             
            }
        }


    }
}