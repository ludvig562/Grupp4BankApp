using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class User
    {
        public static Dictionary<string, User> users = new Dictionary<string, User>();

        private static AuthenticationService? authentication;

        public static void Init()
        {
            authentication = new AuthenticationService(users);
        }

        //properties for users - set as protected? (can only be used in class+subclasses)
        public int UserID { get; set; }
        //somehow work with guid or another system to have an ID automatically assigned instead?

        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public UserRole Role { get; set; }

        public enum UserRole { user, admin }


        //constructor
        public User()
        {

        }

        public User(int userID, string username, string name, string email, string password)
        {
            UserID = userID;
            Username = username;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
