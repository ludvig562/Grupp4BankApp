using Figgle.Fonts;
namespace Grupp4BankApp.Menus
{
    internal class UserMenu
    {

        List<char> myAnimation = new List<char>();
        public UserMenu( )
        {
            myAnimation.DefaultIfEmpty();
        }
        public void LogInEvent()
        {
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
            LogInEvent();

            Console.WriteLine("This Is UserMenu");
        }



    }
}
