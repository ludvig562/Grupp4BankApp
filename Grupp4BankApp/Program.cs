using Grupp4BankApp.Manager;
using Grupp4BankApp.State;
using System.ComponentModel.Design;

namespace Grupp4BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuManager menuManager = new MenuManager();

            menuManager.Update();
            menuManager.Update();
            menuManager.Update();


        }
    }
}
