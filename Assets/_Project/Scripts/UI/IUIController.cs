using System;
using System.Collections.Generic;
using Asteroids.Scripts.UI.Screens;

namespace Asteroids.Scripts.UI
{
    public interface IUIController
    {
        public void Initialize(List<IView> screens, Type startScreenType);
        public void OpenScreen(IView view);
        public void CloseScreen(IView view);
    }
}