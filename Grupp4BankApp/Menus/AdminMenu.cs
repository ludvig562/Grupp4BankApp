using Grupp4BankApp.Users;

namespace Grupp4BankApp.Menus
{
    internal class AdminMenu
    {

        public AdminMenu( )
        {
        }

        public void Display()
        {
            Console.WriteLine("This is Admin Menu");
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
                    "[3] Avsluta meny/logga ut");

                //check if user input is a number
                if (int.TryParse(Console.ReadLine(), out int adminMenuChoice)) //if user input is a number
                {
                    Console.Clear();
                    switch (adminMenuChoice)
                    {
                        case 1: //create a new user account
                            CreateCustomer();
                            break;

                        case 2: //update currency exchange
                            break;

                        case 3: //end admin menu
                            Console.WriteLine("Du har nu loggats ut.");
                            adminMenuRunning = false;
                            break;

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

        //method for creating a new user
        public static void CreateCustomer()
        {
            bool customerCreated = false;

            while (!customerCreated)
            {
                //ask for customer info
                Console.WriteLine("Vad är kundens ID?");
                int newCustomerID = int.Parse(Console.ReadLine());
                Console.WriteLine("Kundens användarnamn");
                string newCustomerUsername = Console.ReadLine();
                Console.WriteLine("Vad heter kunden?");
                string newCustomerName = Console.ReadLine();
                Console.WriteLine("Vad är kundens mejladdress?");
                string newCustomerMailAddress = Console.ReadLine();
                Console.WriteLine("Vad är kundens telefonnummer?");
                string newCustomerPhoneNumber = Console.ReadLine();
                Console.WriteLine("Vad blir nya kundens lösenord?");
                string newCustomerPassword = Console.ReadLine();

                Console.WriteLine($"Vill du skapa ett nytt användarkonto med följande information?\n" +
                    $"ID: {newCustomerID}\n" +
                    $"Namn: {newCustomerName}" +
                    $"E-mail: {newCustomerMailAddress}, " +
                    $"Telefonnummer: {newCustomerPhoneNumber}" +
                    $"Lösenord: {newCustomerPassword}\n" +
                    $"[1] Ja, det vill jag\n" +
                    $"[2] Nej, jag vill ändra informationen");

                if (int.TryParse(Console.ReadLine(), out int adminMenuAddCustomer)) //if user input is a number
                {
                    switch (adminMenuAddCustomer)
                    {
                        case 1: //create a new user account
                            //create new Customer account
                            //save customer info in dictionary?!
                            Customer newUser = new Customer(newCustomerUsername, newCustomerName, newCustomerMailAddress, newCustomerPassword);
                            break;
                        case 2: //if user wishes to change info
                            Console.WriteLine("\nMata in informationen igen: ");
                            break;
                        default: // if user input is a number but not 1 or 2
                            Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 2!\n");
                            break;
                    }
                }
                else // if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra.\n");
                }
            }
        }
    }
}
