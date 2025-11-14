using System;
using System.Collections.Generic;
using Zenject;

namespace Asteroids.Scripts.UI
{
    public class UiControllerInitializer : IInitializable
    {
        private readonly IUIController _uiController;
        private readonly List<IView> _screens;
        private readonly Type _startScreenType;

        public UiControllerInitializer(IUIController uiController, IEnumerable<IView> screens, Type startScreenType)
        {
            _uiController = uiController;
            _screens = new List<IView>(screens);
            _startScreenType = startScreenType;
        }

        public void Initialize()
        {
            _uiController.Initialize(_screens, _startScreenType);
        }
    }
}