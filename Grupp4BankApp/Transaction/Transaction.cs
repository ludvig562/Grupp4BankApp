using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using Spectre.Console;

namespace Grupp4BankApp.Transaction
{

    internal class TransactionSystem
    {
        public void TransferMoney()
        {
            if (AuthenticationService.CurrentUser is Customer currentUser)
            {

                BankAccount? FromAccount = SelectBankAccount(currentUser.CheckingAccounts,currentUser.SavingsAccounts,"Välj konto att överföra från");

                if (FromAccount == null)
                {
                    AnsiConsole.MarkupLine("[yellow]Överföringen avbröts.[/]");
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

            toAccount.AccountBalance += belopp;

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
                    Value = belopp,
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
    }
}
