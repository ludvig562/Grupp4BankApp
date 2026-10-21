using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Admin : User
    {
        //constructor, child from User class
        public Admin(int userID, string username, string name, string email,  string password) : base(userID, username, name, email,  password)
        {
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

                //write out to double check if info is correct ?
                //add if/else case?
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
                            Customer newCustomer = new Customer(newCustomerID, newCustomerUsername, newCustomerName, newCustomerMailAddress,  newCustomerPassword);
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

