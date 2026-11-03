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
        // Stores all users with their usernames as keys
        private readonly Dictionary<string, User> _users;

        // Keeps track of failed login attempts for each username
        private readonly Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();

        // Stores the currently logged-in user
        public static User? CurrentUser { get; private set; }
        public AuthenticationService()
        {
        }

        // Constructor that receives the dictionary containing all users
        public AuthenticationService(Dictionary<string, User> users) 
        {
            _users = users;
        }

        // Checks the username and password and returns the user if login succeeds
        public User? Login(string username, string password)
        {
            // Tries to find a user with the entered username
            if (!_users.TryGetValue(username, out User? user))            {
                Console.WriteLine("Fel användarnamn eller lösenord!");
                return null;
            }

            // Checks if the user's account is locked
            if (user.IsLocked)
            {
                Console.WriteLine("Kontot är låst. Kontakta support.");
                return null;
            }

            // Verifies the entered password against the stored password hash
            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                // Increases the number of failed login attempts by one
                int failures = _failedAttempts.GetValueOrDefault(username) + 1;
                _failedAttempts[username] = failures;

                // Locks the account after three failed login attempts
                if (failures >= 3)
                {
                    user.IsLocked = true;
                    Console.WriteLine("Kontot är låst efter tre felaktiga försök.");
                }
                else 
                { 
                    Console.WriteLine("Fel användarnamn eller lösenord!"); 
                }

                // Returns null because the password was incorrect
                return null;
            }

            // Removes previous failed attempts after a successful login
            _failedAttempts.Remove(username);

            // Saves the successfully logged-in user
            CurrentUser = user;

            // Displays a green message when login succeeds
            var inloggComplete = new Text("Inloggningen lyckades!", new Style(Color.Green1))
            {
                Justification = Justify.Center,
            };
            AnsiConsole.Write(inloggComplete);

            // Returns the logged-in user
            return user;
        }

        // Logs out the current user by setting CurrentUser to null
        public void Logout() 
        {
            CurrentUser = null;
        } 
    }
}
