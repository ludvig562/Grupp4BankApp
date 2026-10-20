using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Admin : User
    {
        //constructor, child from User class
        public Admin(int userID, string name, string email, string phonenumber, string password) : base(userID, name, email, phonenumber, password)
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
                int.TryParse(Console.ReadLine(), out int newCustomerID);
                Console.WriteLine("Vad heter kunden?");
                string newCustomerName = Console.ReadLine();
                Console.WriteLine("Vad är kundens mejladdress?");
                string newCustomerMailAddress = Console.ReadLine();
                Console.WriteLine("Vad är kundens telefonnummer?");
                string newCustomerPhoneNumber = Console.ReadLine();
                Console.WriteLine("Vad blir nya kundens lösenord?");
                string newCustomerPassword = Console.ReadLine();
                Console.Clear();

                //write out to double check if info is correct
                Console.WriteLine($"Vill du skapa ett nytt användarkonto med följande information?\n" +
                    $"ID: {newCustomerID}\n" +
                    $"Namn: {newCustomerName}\n" +
                    $"E-mail: {newCustomerMailAddress}\n" +
                    $"Telefonnummer: {newCustomerPhoneNumber}\n" +
                    $"Lösenord: {newCustomerPassword}\n\n" +
                    $"[1] Ja, det vill jag\n" +
                    $"[2] Nej, jag vill ändra informationen");

                if (int.TryParse(Console.ReadLine(), out int adminMenuAddCustomer)) //if user input is a number
                {
                    switch (adminMenuAddCustomer)
                    {
                        case 1: // if user wishes to add customer with current info
                                //create new Customer account - save in dictionary?!
                            Customer newCustomer = new Customer(newCustomerID, newCustomerName, newCustomerMailAddress, newCustomerPhoneNumber, newCustomerPassword);
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

