using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //constructor, child from User class
        public Customer(int userID, string username, string name, string email, string phonenumber, string password) : base(userID, username, name, email, phonenumber, password)
        {
        }
    }
}
