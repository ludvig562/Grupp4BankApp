using Grupp4BankApp.System;
using Grupp4BankApp.Users;

namespace Grupp4BankApp.Transaction
{

    internal class TransactionSystem
    {
        // Store transactions until their execution time
        private static readonly Queue<PendingTransaction> pendingTransactions =
            new Queue<PendingTransaction>();

        // Protect the queue when accessed by the menu and the timer
        private static readonly object queueLock = new object();

        // Prevent two processing cycles from running at the same time
        private static readonly object processingLock = new object();

        // Keep the timer available while the application is running
        private static global::System.Threading.Timer? transactionTimer;

        // Start checking pending transactions when the application starts
        public static void StartTransactionTimer()
        {
            if (transactionTimer != null)
            {
                return;
            }

            // Check the queue every second
            TimeSpan interval = TimeSpan.FromSeconds(1);

            transactionTimer = new global::System.Threading.Timer(
                ProcessPendingTransactions,
                null,
                interval,
                interval);
        }

        // Find and execute transactions whose waiting time has passed
        private static void ProcessPendingTransactions(object? state)
        {
            lock (processingLock)
            {
                List<PendingTransaction> readyTransactions = new List<PendingTransaction>();

                lock (queueLock)
                {
                    int count = pendingTransactions.Count;

                    // Check the transactions currently in the queue
                    for (int i = 0; i < count; i++)
                    {
                        PendingTransaction transaction =
                            pendingTransactions.Dequeue();

                        if (transaction.ExecuteAtUtc <= DateTimeOffset.UtcNow)
                        {
                            // The waiting time has passed
                            readyTransactions.Add(transaction);
                        }
                        else
                        {
                            // The transaction must wait longer
                            pendingTransactions.Enqueue(transaction);
                        }
                    }
                }

                TransactionSystem transactionSystem = new TransactionSystem();

                foreach (PendingTransaction transaction in readyTransactions)
                {
                    try
                    {
                        transactionSystem.ExecuteTransfer(
                            transaction.FromAccount,
                            transaction.ToAccount,
                            transaction.Amount);
                    }
                    catch (Exception)
                    {
                        // Report the error without automatically repeating the transfer
                        Console.WriteLine("Ett oväntat fel uppstod vid överföringen. " +
                            "Kontrollera konton och transaktionshistorik.");
                    }
                }
            }
        }

        public void TransferMoney()
        {
            if (AuthenticationService.CurrentUser is Customer currentUser)
            {
                foreach (var item in currentUser.CheckingAccounts.Concat(currentUser.SavingsAccounts))
                {
                    Console.WriteLine($"{item}\n");
                }

                Console.Write("Från: ");
                string? from = Console.ReadLine() ?? "";

                BankAccount? FromAccount = currentUser.CheckingAccounts.
                    Concat(currentUser.SavingsAccounts).
                    FirstOrDefault(a => a.BankAccountName == from);

                if (FromAccount == null)
                {
                    Console.WriteLine("Konto finns inte!");
                    return;
                }

                Console.Write("Till: ");
                string? to = Console.ReadLine() ?? "";

                if (!int.TryParse(to, out int bankNumber))
                {

                    BankAccount? toAccount = currentUser.CheckingAccounts.
                        Concat(currentUser.SavingsAccounts).
                        FirstOrDefault(a => a.BankAccountName == to);


                    if (toAccount == null || FromAccount == toAccount)
                    {
                        Console.WriteLine("Konto finns inte!");
                        return;
                    }
                    TransferFunds(FromAccount, toAccount);
                }
                else
                {
                    BankAccount? toAccount = User.users.Values
                         .OfType<Customer>()
                         .SelectMany(c => c.CheckingAccounts.Concat(c.SavingsAccounts))
                         .FirstOrDefault(a => a.BankAccountNumber == bankNumber);

                    if (toAccount == null || FromAccount == toAccount)
                    {
                        Console.WriteLine("Konto finns inte! eller samma kotno");
                        return;
                    }

                    TransferFunds(FromAccount, toAccount);
                }

            }

        }
        public void TransferFunds(BankAccount fromAccount, BankAccount toAccount)
        {

            Console.Write("Belopp: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal belopp))
            {
                Console.WriteLine("Nummer tack");
                return;
            }

            // Check the amount before adding the transaction to the queue
            if (belopp < 1)
            {
                Console.WriteLine("Beloppet måste vara minst 1.");
                return;
            }

            // Check the current balance before accepting the transaction
            if (fromAccount.AccountBalance < belopp)
            {
                Console.WriteLine("Du har inte tillräckligt med pengar på kontot.");
                return;
            }

            // Create a pending transaction
            PendingTransaction transaction = new PendingTransaction(fromAccount,toAccount,belopp);

            // Add the transaction to the queue
            lock (queueLock)
            {
                pendingTransactions.Enqueue(transaction);
            }

            Console.WriteLine("Överföringen har lagts i kö och behandlas om cirka 15 minuter.");

        }

        // Execute a pending transaction when the waiting time has passed
        private void ExecuteTransfer(BankAccount fromAccount,BankAccount toAccount,decimal belopp)
        {

            // Keep the original amount when both accounts use the same currency
            decimal receivedAmount = belopp;

                // Convert the amount when the account currencies are different
                if (fromAccount.Currency != toAccount.Currency)
                {
                    // Require the administrator to approve exchange rates today
                    if (!ExchangeRates.UpdatedToday())
                    {
                        Console.WriteLine("En administratör måste uppdatera växelkurserna idag.");
                        return;
                    }

                    decimal fromRate = ExchangeRates.GetRate(fromAccount.Currency);
                    decimal toRate = ExchangeRates.GetRate(toAccount.Currency);

                    // Convert through SEK and round to two decimal places
                    receivedAmount = Math.Round(belopp * fromRate / toRate,2,MidpointRounding.AwayFromZero);

                    // Stop the transfer if the converted amount rounds to zero
                    if (receivedAmount <= 0)
                    {
                        Console.WriteLine("Det växlade beloppet är för litet.");
                        return;
                    }
                }
            if (fromAccount.AccountBalance >= belopp && belopp >= 1)
            {
                fromAccount.AccountBalance -= belopp;
            }
            else
            {
                if (belopp < 1)
                {
                    Console.WriteLine("Det är under 1, skärp dig");
                    return;
                }
                else
                {
                    Console.WriteLine("Du har inte tillräckligt med pengar på kontot.");
                    return;
                }
            }

            // Add the converted amount to the receiving account
            toAccount.AccountBalance += receivedAmount;

            Customer? sender = FindAccountOwner(fromAccount);
            Customer? reciever = FindAccountOwner(toAccount);


            //Sender
            TransactionLogg SenderLogg = new TransactionLogg
            {
                Date = DateTime.Now,
                Amount = new Amount
                {
                    PlusMinus = "-",
                    Value = belopp,
                    Currency = fromAccount.Currency,

                },
                Description = $"Skickad till {toAccount.BankAccountName}",
                FromCustomerName = sender.Name,
                ToCustomerName = reciever.Name,
                FromAccountName = fromAccount.BankAccountName, ToAccountName = toAccount.BankAccountName,

            };

            //Reciever
            TransactionLogg ReciverLogg = new TransactionLogg
            {
                Date = DateTime.Now,
                Amount = new Amount
                {
                    PlusMinus = "+",
                    Value = receivedAmount,
                    Currency = toAccount.Currency,
                },
                Description = $"Mottagen från {fromAccount.BankAccountName}",
                FromCustomerName = sender.Name,
                ToCustomerName = reciever.Name,
                FromAccountName = fromAccount.BankAccountName,
                ToAccountName = toAccount.BankAccountName,

            };
            fromAccount.TransactionsLogg.Add(SenderLogg);
            toAccount.TransactionsLogg.Add(ReciverLogg);

        }


        public void TransactionLog()
        {
            if (AuthenticationService.CurrentUser is Customer currentUser)
            {

                foreach (var item in currentUser.CheckingAccounts.Concat(currentUser.SavingsAccounts))
                {
                    Console.WriteLine($"{item}\n");
                }

                Console.Write("Skriv in vilken Konto: ");
                string? temp = Console.ReadLine() ?? "";
                Console.WriteLine();
                BankAccount? pickedAccount = currentUser.CheckingAccounts.
                    Concat(currentUser.SavingsAccounts).
                    FirstOrDefault(a => a.BankAccountName == temp);

                if (pickedAccount == null)
                {
                    Console.WriteLine("Konto finns inte!");
                    return;
                }

                Console.WriteLine("========== TRANSAKTIONSHISTORIK ==========\n\n");
                foreach (var item in pickedAccount.TransactionsLogg)
                {
                    Console.WriteLine(item);
                    Console.WriteLine("----------------------------------------\n");

                }

            }
        }

        private Customer? FindAccountOwner(BankAccount account)
        {
            return User.users.Values
                .OfType<Customer>()
                .FirstOrDefault(c =>
                   c.CheckingAccounts
                        .Concat(c.SavingsAccounts)
                        .Any(b => b.BankAccountNumber == account.BankAccountNumber));
        }

    }
}
