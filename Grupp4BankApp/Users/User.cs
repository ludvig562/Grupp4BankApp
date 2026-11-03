using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class User
    {
        public static Dictionary<string, User> users = new Dictionary<string, User>();

        //properties for users - set as protected? (can only be used in class+subclasses)

        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public Enums.UserRole Role { get; set; }


        //constructor
        public User()
        {

        }

        public User(string username, string name, string email, string password)
        {
            Username = username;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
