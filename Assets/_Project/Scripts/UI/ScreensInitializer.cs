using System;
using System.Collections.Generic;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.UI.Screens;
using Asteroids.Scripts.UI.Screens.GameplayScreen;
using Zenject;

namespace Asteroids.Scripts.UI
{
    public class ScreensInitializer : IInitializable
    {
        private readonly IUIController _uiController;
        private readonly List<IView> _screens;
        private readonly Type _startScreenType;
        private readonly IGameplaySessionManager _gameplaySessionManager;

        public ScreensInitializer(IUIController uiController, IEnumerable<IView> screens, Type startScreenType,
            IGameplaySessionManager gameplaySessionManager)
        {
            _uiController = uiController;
            _screens = new List<IView>(screens);
            _startScreenType = startScreenType;
            _gameplaySessionManager = gameplaySessionManager;
        }

        public void Initialize()
        {
            _uiController.Initialize(_screens, _startScreenType);
            foreach (var screen in _screens)
            {
                if (screen is GameplayScreenView gameplayScreenView)
                {
                    _gameplaySessionManager.Initialize(gameplayScreenView);
                }
            }
        }
    }
}