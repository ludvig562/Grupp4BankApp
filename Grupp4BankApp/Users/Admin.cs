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

        //method for creating a new user
        public static void CreateCustomer()
        {
            bool customerCreated = false;

            while (!customerCreated)
            {
                //ask for customer info
                Console.WriteLine("Vad är kundens användarnamn?");
                string newCustomerUsername = Console.ReadLine();

                /*check if username already exists in dictionary via ContainsKey instead of TryGetValue
                because here we only need to check if the name already exists in the dictionary,
                we don't need to get the entire user*/
                if (User.users.ContainsKey(newCustomerUsername))
                {
                    Console.WriteLine("Användarnamn finns redan. Välj ett annat användarnamn.");
                    continue; //breaking the if loop but not the while loop, so admin can correct username
                }

                Console.WriteLine("Vad heter kunden?");
                string newCustomerName = Console.ReadLine();
                Console.WriteLine("Vad är kundens mejladdress?");
                string newCustomerMailAddress = Console.ReadLine();
                Console.WriteLine("Vad blir nya kundens lösenord?");
                string newCustomerPassword = Console.ReadLine();
                Console.Clear();

                Console.WriteLine($"Vill du skapa ett nytt användarkonto med följande information?\n\n" +
                    $"Namn: {newCustomerName}\n" +
                    $"Användarnamn: {newCustomerUsername}\n" +
                    $"E-mail: {newCustomerMailAddress}\n" +
                    $"Lösenord: {newCustomerPassword}\n\n" +
                    $"[1] Ja, det vill jag\n" +
                    $"[2] Nej, jag vill ändra informationen");

                if (int.TryParse(Console.ReadLine(), out int adminMenuAddCustomer)) //if user input is a number
                {
                    switch (adminMenuAddCustomer)
                    {
                        case 1: //create a new user account
                            Customer newCustomer = new Customer(newCustomerUsername, newCustomerName, newCustomerMailAddress, newCustomerPassword);
                            User.users.Add(newCustomerUsername, newCustomer);
                            Console.WriteLine($"Ny användare {newCustomerName} har skapats.\n");
                            customerCreated = true;
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

