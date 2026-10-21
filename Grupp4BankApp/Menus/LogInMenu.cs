using Figgle.Fonts;
using Grupp4BankApp.System;
using Grupp4BankApp.Users;

namespace Grupp4BankApp.Menus
{
    internal class LogInMenu
    {
        private readonly AuthenticationService authentication;
        public LogInMenu()
        {
            authentication = new AuthenticationService(User.users);
        }
      
        public void BankLogo()
        {
            Console.WriteLine(FiggleFonts.Starwars.Render("N E T 26 Bank"));
        }
        public void Display()
        {
            BankLogo();
            LogInUI();

        }

        public void LogInUI()
        {
            
            User? tempUser = null;
            while (tempUser == null)
            {
                Console.Write("Användarnamn: ");
                string username = Console.ReadLine() ?? "";

                Console.Write("Lösenord: ");
                string password = Console.ReadLine() ?? "";

                tempUser = authentication.Login(username, password);
            }
            
        }
    }
}
