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

            AuthenticationService authentication =// creates a new object for AuthenticationService that sends the dictionery to the construktor
                new AuthenticationService(users);

            //-------TEMPORARY TEST FOR LOGIN METHOD-------//
            User bob = new User();

            bob.Username = "bob";
            bob.Password = PasswordHasher.HashPassword("1234");
            bob.Role = UserRole.User;

            users.Add(bob.Username, bob);
            //---------------------------------------------//

            User? loggedInUser = null;

            while (loggedInUser == null)// while loggedinuser is null keep repeting the login method
            {
                loggedInUser = authentication.Login();
            }
        }
    }
}
