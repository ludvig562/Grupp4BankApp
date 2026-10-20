using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class AuthenticationService
    {
        private Dictionary<string, User> _users;

        public AuthenticationService(Dictionary<string, User> users)
        {
            _users = users;
        }

        public User? Login()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Lösenord: ");
            string password = Console.ReadLine() ?? "";

            //trys to get the same username as entered and then if found it calls the method VerifyPassword to verify if the saved hash to the username is the same as the hash of the entered password
            if (_users.TryGetValue(email, out User? useremail)) 
            {
                if (PasswordHasher.VerifyPassword(password, useremail.Password))
                {
                    Console.WriteLine("Inloggningen lyckades!");
                    return useremail;// if correct it returns the user
                }
            }

            Console.WriteLine("Fel användarnamn eller lösenord!");
            return null;//if it fails it returns null so the program runs again
        }
    }
}
