using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;

namespace Grupp4BankApp.Menus
{
    internal class LogInMenu
    {
        private readonly AuthenticationService authentication;
        public LogInMenu() : this(new AuthenticationService(User.users)) { }
        public LogInMenu(AuthenticationService authenticationService) => authentication = authenticationService;

        public void BankLogo() => Console.WriteLine(FiggleFonts.Starwars.Render("N E T 26 Bank"));
        public User? Display()
        {
            BankLogo();
            return LogInUI();
        }
        public User? LogInUI()
        {
            while (true)
            {
                Console.Write("Användarnamn: ");
                string? username = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username)) 
                {
                    return null;
                }

                Console.Write("Lösenord: ");
                string? password = Console.ReadLine();

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
