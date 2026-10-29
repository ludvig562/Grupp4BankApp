using Grupp4BankApp.Users;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class AuthenticationService
    {
        private readonly Dictionary<string, User> _users;
        private readonly Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();

        public static User? CurrentUser { get; private set; }
        public AuthenticationService()
        { 
        }
        public AuthenticationService(Dictionary<string, User> users) 
        {
            _users = users;
        } 

        public User? Login(string username, string password)
        {
            if (!_users.TryGetValue(username, out User? user))//if it dosent find the user print Fel användarnamn eller lösenord! and return null 
            {
                Console.WriteLine("Fel användarnamn eller lösenord!");
                return null;
            }
            if (user.IsLocked)
            {
                Console.WriteLine("Kontot är låst. Kontakta support.");//if the user is locked print Kontot är låst. Kontakta support.
                return null;
            }
            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                int failures = _failedAttempts.GetValueOrDefault(username) + 1;
                _failedAttempts[username] = failures;
                if (failures >= 3)
                {
                    user.IsLocked = true;
                    Console.WriteLine("Kontot är låst efter tre felaktiga försök.");
                }
                else 
                { 
                    Console.WriteLine("Fel användarnamn eller lösenord!"); 
                }
                return null;
            }
            _failedAttempts.Remove(username);// remove 
            CurrentUser = user;

            var inloggComplete = new Text("Inloggningen lyckades!", new Style(Color.Green1))
            {
                Justification = Justify.Center,
            };
            AnsiConsole.Write(inloggComplete);
            //Console.WriteLine("Inloggningen lyckades!\n");
            return user;
        }

        public User? GetUser() 
        {
            return CurrentUser;
        } 
        public void Logout() 
        {
            CurrentUser = null;
        } 
    }
}
