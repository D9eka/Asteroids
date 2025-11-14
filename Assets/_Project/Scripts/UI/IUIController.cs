using System;
using System.Collections.Generic;

namespace Asteroids.Scripts.UI
{
    public interface IUIController
    {
        public void OpenScreen(IView view);
        public void CloseScreen(IView view);
        public void Initialize(List<IView> screens, Type startScreenType);
    }
}