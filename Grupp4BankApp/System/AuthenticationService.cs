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
            Console.Write("Användarnamn: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Lösenord: ");
            string password = Console.ReadLine() ?? "";

            if (_users.TryGetValue(username, out User? user))
            {
                if (PasswordHasher.VerifyPassword(password, user.Password))
                {
                    Console.WriteLine("Inloggningen lyckades!");
                    return user;
                }
            }

            Console.WriteLine("Fel användarnamn eller lösenord!");
            return null;
        }
    }
}
