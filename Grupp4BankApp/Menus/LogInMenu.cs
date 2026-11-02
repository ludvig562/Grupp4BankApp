using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using Spectre.Console;

namespace Grupp4BankApp.Menus
{
    internal class LogInMenu
    {
        // Stores the authentication service used for logging in users
        private readonly AuthenticationService authentication;

        // Creates an authentication service using the existing users
        public LogInMenu() : this(new AuthenticationService(User.users)) { }

        // Constructor that receives an authentication service
        public LogInMenu(AuthenticationService authenticationService) => authentication = authenticationService;

        // Displays the bank logo and version number
        public void BankLogo()
        {

            var font = FigletFont.Load("Fonts/ANSI Regular.flf");
            var figlet = new FigletText(font, "NET26 Banken")
            {
                Justification = Justify.Center,
                Color = Color.SkyBlue1,

            };
            var version = new Text("Version 1.0.0", new Style(Color.Grey))
            {
                Justification = Justify.Center,
            };
            var panel = new Panel(figlet)
            {
                Border = BoxBorder.Double,
                BorderStyle = new Style(Color.DeepSkyBlue1),
                Padding = new Padding(1, 1, 1, 1)
            };
            AnsiConsole.Write(panel);
            AnsiConsole.Write(version);
            AnsiConsole.WriteLine();

        }

        // Displays the bank logo and starts the login process
        public User? Display()
        {

            BankLogo();
            return LogInUI();
        }

        // Handles username and password input until login succeeds
        public User? LogInUI()
        {
            while (true)
            {


                //var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
                //AnsiConsole.MarkupLine($"Hello, [blue]{name}[/]!");

                //Console.Write("Användarnamn: ");

                // Asks the user to enter their username
                string? username = AnsiConsole.Ask<string>("[SkyBlue1]Användarnamn:[/] ");

                if (string.IsNullOrWhiteSpace(username))
                {
                    return null;
                }

                //Console.Write("Lösenord: ");

                // Asks for the password and hides it while typing
                var temp = new TextPrompt<string>("[SkyBlue1]Lösenord: [/] ").Secret();
                string? password = AnsiConsole.Prompt(temp);

                // Returns null if no password was entered
                if (password == null)
                {
                    return null;
                }

                // Checks the username and password using AuthenticationService
                User? user = authentication.Login(username, password);

                // Returns the user if the login was successful
                if (user != null)
                {
                    return user;
                }
            }
        }
    }
}
