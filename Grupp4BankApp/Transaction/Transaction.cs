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

                BankAccount? fromAccount = SelectBankAccount(currentUser.CheckingAccounts,currentUser.SavingsAccounts,"Välj konto att överföra från");

                if (fromAccount == null)
                {
                    AnsiConsole.MarkupLine("[yellow]Överföringen avbröts.[/]");
                    return;
                }

                AnsiConsole.WriteLine();
                BankAccount? toAccount = null;

                while (toAccount == null)
                {
                    int bankNumber = AnsiConsole.Prompt(
                        new TextPrompt<int>("[bold white]Ange mottagarens banknummer:[/]")
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

            }

        }
        public void TransferFunds(BankAccount fromAccount, BankAccount toAccount)
        {

            AnsiConsole.MarkupLine($"[bold white]Tillgängligt saldo:[/] [green1]{fromAccount.AccountBalance:N2}[/][bold white] {fromAccount.Currency}[/]");
            AnsiConsole.WriteLine();
            decimal amount = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[bold white]Ange belopp att  överföra:[/]")
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

            // Genomför överföringen
            fromAccount.AccountBalance -= amount;
            toAccount.AccountBalance += amount;

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
               


                AnsiConsole.Write(
        new Rule("[bold cyan]TRANSAKTIONSHISTORIK[/]")
            .RuleStyle("cyan")
    );

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
