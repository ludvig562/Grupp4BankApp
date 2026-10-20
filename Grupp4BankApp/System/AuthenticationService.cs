using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        private int UserMaxAttempts = 0;
        private int AppMaxAttempts = 0;
        public User? Login()
        {
            Console.Write("Användarnamn: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Lösenord: ");
            string password = Console.ReadLine() ?? "";

            //trys to get the same username as entered and then if found it calls the method VerifyPassword to verify if the saved hash to the username is the same as the hash of the entered password

            if (_users.TryGetValue(username, out User? user)) 
            {
                if(UserMaxAttempts == 3)
                {
                    user.IsLocked = true;
                }

                if (PasswordHasher.VerifyPassword(password, user.Password) && user.IsLocked == false)
                {
                    Console.WriteLine("Inloggningen lyckades!");
                    return user;// if correct it returns the user
                }
                else if (user.IsLocked == true)
                {
                    Console.WriteLine("Förmånga mslyckade försök, du är ut låst från ditt konto\nKontakta support för att låsa up ditt konto");
                    return null;
                }
               
                UserMaxAttempts++;
                Console.WriteLine("Fel användarnamn eller lösenord!");
                return null;
            }
            else
            {
                AppMaxAttempts++;
                Console.WriteLine("Fel användarnamn eller lösenord!");
                return null;//if it fails it returns null so the program runs again
            }
        }
    }
}
