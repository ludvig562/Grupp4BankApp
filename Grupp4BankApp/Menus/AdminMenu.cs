using Grupp4BankApp.Manager;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;

namespace Grupp4BankApp.Menus
{
    internal class AdminMenu
    {
        public static AuthenticationService Auth = new();

        public AdminMenu( )
        {
        }

        public void Display()
        {
            Console.Clear();
            RunAdminMenu();
        }

        //method for admin menu (once logged in)
        public static void RunAdminMenu()
        {
            bool adminMenuRunning = true;
            while (adminMenuRunning)
            {
                Console.WriteLine("Vad vill du göra?\n" +
                    "[1] Skapa ett nytt användarekonto\n" +
                    "[2] Uppdatera växelkurs\n" +
                    "[3] Ändra ränta regler\n" +
                    "[4] Ändra kundens kreditvärdighet\n" +
                    "[5] Avsluta meny/logga ut");

                //check if user input is a number
                if (int.TryParse(Console.ReadLine(), out int adminMenuChoice)) //if user input is a number
                {
                    Console.Clear();
                    switch (adminMenuChoice)
                    {
                        case 1: //create a new user account
                            Admin.CreateCustomer();
                            break;

                        case 2: // Download and approve exchange rates
                            Admin.UpdateExchangeRates();
                            break;
                        case 3:
                            Admin.UppdateLoanInterest();
                            break;
                        case 4:
                            Admin.UpdateCustomerCreditRating();
                            break;
                        case 5: //end admin menu
                            Console.Clear();
                            Console.WriteLine("Du har loggats ut ");
                            Auth.Logout();
                            return;
                        default: //if user input is a number but not between 1 and 3
                            Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 3!\n");
                            break;
                    }
                }
                else //if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra!\n");
                }
            }
        }
    }
}
