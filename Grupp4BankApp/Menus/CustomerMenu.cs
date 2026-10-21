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

        //public static int OpenBankAccount()
        //{
            // Which type of bankaccount do they want (check or savings)
            //Console.WriteLine("Vilken typ av konto vill du öppna?");
            //Console.WriteLine("[1] Vanligt konto");
            //Console.WriteLine("[2] Sparkonto");
            //int accountType;
            //int.TryParse(Console.ReadLine(), out accountType);
            //if (accountType == 1 || accountType == 2)
            //{
            //    return accountType;
            //}
            //else
            //{
            //    do
            //    {
            //        Console.WriteLine("Välj en siffra mellan 1 och 2!");
            //    }
            //    while (!int.TryParse(Console.ReadLine(), out accountType) || (accountType < 1 || accountType > 2));
            //    return accountType;
            //}
    //    }
    //}


    }
}
