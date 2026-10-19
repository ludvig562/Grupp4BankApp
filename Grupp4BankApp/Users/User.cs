using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public enum UserRole
        {
            User,
            Admin
        }

    }
}
