
using Grupp4BankApp.Users;
using Grupp4BankApp.System;
using Spectre.Console;

namespace Grupp4BankApp.Transaction
{
    internal class TransactionStatistics
    {
        public static void ShowStatistics()
        {
            // Get all bank accounts from all customers
            var accounts = User.users.Values
                .OfType<Customer>()
                .SelectMany(c => c.CheckingAccounts
                .Concat(c.SavingsAccounts))
                .ToList();

            // Get only outgoing transactions to avoid duplicates
            var transactions = accounts
                .SelectMany(a => a.TransactionsLogg)
                .Where(t => t.Amount.PlusMinus == "-")
                .ToList();

            AnsiConsole.Write(
                new Rule("[bold cyan]Transaktionsstatistik[/]"));

            if (transactions.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]Inga genomförda transaktioner finns.[/]");
                Console.ReadKey(true);
                return;
            }

            // Calculate total amounts separately for each currency
            var totals = transactions
                .GroupBy(t => t.Amount.Currency)
                .Select(g => new { Currency = g.Key, Total = g.Sum(t => t.Amount.Value) });

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("Statistik")
                .AddColumn("Resultat");

            table.AddRow("Totalt antal överföringar", transactions.Count.ToString());

            foreach (var total in totals)
            {
                table.AddRow($"Totalt överfört ({total.Currency})", $"{total.Total:N2}");
            }

            // Find the largest outgoing transaction
            var largest = transactions
                .OrderByDescending(t => t.Amount.Value)
                .First();

            table.AddRow(
                "Största registrerade belopp",
                $"{largest.Amount.Value:N2} {largest.Amount.Currency}");

            AnsiConsole.Write(table);

            // Find accounts with the most outgoing transfers
            var popularAccounts = accounts
                .Select(a => new { Account = a, Count = a.TransactionsLogg.Count(t => t.Amount.PlusMinus == "-") })
                .Where(a => a.Count > 0)
                .OrderByDescending(a => a.Count)
                .Take(5);

            var popularTable = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold cyan]Mest använda konton[/]")
                .AddColumn("Kontonummer")
                .AddColumn("Kontonamn")
                .AddColumn("Antal överföringar");

            foreach (var item in popularAccounts)
            {
                popularTable.AddRow(item.Account.BankAccountNumber.ToString(),
                    Markup.Escape(item.Account.BankAccountName),
                    item.Count.ToString());
            }

            AnsiConsole.Write(popularTable);

            Console.WriteLine("\nTryck på valfri tangent...");
            Console.ReadKey(true);
        }
    }
}
