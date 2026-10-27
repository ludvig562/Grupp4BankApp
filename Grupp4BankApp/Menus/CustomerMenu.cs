using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using System.Security.Cryptography.X509Certificates;
using Grupp4BankApp.Transaction;
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
                Console.WriteLine("Vad vill du göra?\n" +
                    "[1] Lista alla konto\n" +
                    "[2] Öppna nya konto\n" +
                    "[3] Sätt in pengar\n" +
                    "[4] Överför pengar\n" +
                    "[5] Låna Pengar\n" +
                    "[6] Transaktioner\n" +
                    "[7] Uppdatera dina personliga uppgifter\n" +
                    "[8] Logga ut\n");
                if (int.TryParse(Console.ReadLine(), out int customerMenuChoice))
                {
                    switch (customerMenuChoice)
                    {
                        case 1:
                            Console.Clear();
                            currentUser.ShowBankAccounts();
                            break;
                        case 2:
                            Console.Clear();
                            currentUser.OpenBankAccount(); //customer opens bank account
                            break;
                        case 3:
                            Console.Clear();
                            currentUser.DepositMoney();
                            break;
                        case 4:
                            Console.Clear();
                            transactionSystem.TransferMoney();
                            break;
                        case 5:
                            Console.Clear();
                            loans.ShowLoanMenu(); // opens loan menu
                            break;
                        case 6:
                            Console.Clear();
                            transactionSystem.TransactionLog();
                            break;
                        case 7:
                            Console.Clear();

                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("Du har loggats ut ");
                            Auth.Logout();
                            return;
                        default:
                            Console.WriteLine("Du måste skriva en siffra mellan 1 - 7");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Du måste skriva en siffra");
                }
            }
        }
    }
}