using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using System.Security.Cryptography.X509Certificates;
namespace Grupp4BankApp.Menus
{
    internal class CustomerMenu
    {
        AuthenticationService Auth = new();
        Loan loans = new();
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
                    "[7] Logga ut\n");
                if (int.TryParse(Console.ReadLine(), out int customerMenuChoice))
                {
                    switch (customerMenuChoice)
                    {
                        case 1:
                            break;
                        case 2:
                            Customer? currentUser = (Customer)AuthenticationService.CurrentUser; //convert user to customer type
                            currentUser.OpenBankAccount(); //customer opens bank account
                            break;
                        case 3:
                            break;
                        case 4:
                            Console.Clear();
                            Transaction();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("vad vill du göra?\n" +
                                "[1] Lista över dina lån\n" +
                                "[2] Låna Pengar\n" +
                                "[3] Gå tillbaka\n");
                            int loanChoise = 0;
                            while (!int.TryParse(Console.ReadLine(), out loanChoise) || loanChoise < 1 || loanChoise > 3)
                            {
                                Console.WriteLine("Du måste ange ett giltit tal!");
                            }
                            if (loanChoise == 1)
                            {
                                loans.PrintLoans();
                            }
                            else if (loanChoise == 2)
                            {
                                Console.Write("Hur mycket vill du låna?: ");
                                int loan = 0;
                                while (!int.TryParse(Console.ReadLine(), out loan))
                                {
                                    Console.WriteLine("Du måste ange ett giltit antal!");
                                }
                                loans.PrintAccounts();
                                Console.WriteLine("Vilket konto ska pengarna sättas in i?: ");
                                int accountChoise = 0;
                                while (!int.TryParse(Console.ReadLine(), out accountChoise))
                                {
                                    Console.WriteLine("Du måste ange ett giltit tal!");
                                }
                                if (loans.CheckCustomerMoney(loan))
                                {
                                    loans.MakeLoan(loan, accountChoise);
                                }
                            }
                            break;
                        case 6:
                            break;
                        case 7:
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

        public void Transaction()
        {
            if (AuthenticationService.CurrentUser is Customer currentUser)
            {
                foreach (var item in currentUser.checkingAccounts.Concat(currentUser.savingsAccounts))
                {
                    Console.WriteLine($"{item}\n");
                }

                Console.Write("Från: ");
                string? from = Console.ReadLine() ?? "";

                BankAccount? FromAccount = currentUser.checkingAccounts.
                    Concat(currentUser.savingsAccounts).
                    FirstOrDefault(a => a.BankAccountName == from);

                if (FromAccount == null)
                {
                    Console.WriteLine("Konto finns inte!");
                    return;
                }

                Console.Write("Till: ");
                string? to = Console.ReadLine() ?? "";

                BankAccount? toAccount = currentUser.checkingAccounts.
                    Concat(currentUser.savingsAccounts).
                    FirstOrDefault(a => a.BankAccountName == to);

                if (toAccount == null)
                {
                    Console.WriteLine("Konto finns inte!");
                    return;
                }

                Factor(FromAccount, toAccount);



            }

        }

        //Flytta till transaction klassen sen

        public void Factor(BankAccount fromACC, BankAccount toACC)
        {

            Console.Write("Belopp: ");

            if (!int.TryParse(Console.ReadLine(), out int belopp))
            {
                Console.WriteLine("Nummer tack");
                return;
            }

            if (fromACC.AccountBalance >= belopp)
            {
                fromACC.AccountBalance -= belopp;
            }
            else 
            {
                Console.WriteLine("Du har inte tillräckligt med pengar på kontot.");
            }
            
            toACC.AccountBalance += belopp;

        }
    }
}