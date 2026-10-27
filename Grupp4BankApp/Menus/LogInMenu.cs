using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;
using Spectre.Console;

namespace Grupp4BankApp.Menus
{
    internal class LogInMenu
    {
        private readonly AuthenticationService authentication;
        public LogInMenu() : this(new AuthenticationService(User.users)) { }
        public LogInMenu(AuthenticationService authenticationService) => authentication = authenticationService;

        public void BankLogo()
        {

            var font = FigletFont.Load("ANSI Regular.flf");
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
        public User? Display()
        {

            BankLogo();
            return LogInUI();
        }
        public User? LogInUI()
        {
            while (true)
            {


                //var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
                //AnsiConsole.MarkupLine($"Hello, [blue]{name}[/]!");

                //Console.Write("Användarnamn: ");
                string? username = AnsiConsole.Ask<string>("[SkyBlue1]Användarnamn:[/] ");

                if (string.IsNullOrWhiteSpace(username))
                {
                    return null;
                }

                //Console.Write("Lösenord: ");
                var temp = new TextPrompt<string>("[SkyBlue1]Lösenord: [/] ").Secret();
                string? password = AnsiConsole.Prompt(temp);

                if (password == null)
                {
                    return null;
                }

                User? user = authentication.Login(username, password);

                if (user != null)
                {
                    return user;
                }
            }
        }
    }
}
