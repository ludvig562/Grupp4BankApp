using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.State
{
    enum eStates
    {
        eLogInMenu,
        eUserMenu,
        eAdminMenu,

    }
    internal class StateStack
    {

        public Stack<eStates> myStateStack = new Stack<eStates>();  
        
        public StateStack()
        {
            myStateStack.Push(eStates.eLogInMenu);
        }

        public eStates GetCurrentStateEnum() 
        { 
            return myStateStack.Peek();
        }

        public void Push(eStates aNewState)
        {
             myStateStack.Push(aNewState);
        }

        public void Pop()
        {
            if (myStateStack.Count > 1)
            {
                myStateStack.Pop();
            }

        }


    }
}
