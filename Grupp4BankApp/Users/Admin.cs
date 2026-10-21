using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Admin : User
    {
        //constructor, child from User class
        public Admin(string username, string name, string email, string password) : base(username, name, email, password)
        {
            Role = Enums.UserRole.customer; //automatically add the role of admin
        }

    }
}

