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

        // Download exchange rates and ask the administrator to approve them
        public static void UpdateExchangeRates()
        {
            // Wait for the request because the existing console menu is synchronous
            ExchangeRates.UpdateFromApiAsync().GetAwaiter().GetResult();
        }

        //method for creating a new user
        public static void CreateCustomer()
        {
            bool customerCreated = false;

            string newCustomerUsername = GetUserName(); //get username via method GetUserName()
            string newCustomerName = GetName(); //get name via method GetName()
            string newCustomerMailAddress = GetMailAddress(); //get mailaddress via method GetMailAddress()
            string newCustomerPassword = GetPassword(); //get password via method GetPassword()
            Console.Clear();

            while (!customerCreated)
            {
                //via method ConfirmNewCustomerInfo: show confirmation of added info
                //+ ask if they wish to save as it or if they wish to update certain info
                int adminMenuAddCustomer = ConfirmNewCustomerInfo(newCustomerName, newCustomerUsername, newCustomerMailAddress, newCustomerPassword);

                switch (adminMenuAddCustomer)
                {
                    case 1: //create a new user account
                        Customer newCustomer = new Customer(newCustomerUsername, newCustomerName, newCustomerMailAddress, PasswordHasher.HashPassword(newCustomerPassword));
                        User.users.Add(newCustomerUsername, newCustomer);
                        Console.WriteLine($"Ny användare {newCustomerName} har skapats.\n");
                        customerCreated = true;
                        break;
                    case 2: //if user wishes to change info
                        int newCustomerUpdateInfoChoice = UpdateCustomerInformation();
                        switch (newCustomerUpdateInfoChoice)
                        {
                            case 1:
                                newCustomerUsername = GetUserName();
                                break;
                            case 2:
                                newCustomerName = GetName();
                                break;
                            case 3:
                                newCustomerMailAddress = GetMailAddress();
                                break;
                            case 4:
                                newCustomerPassword = GetPassword();
                                break;
                            default:
                                Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 4!\n");
                                break;
                        }
                        break;

                    default: // if user input is a number but not 1 or 2
                        Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 2!\n");
                        break;
                }
            }
        }

        //method for getting the customer username
        public static string GetUserName()
        {
            string newCustomerUsername;

            do
            {
                //ask for customer info
                Console.WriteLine("Vad är kundens användarnamn?");
                newCustomerUsername = Console.ReadLine();

                //check if user input is not null/empty
                if (string.IsNullOrWhiteSpace(newCustomerUsername))
                {
                    Console.WriteLine("Användarnamnet får inte vara tomt! Vänligen skriv ett användarnamn.");
                    continue; //breaking the if loop but not the while loop, so admin can write username
                }

                /*check if username already exists in dictionary via ContainsKey instead of TryGetValue
                because here we only need to check if the name already exists in the dictionary,
                we don't need to get the entire user*/
                if (User.users.ContainsKey(newCustomerUsername))
                {
                    Console.WriteLine("Användarnamn finns redan. Välj ett annat användarnamn.");
                    continue; //breaking the if loop but not the while loop, so admin can correct username
                }
            }
            while (string.IsNullOrWhiteSpace(newCustomerUsername) || User.users.ContainsKey(newCustomerUsername));
            return newCustomerUsername;
        }

        //method for getting the customer's username
        public static string GetName()
        {
            string newCustomerName;
            do
            {
                Console.WriteLine("Vad heter kunden?");
                newCustomerName = Console.ReadLine();

                //check if user input is not null/empty
                if (string.IsNullOrWhiteSpace(newCustomerName))
                {
                    Console.WriteLine("Namnet får inte vara tomt! Vänligen skriv ett namn.");
                }
            }
            while (string.IsNullOrWhiteSpace(newCustomerName));
            return newCustomerName;
        }

        //method for getting the customer's email address
        public static string GetMailAddress()
        {
            string newCustomerMailAddress;
            do
            {
                Console.WriteLine("Vänligen skriv mejladdressen du vill använda:");
                newCustomerMailAddress = Console.ReadLine();

                //check if user input is not null/empty
                if (string.IsNullOrWhiteSpace(newCustomerMailAddress))
                {
                    Console.WriteLine("Mejladdressen får inte vara tomt! Vänligen skriv en giltig mejladdress.");
                }
            }
            while (string.IsNullOrWhiteSpace(newCustomerMailAddress));
            return newCustomerMailAddress;
        }

        //method for getting the customer's password
        public static string GetPassword()
        {
            string newCustomerPassword;
            string confirmPassword;
            do
            {
                Console.WriteLine("Vänligen skriv lösenordet du vill använda:");
                newCustomerPassword = Console.ReadLine();

                //check if user input is not null/empty
                if (string.IsNullOrWhiteSpace(newCustomerPassword))
                {
                    Console.WriteLine("Lösenordet får inte vara tomt! Vänligen skriv ett giltigt lösenord.");
                }

                Console.WriteLine("Vänligen bekräfta lösenordet:");
                confirmPassword = Console.ReadLine();

                if (newCustomerPassword != confirmPassword)
                {
                    Console.WriteLine("Lösenorden matchar inte. Försök igen.");
                }
            }
            while (string.IsNullOrWhiteSpace(newCustomerPassword) || newCustomerPassword != confirmPassword);
            return newCustomerPassword;
        }

        //method for showing customer info when creating new user + asking if they wish to update info
        public static int ConfirmNewCustomerInfo(string newCustomerName, string newCustomerUsername, string newCustomerMailAddress, string newCustomerPassword)
        {
            int adminMenuAddCustomer;

            Console.WriteLine($"Vill du skapa ett nytt användarkonto med följande information?\n\n" +
                $"Namn: {newCustomerName}\n" +
                $"Användarnamn: {newCustomerUsername}\n" +
                $"E-mail: {newCustomerMailAddress}\n" +
                $"Lösenord: {newCustomerPassword}\n\n" +
                $"[1] Ja, det vill jag\n" +
                $"[2] Nej, jag vill ändra informationen");

            int.TryParse(Console.ReadLine(), out adminMenuAddCustomer); //if user input is a number
            if (adminMenuAddCustomer == 1 || adminMenuAddCustomer == 2) //if user input is 1 or 2
            {
                return adminMenuAddCustomer;
            }
            else //if user input is not a number, or not 1 or 2
            {
                do
                {
                    Console.WriteLine("Vänligen välj en siffra mellan 1 och 2!");
                }
                while (!int.TryParse(Console.ReadLine(), out adminMenuAddCustomer) || (adminMenuAddCustomer < 1 || adminMenuAddCustomer > 2));
                return adminMenuAddCustomer;
            }
        }


        //method for updating customer info before creating customer
        public static int UpdateCustomerInformation()
        {
            Console.WriteLine("\nVad vill du ändra?\n" +
                                "[1] Användarnamn\n" +
                                "[2] Namn\n" +
                                "[3] Mejladdress\n" +
                                "[4] Password\n");
            int newCustomerUpdateInfoChoice;
            int.TryParse(Console.ReadLine(), out newCustomerUpdateInfoChoice); //if user input is a number
            if (newCustomerUpdateInfoChoice == 1 || newCustomerUpdateInfoChoice == 2 || newCustomerUpdateInfoChoice == 3 || newCustomerUpdateInfoChoice == 4) //if user input is between 1-4
            {
                return newCustomerUpdateInfoChoice;
            }
            else
            {
                do
                {
                    Console.WriteLine("Vänligen välj en siffra mellan 1 och 4!");
                }
                while (!int.TryParse(Console.ReadLine(), out newCustomerUpdateInfoChoice) || (newCustomerUpdateInfoChoice < 1 || newCustomerUpdateInfoChoice > 4));
                return newCustomerUpdateInfoChoice;
            }
        }
    }
}

