using Grupp4BankApp.State;
using Figgle;
using Figgle.Fonts;
using Microsoft.VisualBasic;
namespace Grupp4BankApp.Menus
{
    internal class UserMenu
    {
        StateStack myState;

        List<char> myAnimation = new List<char>();
        public UserMenu(StateStack aState)
        {
            myState = aState;
            myAnimation.DefaultIfEmpty();
        }
        public void LogInEvent()
        {
            string loadingIn = "Logging in";
            string dots = ". . .";



            for (int j = 0; j < 4; j++)
            {
                Console.Clear();

                Console.WriteLine(FiggleFonts.Larry3d.Render(loadingIn));
                Thread.Sleep(500);
                for (int i = 0; i < dots.Length; i++)
                {
                    Console.Clear();

                    string temp = dots.Substring(0, i + 1);
                    //Larry3d
                    //Standard
                    
                    Console.WriteLine(FiggleFonts.Larry3d.Render(loadingIn + " " + temp));
                  //  Console.WriteLine(FiggleFonts.Standard.Render(loadingIn + " " + temp));


                    Thread.Sleep(300);
                }
            }


        }
        public void Display()
        {
            LogInEvent();

            Console.WriteLine("This Is UserMenu");
        }



    }
}
