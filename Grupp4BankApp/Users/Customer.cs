using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //constructor, child from User class
        public Customer(int userID, string name, string email, string phonenumber, string password) : base(userID, name, email, phonenumber, password)
        {
        }

        //public override string ToString()
        //{
        //    return $"{UserID}, {Name}, {Email}, {PhoneNumber}, {Password}";
        //}
    }
}
