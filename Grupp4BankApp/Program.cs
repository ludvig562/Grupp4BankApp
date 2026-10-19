using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using static Grupp4BankApp.Users.User;

namespace Grupp4BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, User> users = new Dictionary<string, User>();

            AuthenticationService authentication =
                new AuthenticationService(users);

            User bob = new User();

            bob.Username = "bob";
            bob.Password = PasswordHasher.HashPassword("1234");
            bob.Role = UserRole.User;

            users.Add(bob.Username, bob);

            User? loggedInUser = null;

            while (loggedInUser == null)
            {
                loggedInUser = authentication.Login();
            }
        }
    }
}
