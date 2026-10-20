using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Admin : User
    {
        //constructor, child from User class
        public Admin(int userID, string username, string name, string email, string phonenumber, string password) : base(userID, username, name, email, phonenumber, password)
        {
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
                    switch (adminMenuChoice)
                    {
                        case 1: //create a new user account

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

                            //write out to double check if info is correct?
                            //add if/else case?
                            //Console.WriteLine($"Vill du skapa ett nytt användarkonto med följande information?\n" +
                            //    $"ID: {newCustomerID}\n" +
                            //    $"Namn: {newCustomerName}" +
                            //    $"E-mail: {newCustomerMailAddress}, " +
                            //    $"Telefonnummer: {newCustomerPhoneNumber}" +
                            //    $"Lösenord: {newCustomerPassword}\n" +
                            //    $"[1] Ja, det vill jag\n" +
                            //    $"[2] Nej, jag vill ändra informationen");

                            //create new Customer account
                            //save customer info in dictionary!
                            Customer newCustomer = new Customer(newCustomerID, newCustomerUsername, newCustomerName, newCustomerMailAddress, newCustomerPhoneNumber, newCustomerPassword);
                            break;

                        case 2: //update currency exchange
                            break;

                        case 3: //end admin menu
                            Console.WriteLine("Du har nu loggats ut.");
                            adminMenuRunning = false;
                            break;

                        default: //if user input is a number but not between 1 and 3
                            Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 3!");
                            break;
                    }
                }
                else //if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra!");
                }
            }
        }
    }
}
