using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using Spectre.Console;
using System.Security.Principal;

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

                BankAccount? fromAccount = SelectBankAccount(currentUser.CheckingAccounts,currentUser.SavingsAccounts,"Välj konto att överföra från");

                if (fromAccount == null)
                {
                    AnsiConsole.MarkupLine("[yellow]Överföringen avbröts.[/]");
                    return;
                }

                AnsiConsole.WriteLine();
                BankAccount? toAccount = null;

                while (toAccount == null || fromAccount == toAccount)
                {
                    int bankNumber = AnsiConsole.Prompt(
                        new TextPrompt<int>("[bold white]Ange mottagarens banknummer:[/]")
                        .PromptStyle(Color.White)
                            .ValidationErrorMessage("[red]Banknumret måste innehålla minst 4 siffror.[/]")
                            .Validate(number => number >= 1000 ? ValidationResult.Success() : ValidationResult.Error("[red]Banknumret måste innehålla minst 4 siffror.[/]")
                            )
                    );

                    AnsiConsole.WriteLine();
                    toAccount = User.users.Values
                        .OfType<Customer>()
                        .SelectMany(c => c.CheckingAccounts.Concat(c.SavingsAccounts))
                        .FirstOrDefault(a => a.BankAccountNumber == bankNumber);

                    if (toAccount == null)
                    {
                        AnsiConsole.MarkupLine("[red]Inget konto hittades med det banknumret i vårt system.[/]");
                        continue;
                    }

                    if (fromAccount == toAccount)
                    {
                        AnsiConsole.MarkupLine("[red]Du kan inte överföra till samma konto.[/]");
                        continue;

                    }
                }
                TransferFunds(fromAccount, toAccount);
                Console.ReadKey();
                Console.Clear();

            }

        }
        public void TransferFunds(BankAccount fromAccount, BankAccount toAccount)
        {

            AnsiConsole.MarkupLine($"[bold white]Tillgängligt saldo:[/] [green1]{fromAccount.AccountBalance:N2}[/][bold white] {fromAccount.Currency}[/]");
            AnsiConsole.WriteLine();
            decimal amount = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[bold white]Ange belopp att  överföra:[/]")
                .PromptStyle(Color.White)
                    .ValidationErrorMessage("[red]Ange ett giltigt belopp.[/]")
                    .Validate(value =>
                    {
                        if (value < 1)
                        {
                            return ValidationResult.Error(
                                "[red]Beloppet måste vara minst 1.[/]"
                            );
                        }

                        if (value > fromAccount.AccountBalance)
                        {
                            return ValidationResult.Error(
                                "[red]Du har inte tillräckligt med pengar på kontot.[/]"
                            );
                        }

                        return ValidationResult.Success();
                    })
            );

            //fromAccount.AccountBalance -= amount;
            //toAccount.AccountBalance += amount;

            // Create a pending transaction
            PendingTransaction transaction = new PendingTransaction(fromAccount, toAccount, amount);

            // Add the transaction to the queue
            lock (queueLock)
            {
                pendingTransactions.Enqueue(transaction);
            }

            AddTransactionToLogg(toAccount,fromAccount,amount);
#if RELEASE
            AnsiConsole.Progress()
          .AutoClear(true)
          .Start(ctx =>
          {
              var task = ctx.AddTask("Processing transaction");

              while (!ctx.IsFinished)
              {
                  task.Increment(4);
                  Thread.Sleep(50);
              }
          });

            Console.Clear();
            AnsiConsole.MarkupLine("[green]Transaction complete! Avaliable in 15 Minutes.[/]");
#elif DEBUG

            Console.WriteLine("Överföringen har lagts i kö och behandlas om cirka 15 minuter.");
#endif

        }
        
            // // Execute a pending transaction when the waiting time has passed
         private void ExecuteTransfer(BankAccount fromAccount, BankAccount toAccount, decimal belopp)
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
                receivedAmount = Math.Round(belopp * fromRate / toRate, 2, MidpointRounding.AwayFromZero);

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



         


        }


        public void TransactionLog()
        {
            if (AuthenticationService.CurrentUser is Customer currentUser)
            {

                BankAccount? pickedAccount = SelectBankAccount(currentUser.CheckingAccounts, currentUser.SavingsAccounts, "Välj konto att öppna tranasktioner");

                if (pickedAccount == null)
                {
                    AnsiConsole.MarkupLine("[yellow]Överföringen avbröts.[/]");
                    return;
                }

                AnsiConsole.WriteLine();
               


                AnsiConsole.Write(new Rule("[bold cyan]TRANSAKTIONSHISTORIK[/]").RuleStyle("cyan"));

                if (pickedAccount.TransactionsLogg.Count == 0)
                {
                    AnsiConsole.MarkupLine(
                        "[yellow]Det finns inga transaktioner för detta konto.[/]"
                    );
                    return;
                }

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .Title("[bold cyan]Kontots transaktioner[/]")
                    .Expand();

                table.AddColumn("[grey]Datum[/]");
                table.AddColumn("[grey]Beskrivning[/]");
                table.AddColumn("[grey]Från[/]");
                table.AddColumn("[grey]Till[/]");
                table.AddColumn("[grey]Belopp[/]");

                foreach (var item in pickedAccount.TransactionsLogg)
                {
                    string amountColor = item.Amount.PlusMinus == "+"
                        ? "green"
                        : "red";

                    string amount =
                        $"{item.Amount.PlusMinus}{item.Amount.Value:N2} {item.Amount.Currency}";

                    table.AddRow(
                        Markup.Escape(item.Date.ToString("yyyy-MM-dd HH:mm")),
                        Markup.Escape(item.Description),
                        Markup.Escape($"{item.FromAccountName} ({item.FromCustomerName})"),
                        Markup.Escape($"{item.ToAccountName} ({item.ToCustomerName})"),
                        $"[{amountColor}]{Markup.Escape(amount)}[/]"
                    );
                }

                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine("[grey]Any key to continue[/]");
                Console.ReadLine();
                Console.Clear();
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

        private BankAccount? SelectBankAccount(List<BankAccount> checkingAccounts, List<BankAccount> savingsAccounts, string title)
        {

            AnsiConsole.MarkupLine("[grey]Esc - Avbryt[/]");
            var accounts = checkingAccounts
                .Select(a => (Account: a, Type: "Checkkonto"))
                .Concat(savingsAccounts.Select(a => (Account: a, Type: "Sparkonto")))
                .ToList();

            if (accounts.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Du har inga bankkonton![/]");
                return null;
            }

            int selectedIndex = 0;
            BankAccount? selectedAccount = null;

            Table CreateTable()
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .Title($"[bold cyan]{Markup.Escape(title)}[/]")
                    .AddColumn(" ")
                    .AddColumn("[bold white]Banknamn[/]")
                    .AddColumn("[bold white]Banknummer[/]")
                    .AddColumn("[bold white]Kontotyp[/]")
                    .AddColumn("[bold white]Saldo[/]");

                for (int i = 0; i < accounts.Count; i++)
                {
                    var account = accounts[i].Account;
                    bool selected = i == selectedIndex;

                    string name = Markup.Escape(account.BankAccountName);
                    string number = account.BankAccountNumber.ToString();
                    string type = accounts[i].Type;
                    string balance = $"{account.AccountBalance:N2} {account.Currency}";
                    if (selected)
                    {
                        table.AddRow(
                            "[bold yellow]>[/]",
                            $"[bold white on black]{name}[/]",
                            $"[bold white on black]{Markup.Escape(number)}[/]",
                            $"[bold white on black]{type}[/]",
                            $"[bold white on black]{Markup.Escape(balance)}[/]"
                        );
                    }
                    else
                    {
                        table.AddRow(
                            "",
                            $"[gray]{name}[/]",
                            $"[gray]{Markup.Escape(number)}[/]",
                            $"[gray]{type}[/]",
                            $"[gray]{Markup.Escape(balance)}[/]"
                        );
                    }
                }

                return table;
            }

            AnsiConsole.Live(CreateTable())
                .Start(ctx => 
                {
                    ctx.UpdateTarget(CreateTable());
                    ctx.Refresh();
                    while (true)
                    {
                        var key = Console.ReadKey(true).Key;

                        if (key == ConsoleKey.UpArrow)
                        {
                            selectedIndex--;

                            if (selectedIndex < 0)
                                selectedIndex = accounts.Count - 1;
                        }
                        else if (key == ConsoleKey.DownArrow)
                        {
                            selectedIndex++;

                            if (selectedIndex >= accounts.Count)
                                selectedIndex = 0;
                        }
                        else if (key == ConsoleKey.Enter)
                        {
                            selectedAccount = accounts[selectedIndex].Account;
                            break;
                        }
                        else if (key == ConsoleKey.Escape)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }

                        ctx.UpdateTarget(CreateTable());
                        ctx.Refresh();
                    }
                });

            return selectedAccount;
        }

        public void AddTransactionToLogg(BankAccount fromAccount, BankAccount toAccount,decimal amount)
        {


            Customer? sender = FindAccountOwner(fromAccount);
            Customer? reciever = FindAccountOwner(toAccount);

            //Sender
            TransactionLogg SenderLogg = new TransactionLogg
            {
                Date = DateTime.Now,
                Amount = new Amount
                {
                    PlusMinus = "-",
                    Value = amount,
                    Currency = fromAccount.Currency,

                },
                Description = $"Skickad till {toAccount.BankAccountName}",
                FromCustomerName = sender.Name,
                ToCustomerName = reciever.Name,
                FromAccountName = fromAccount.BankAccountName,
                ToAccountName = toAccount.BankAccountName,

            };

            //Reciever
            TransactionLogg ReciverLogg = new TransactionLogg
            {
                Date = DateTime.Now,
                Amount = new Amount
                {
                    PlusMinus = "+",
                    Value = amount,
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
    }
}
