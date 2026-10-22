using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Admin : User
    {
        //constructor, child from User class
        public Admin(string username, string name, string email, string password) : base(username, name, email, password)
        {
            Role = Enums.UserRole.admin; //automatically add the role of admin
        }


        public void GetCustomerInfo()
        {
            bool customerCreated = false;

            while (!customerCreated)
            {
                //ask for customer info
                Console.WriteLine("Kundens användarnamn");
                string newCustomerUsername = Console.ReadLine();
                Console.WriteLine("Vad heter kunden?");
                string newCustomerName = Console.ReadLine();
                Console.WriteLine("Vad är kundens mejladdress?");
                string newCustomerMailAddress = Console.ReadLine();
                Console.WriteLine("Vad blir nya kundens lösenord?");
                string newCustomerPassword = Console.ReadLine();

                Console.WriteLine($"Vill du skapa ett nytt användarkonto med följande information?\n" +
                    $"Namn: {newCustomerName}" +
                    $"E-mail: {newCustomerMailAddress}, " +
                    $"Lösenord: {newCustomerPassword}\n" +
                    $"[1] Ja, det vill jag\n" +
                    $"[2] Nej, jag vill ändra informationen");
            }
        }

        //method for creating a new user
        public static void CreateCustomer()
        {

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

