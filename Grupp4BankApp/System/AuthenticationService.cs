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
        static User? currentLoggedUser;
        public User? Login(string aUserName, string aPassword)
        {

            //trys to get the same aUserName as entered and then if found it calls the method VerifyPassword to verify if the saved hash to the aUserName is the same as the hash of the entered password

            if (_users.TryGetValue(aUserName, out User? currentUser))
            {
                if (UserMaxAttempts == 3)
                {
                    currentUser.IsLocked = true;
                }

                if (PasswordHasher.VerifyPassword(aPassword, currentUser.Password) && currentUser.IsLocked == false)
                {
                    currentLoggedUser = currentUser;
                    Console.WriteLine("Inloggningen lyckades!");
                    return currentUser;// if correct it returns the currentUser
                }
                else if (currentUser.IsLocked == true)
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
        public User GetUser()
        {
           
            return currentLoggedUser;
        }


    }
}
