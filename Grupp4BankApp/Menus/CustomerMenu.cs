using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Transaction;
using Grupp4BankApp.Users;
using Spectre.Console;
using System.Security.Cryptography.X509Certificates;
namespace Grupp4BankApp.Menus
{
    internal class CustomerMenu
    {
        private Customer? currentUser;
        AuthenticationService Auth = new();
        Loan loans = new();
        TransactionSystem transactionSystem = new TransactionSystem();
        List<char> myAnimation = new List<char>();
        public CustomerMenu()
        {
            myAnimation.DefaultIfEmpty();
        }
        public void LogInEvent()
        {
            AnsiConsole.Live(new Text("Initializing..."))
            .Start(ctx =>
            {
                Thread.Sleep(1000);

                // Replace with a panel
                var panel = new Panel("Loading configuration...")
                .Header("Step 1")
                .BorderColor(Color.Blue);
                ctx.UpdateTarget(panel);
                Thread.Sleep(1000);

                // Replace with a different panel
                var panel2 = new Panel("Connecting to database...")
                .Header("Step 2")
                .BorderColor(Color.Yellow);
                ctx.UpdateTarget(panel2);
                Thread.Sleep(1000);

                // Replace with final panel
                var panel3 = new Panel("[green]Ready![/]")
                .Header("Complete")
                .BorderColor(Color.Green);
                ctx.UpdateTarget(panel3);
                Thread.Sleep(1000);
            });
            string loadingIn = "Logging in";
            string dots = ". . .";

            for (int j = 0; j < 3; j++)
            {
                Console.Clear();

                Console.WriteLine(FiggleFonts.Larry3d.Render(loadingIn));
                Thread.Sleep(400);
                for (int i = 0; i < dots.Length; i++)
                {
                    Console.Clear();

                    string temp = dots.Substring(0, i + 1);
                    //Larry3d
                    //Standard

                    Console.WriteLine(FiggleFonts.Larry3d.Render(loadingIn + " " + temp));
                    //  Console.WriteLine(FiggleFonts.Standard.Render(loadingIn + " " + temp));

                    Thread.Sleep(200);
                }
            }

        }
        public void Display()
        {

#if RELEASE

            LogInEvent();
#endif

            Console.Clear();

            RunCustomerMenu();
        }

        public void RunCustomerMenu()
        {
            currentUser = AuthenticationService.CurrentUser as Customer;

            bool customerMenuRunning = true;

            while (customerMenuRunning)
            {
                var panel = new Panel( new Markup(
                    "[bold cyan]Välkommen till banken![/]\n\n" +
                    "[grey]Välj vad du vill göra i menyn.[/]" ))
                {
                    Header = new PanelHeader(" HUVUDMENY "),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(2, 1),
                };

                AnsiConsole.Write(
                    new Align(
                        panel,
                        HorizontalAlignment.Center
                    )
                );

                var picked = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .HighlightStyle(new Style(Color.White, Color.Black))
                        .WrapAround()
                        .AddChoices(
                            "Lista alla konto",
                            "Öppna nya konto",
                            "Sätt in pengar",
                            "Överför pengar",
                            "Låna pengar",
                            "Transaktioner",
                            "Logga ut"
                        )
                );
                switch (picked)
                {
                    case "Lista alla konto":
                        // Gets the logged-in customer and displays their bank accounts
                        if (AuthenticationService.CurrentUser is Customer customer)
                        {
                            customer.ShowBankAccounts();
                        }
                        else
                        {
                            Console.WriteLine("Ingen kund är inloggad.");
                        }

                        break;
                    case "Öppna nya konto":
                        currentUser.OpenBankAccount(); //customer opens bank account
                        break;
                    case "Sätt in pengar":
                        break;
                    case "Överför pengar":
                        Console.Clear();
                        transactionSystem.TransferMoney();
                        break;
                    case "Låna pengar":
                        loans.ShowLoanMenu(); // opens loan menu
                        break;
                    case "Transaktioner":
                        Console.Clear();
                        transactionSystem.TransactionLog();
                        break;
                    case "Logga ut":
                        Console.Clear();
#if RELEASE
                        AnsiConsole.Status()
                       .Spinner(Spinner.Known.Arc)
                       .SpinnerStyle(Color.Red)
                       .Start("Loggas ut...", ctx =>
                        {
                            Thread.Sleep(2000);
                          AnsiConsole.MarkupLine("[green]Du har loggats ut[/]");
                          });

                        Auth.Logout();
#endif
                        return;
                    default:
                        Console.WriteLine("Du måste skriva en siffra mellan 1 - 7");
                        break;
                }

            }
        }


    }
}