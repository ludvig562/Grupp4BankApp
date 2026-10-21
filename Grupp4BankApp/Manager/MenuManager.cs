using Grupp4BankApp.Menus;

namespace Grupp4BankApp.Manager
{
    enum eMenus
    {
        eLogInMenu,
        eUserMenu,
        eAdminMenu,

    }
    internal static class MenuManager
    {


       // static private StateStack myState = new StateStack();

        static private LogInMenu? myLogInMenu;
        static private CustomerMenu? myUserMenu;
        static private AdminMenu? myAdminMenu;

        public static void Init()
        {
            myLogInMenu = new LogInMenu();
            myUserMenu = new CustomerMenu();
            myAdminMenu = new AdminMenu();
        }

        public static void SelectMenu(eMenus aState)
        {

            switch (aState)
            {
                case eMenus.eLogInMenu:
                    if (myLogInMenu != null)
                        myLogInMenu.Display();
                    break;
                case eMenus.eUserMenu:
                    if (myUserMenu != null)
                        myUserMenu.Display();

                    break;
                case eMenus.eAdminMenu:
                    if (myAdminMenu != null)
                        myAdminMenu.Display();

                    break;
                default:
                    break;
            }
        }

        //public static void Update()
        //{

        //    switch (myState.GetCurrentStateEnum())
        //    {
        //        case eMenus.eLogInMenu:
        //            if (myLogInMenu != null)
        //                myLogInMenu.Display();
        //            break;
        //        case eMenus.eUserMenu:
        //            if (myUserMenu != null)
        //                myUserMenu.Display();

        //            break;
        //        case eMenus.eAdminMenu:
        //            if (myAdminMenu != null)
        //                myAdminMenu.Display();

        //            break;
        //        default:
        //            break;
        //    }
        //}
        //public static void PushMenu(eMenus menu)
        //{
        //    myState.Push(menu);
        //    Update();

        //}

    }
}
