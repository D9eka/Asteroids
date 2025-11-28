using System;
using System.Collections.Generic;
using Asteroids.Scripts.UI.Screens;

namespace Asteroids.Scripts.UI
{
    public class UIController : IUIController
    {
        private readonly Stack<IView> _screenStack = new Stack<IView>();
        
        public void Initialize(List<IView> screens, Type startScreenType)
        {
            foreach (var screen in screens)
            {
                if (screen.GetType() == startScreenType)
                {
                    OpenScreen(screen);
                }
                else
                {
                    screen.Hide();
                }
            }
        }

        public void OpenScreen(IView view)
        {
            if (_screenStack.Count > 0)
            {
                IView currentScreen = _screenStack.Peek();
                if (currentScreen == view) return;
                currentScreen.Hide();
            }

            _screenStack.Push(view);
            view.Show();
        }

        public void CloseScreen(IView view)
        {
            if (_screenStack.Count == 0) return;
            if (_screenStack.Peek() != view) return;

            _screenStack.Pop();
            view.Hide();

            if (_screenStack.Count > 0)
            {
                _screenStack.Peek().Show();
            }
        }
    }
}