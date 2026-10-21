using Figgle.Fonts;
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
                Console.WriteLine("1.  Lista alla konto\n" +
                    "2. Öppna nya konto\n" +
                    "3. Sätt in pengar\n" +
                    "4. Överför pengar\n" +
                    "5. Låna Pengar\n" +
                    "6. Transaktioner\n" +
                    "7. Logga ut\n");
                if (int.TryParse(Console.ReadLine(), out int customerMenuChoice))
                {
                    switch (customerMenuChoice)
                    {
                        case 1:
                            break;
                        case 2:
                            OpenBankAccount();
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

        public void OpenBankAccount()
        {
            Console.WriteLine("Vilken typ av konto vill du öppna?");
            Console.WriteLine("[1] Vanligt konto");
            Console.WriteLine("[2] Sparkonto");

        }


    }
}
