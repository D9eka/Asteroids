using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.UI.Screens;
using Asteroids.Scripts.UI.Screens.EndGameScreen;
using Asteroids.Scripts.UI.Screens.GameplayScreen;
using Asteroids.Scripts.UI.Screens.MainScreen;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.UI
{
    public class ScreensInitializer : IInitializable
    {
        private readonly IUIController _uiController;
        private readonly Type _startScreenType;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly DiContainer _container;
        private readonly UnityEngine.Camera _camera;
        private readonly IAddressableLoader _addressableLoader;
        private readonly ReviveFlowController _reviveFlowController;
        private readonly List<IView> _screens = new ();

        public ScreensInitializer(IUIController uiController, Type startScreenType,
            IGameplaySessionManager gameplaySessionManager, DiContainer container, UnityEngine.Camera camera, 
            IAddressableLoader addressableLoader, ReviveFlowController reviveFlowController)
        {
            _uiController = uiController;
            _startScreenType = startScreenType;
            _gameplaySessionManager = gameplaySessionManager;
            _container = container;
            _camera = camera;
            _addressableLoader = addressableLoader;
            _reviveFlowController = reviveFlowController;
        }

        public async void Initialize()
        {
            try
            {
                await BindScreen<ReviveScreenView>(AddressableId.ReviveScreen, ScreenInjectId.ReviveScreenView);
                await BindScreen<GameplayScreenView>(AddressableId.GameplayScreen, ScreenInjectId.GameplayScreenView);
                await BindScreen<MainScreenView>(AddressableId.MainScreen, ScreenInjectId.MainScreenView);
            
                _uiController.Initialize(_screens, _startScreenType);
                foreach (IView screen in _screens)
                {
                    if (screen is GameplayScreenView gameplayScreenView)
                    {
                        _gameplaySessionManager.Initialize(gameplayScreenView);
                    }

                    if (screen is ReviveScreenView reviveScreenView)
                    {
                        _reviveFlowController.Initialize(reviveScreenView);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async Task BindScreen<TView>(AddressableId screenAddressableId, ScreenInjectId screenId)
            where TView : IView
        {
            Task<GameObject> task = _addressableLoader.Load<GameObject>(screenAddressableId);
            await task;
            GameObject screenGo = _container.InstantiatePrefab(task.Result);
            
            TView screen = screenGo.GetComponent<TView>();
            _screens.Add(screen);

            _container.Bind<IView>().FromInstance(screen).AsCached();
            _container.Bind<IView>()
                .WithId(screenId)
                .FromInstance(screen)
                .AsCached();
            
            screenGo.GetComponent<Canvas>().worldCamera = _camera;

            await Task.CompletedTask;
        }
    }
}