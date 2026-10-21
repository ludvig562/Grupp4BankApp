using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //properties: lists for bankaccounts
        public List<BankAccount> checkingAccounts { get; set; }
        public List<BankAccount> savingsAccounts { get; set; }


        //constructor, child from User class
        public Customer(string username, string name, string email, string password) : base(username, name, email, password)
        {
            Role = Enums.UserRole.customer; //automatically add the role of customer
            checkingAccounts = new List<BankAccount>();
            savingsAccounts = new List<BankAccount>();
        }

        //add addbankaccount method here, that receives info from other methods in customermenu class?!
        //1 metod for choosing account type
        //2 metod for creating account
        //have the console writelines in customermenu + the actual logic in customer?

        //method to open new bankaccount
        public void OpenBankAccount()
        {
            // Which type of bankaccount do they want (check or savings)
            Console.WriteLine("Vilken typ av konto vill du öppna?");
            Console.WriteLine("[1] Vanligt konto");
            Console.WriteLine("[2] Sparkonto");
            bool accountTypeChosen = false;

            while (!accountTypeChosen)
            {
                if (int.TryParse(Console.ReadLine(), out int accountType))
                {
                    // Create a BankAccount object of the correct type
                    switch (accountType)
                    {
                        case 1: //open checking account
                            //choose currency + name via method
                            Enums.CurrencyTypes chosenCurrency = CustomerMenu.ChooseCurrency();
                            string bankAccountName = CustomerMenu.ChooseBankAccountName();
                            //create bank account + add to checkingaccounts list
                            BankAccount newCheckingAccount = new BankAccount(Enums.AccountType.checkingsAccount, bankAccountName, chosenCurrency);
                            this.checkingAccounts.Add(newCheckingAccount);
                            //confirmation message that account has been created
                            Console.WriteLine($"Du har nu skapat ett konto i {chosenCurrency}. " +
                                $"Ditt bankkontonummer är: {newCheckingAccount.BankAccountNumber}\n");
                            accountTypeChosen = true; //end while loop
                            break;

                        case 2: //open savings account
                            //choose currency + name via method
                            chosenCurrency = CustomerMenu.ChooseCurrency();
                            bankAccountName = CustomerMenu.ChooseBankAccountName();
                            //create bank account + add to savingsaccounts list
                            BankAccount newSavingsAccount = new BankAccount(Enums.AccountType.savingsAccount, bankAccountName, chosenCurrency);
                            this.savingsAccounts.Add(newSavingsAccount);
                            //confirmation message that account has been created
                            Console.WriteLine($"Du har nu skapat ett sparkonto i {chosenCurrency}. " +
                                $"Ditt bankkontonummer är: {newSavingsAccount.BankAccountNumber}\n");
                            accountTypeChosen = true;
                            break;

                        default: //if user input is a number but not 1 or 2
                            Console.WriteLine("Oops. Ogiltigt val. Välj en siffra mellan 1 och 2!\n");
                            break;
                    }
                }
                else //if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra!");
                }
            }
        }
    }
}
