using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.System
{

    internal class Enums
    {

        public enum Menus
        {
            LogInMenu,
            UserMenu,
            AdminMenu,
        }

        public enum CurrencyTypes
        {
            SEK,
            EUR,
            USD
        }

        public enum AccountType
        {
            checkingsAccount,
            savingsAccount
        }

        public enum UserRole
        {
            customer,
            admin
        }

    }
}