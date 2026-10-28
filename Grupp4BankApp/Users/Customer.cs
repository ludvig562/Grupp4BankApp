using Grupp4BankApp.Menus;
using Grupp4BankApp.System;
using Grupp4BankApp.Transaction;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace Grupp4BankApp.Users
{
    internal class Customer : User
    {
        //properties: lists for bankaccounts
        public List<BankAccount> CheckingAccounts { get; set; }
        public List<BankAccount> SavingsAccounts { get; set; }

        public List<Loan> Loans { get; set; }

        //constructor, child from User class
        public Customer(string username, string name, string email, string password) : base(username, name, email, password)
        {
            Role = Enums.UserRole.customer; //automatically add the role of customer
            CheckingAccounts = new List<BankAccount>();
            SavingsAccounts = new List<BankAccount>();

            Loans = new List<Loan>();
        }

        //------ CUSTOMER UPDATES THEIR OWN INFO ------//

        //method for customer to update their own info
        public void UpdateCustomerInformation()
        {
            bool informationUpdated = false;
            while (!informationUpdated)
            {
                //ask customer which info they wish to update
                Console.WriteLine("Vilken information vill du uppdatera?\n" +
                "[1] Emailaddress\n" +
                "[2] Lösenord\n");

                if (int.TryParse(Console.ReadLine(), out int updateInfoMenuChoice)) //check if user input is a number
                {
                    switch (updateInfoMenuChoice)
                    {
                        case 1: //update email address
                            UpdateEmail();
                            informationUpdated = true;
                            break;
                        case 2: //update password
                            UpdatePassword();
                            informationUpdated = true;
                            break;
                        default:
                            Console.WriteLine("Oops. Du måste välja en siffra mellan 1 och 2.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Du måste ange en siffra mellan 1 och 2.");
                }
            }
        }

        //method for updating customer email address
        public void UpdateEmail()
        {
            Console.WriteLine($"Ditt nuvarande mejladdress är: {this.Email}\n");
            string newEmailAddress = Admin.GetMailAddress();
            this.Email = newEmailAddress;
            Console.WriteLine($"Du har nu bytt till följande mejladdress: {this.Email}");
        }

        //method for updating customer password
        public void UpdatePassword()
        {
            string newPassword = Admin.GetPassword();
            this.Password = PasswordHasher.HashPassword(newPassword);
            Console.WriteLine("Du har nu bytt ditt lösenord.");
        }


        //------ DISPLAY ALL CUSTOMER'S BANK ACCOUNTS ------//

        // Displays all the customer's bank accounts and their balances
        public void ShowBankAccounts()
        {
            Console.Clear();
            Console.WriteLine("========== MINA BANKKONTON ==========\n");

            // Checks whether the customer has no bank accounts
            if (CheckingAccounts.Count == 0 && SavingsAccounts.Count == 0)
            {
                Console.WriteLine("Du har inga bankkonton ännu.");
            }
            else
            {
                // Displays all checking accounts
                if (CheckingAccounts.Count > 0)
                {
                    Console.WriteLine("--- VANLIGA KONTON ---\n");

                    foreach (BankAccount account in CheckingAccounts)
                    {
                        Console.WriteLine($"Namn: {account.BankAccountName}");
                        Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                        Console.WriteLine(
                            $"Saldo: {account.AccountBalance:N2} {account.Currency}");
                        Console.WriteLine("------------------------------------");
                    }
                }

                // Displays all savings accounts
                if (SavingsAccounts.Count > 0)
                {
                    Console.WriteLine("\n--- SPARKONTON ---\n");

                    foreach (BankAccount account in SavingsAccounts)
                    {
                        Console.WriteLine($"Namn: {account.BankAccountName}");
                        Console.WriteLine($"Kontonummer: {account.BankAccountNumber}");
                        Console.WriteLine(
                            $"Saldo: {account.AccountBalance:N2} {account.Currency}");
                        Console.WriteLine("------------------------------------\n\n");
                    }
                }
            }

            // Waits for the customer to press Enter before returning to the menu
            //Console.WriteLine("\nTryck på Enter för att återgå till menyn.");
            //Console.ReadKey();
            //Console.Clear();
        }


        //------ CUSTOMER OPENS NEW BANK ACCOUNT ------//

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
                            OpenCheckingAccount(); //method to open checking account
                            accountTypeChosen = true; //end while loop
                            break;

                        case 2: //open savings account
                            OpenSavingsAccount(); //method to open savings account
                            accountTypeChosen = true; //end while loop
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

        //method for opening checking account
        public void OpenCheckingAccount()
        {
            Enums.CurrencyTypes chosenCurrency = ChooseCurrency(); //ask for currency
            string bankAccountName = ChooseBankAccountName(); //ask for name of bank account

            //create new bank account
            BankAccount newCheckingAccount = new BankAccount(Enums.AccountType.checkingsAccount, bankAccountName, chosenCurrency);
            //add new bank account to customer's bank account list
            this.CheckingAccounts.Add(newCheckingAccount);

            WriteConfirmationMessage(newCheckingAccount); //write confirmation message
        }

        //method for opening savings account
        public void OpenSavingsAccount()
        {
            Enums.CurrencyTypes chosenCurrency = ChooseCurrency(); //ask for currency
            string bankAccountName = ChooseBankAccountName(); //ask for name of bank account

            //create new bank account
            BankAccount newSavingsAccount = new BankAccount(Enums.AccountType.savingsAccount, bankAccountName, chosenCurrency);
            //add new bank account to customer's bank account list
            this.SavingsAccounts.Add(newSavingsAccount);

            WriteConfirmationMessage(newSavingsAccount); //write confirmation message
        }


        //------ CUSTOMER DEPOSITS/WITHDRAWS MONEY IN/FROM THEIR OWN ACCOUNT ------//

        //method for depositing money into account
        public void DepositMoney()
        {
            bool depositCompleted = false;
            while (!depositCompleted)
            {
                ShowBankAccounts(); //show overview of all their accounts
                Console.WriteLine("På vilket konto vill du sätta in pengar? Fyll i ditt bankkontonummer.");
                if (int.TryParse(Console.ReadLine(), out int bankNumber))
                {
                    BankAccount account = FindBankAccount(bankNumber); //find the chosen bank account
                    if (account == null) //if account number does not exist
                    {
                        Console.WriteLine("Det finns inget konto med det kontonumret.");
                        continue;
                    }

                    decimal depositMoneyAmount = GetDepositAmount(); //use method to get the deposit money amount
                    account.Deposit(depositMoneyAmount); //method for depositing money (in bankaccount class)
                    WriteMessageInterestSavingsAccount(account); //method for writing interest in savings account
                    depositCompleted = true;
                }
                else
                {
                    Console.WriteLine("Oops. Du måste skriva ett bankkontonummer. Försök igen.");
                    Console.ReadKey();
                }
            }
        }

        //method for getting the deposit amount
        public static decimal GetDepositAmount()
        {
            bool validDepositAmount = false;
            decimal depositAmount = 0;
            do
            {
                Console.WriteLine("Hur mycket vill du sätta in?");

                if (decimal.TryParse(Console.ReadLine(), out depositAmount) && depositAmount > 0)
                {
                    validDepositAmount = true;
                }
                else
                {
                    Console.WriteLine("Ogiltigt belopp. Vänligen välj ett belopp större än noll.");
                }
            }
            while (!validDepositAmount);
            return depositAmount;
        }


        //method for depositing money into account
        public void WithDrawMoney()
        {
            bool withdrawalCompleted = false;
            while (!withdrawalCompleted)
            {
                ShowBankAccounts(); //show overview of all their accounts
                Console.WriteLine("Från vilket konto vill du ta ut pengar? Fyll i ditt bankkontonummer.");
                if (int.TryParse(Console.ReadLine(), out int bankNumber))
                {
                    BankAccount account = FindBankAccount(bankNumber); //find the chosen bank account
                    if (account == null) //if account number does not exist
                    {
                        Console.WriteLine("Det finns inget konto med det kontonumret.");
                        continue;
                    }

                    decimal withdrawalAmountMoney = GetWithdrawalAmount(account); //use method to get the withdrawal money amount
                    account.Withdraw(withdrawalAmountMoney); //method for depositing money (in bankaccount class)
                    WriteMessageInterestSavingsAccount(account); //method for writing interest in savings account
                    withdrawalCompleted = true;
                }
                else
                {
                    Console.WriteLine("Oops. Du måste skriva ett bankkontonummer. Försök igen.");
                    Console.ReadKey();
                }
            }
        }


        //method for getting the withdrawal amount
        public static decimal GetWithdrawalAmount(BankAccount account)
        {
            bool validWithdrawalAmount = false;
            decimal withDrawalAmount = 0;
            do
            {
                Console.WriteLine("Hur mycket vill du ta ut?");

                if (decimal.TryParse(Console.ReadLine(), out withDrawalAmount) && withDrawalAmount > 0 
                    && withDrawalAmount <= account.AccountBalance) //amount should be less than what is in their account
                {
                    validWithdrawalAmount = true;
                }
                else
                {
                    Console.WriteLine($"Ogiltigt belopp. " +
                        $"Vänligen välj ett belopp större än noll " +
                        $"och mindre än din balans på kontot: {account.AccountBalance} {account.Currency}.");
                }
            }
            while (!validWithdrawalAmount);
            return withDrawalAmount;
        }

        //method for finding the bank account chosen by the customer
        private BankAccount FindBankAccount(int bankNumber)
        {
            foreach (BankAccount account in CheckingAccounts)
            {
                if (account.BankAccountNumber == bankNumber)
                {
                    return account;
                }
            }
            foreach (BankAccount account in SavingsAccounts)
            {
                if (account.BankAccountNumber == bankNumber)
                {
                    return account;
                }
            }
            return null;
        }


        //method for getting the bank account name
        public static string ChooseBankAccountName()
        {
            Console.WriteLine("Vad ska ditt nya konto heta?");
            string bankAccountName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(bankAccountName))
            {
                return bankAccountName;
            }
            else //if user didn't write a name for the account
            {
                do
                {
                    Console.WriteLine("Vänligen skriv ett namn för ditt konto.");
                    bankAccountName = Console.ReadLine();
                }
                while (string.IsNullOrWhiteSpace(bankAccountName));
                return bankAccountName;
            }
        }

        //method for choosing bank account currency
        public static Enums.CurrencyTypes ChooseCurrency()
        {
            Console.WriteLine("Vilken valuta vill du öppna kontot i?");
            Console.WriteLine("[1] SEK");
            Console.WriteLine("[2] EUR");
            Console.WriteLine("[3] USD");

            //create variable for chosen currency, with standard value SEK
            Enums.CurrencyTypes chosenCurrency = Enums.CurrencyTypes.SEK;

            bool currencyChosen = false;
            while (!currencyChosen)
            {
                if (int.TryParse(Console.ReadLine(), out int currencyChoice))
                {
                    //add currency from enum to chosen currency
                    switch (currencyChoice)
                    {
                        case 1:
                            chosenCurrency = Enums.CurrencyTypes.SEK;
                            currencyChosen = true;
                            break;
                        case 2:
                            chosenCurrency = Enums.CurrencyTypes.EUR;
                            currencyChosen = true;
                            break;
                        case 3:
                            chosenCurrency = Enums.CurrencyTypes.USD;
                            currencyChosen = true;
                            break;
                        default: //if user input is a number but not 1, 2 or 3
                            Console.WriteLine("Ogiltigt val. Välj en siffra mellan 1 och 3!");
                            break;
                    }
                }
                else //if user input is not a number
                {
                    Console.WriteLine("Oops. Du måste välja en siffra!");
                }
            }
            return chosenCurrency;
        }

        //method for writing out a confirmation message when a new bankacount has been created
        public static void WriteConfirmationMessage(BankAccount account)
        {
            Console.WriteLine($"\nDu har nu skapat ett konto:\n" +
                                $"Namn: {account.BankAccountName}\n" +
                                $"Valuta: {account.Currency}\n" +
                                $"Bankkontonummer: {account.BankAccountNumber}\n" +
                                $"Konto saldo: {account.AccountBalance}\n");
        }

        public static void WriteMessageInterestSavingsAccount(BankAccount account)
        {
            if (account.AccountType == Enums.AccountType.savingsAccount)
            {
                decimal yearlyInterest = account.CalculateInterest();
                Console.WriteLine($"Din beräknade årsränta är: {yearlyInterest} {account.Currency}.\n");
            }
        }

    }
}