using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class User
    {
        //properties for users 
        public int UserID { get; set; }
        //somehow work with guid or another system to have an ID automatically assigned instead?

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        //constructor
        public User(int userID, string name, string email, string phonenumber, string password)
        {
            UserID = userID;
            Name = name;
            Email = email;
            PhoneNumber = phonenumber;
            Password = password;
        }
    }
}
