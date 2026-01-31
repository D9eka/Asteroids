using System;
using System.Collections.Generic;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.UI.Screens;
using Asteroids.Scripts.UI.Screens.GameplayScreen;
using Cysharp.Threading.Tasks;
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
        private readonly IResourcesLoader _resourcesLoader;
        private readonly List<IView> _screens = new ();

        public ScreensInitializer(IUIController uiController, Type startScreenType,
            IGameplaySessionManager gameplaySessionManager, DiContainer container, UnityEngine.Camera camera, 
            IResourcesLoader resourcesLoader)
        {
            _uiController = uiController;
            _startScreenType = startScreenType;
            _gameplaySessionManager = gameplaySessionManager;
            _container = container;
            _camera = camera;
            _resourcesLoader = resourcesLoader;
        }

        public async void Initialize()
        {
            try
            {
                await BindScreen<GameplayScreenView>(ResourceObjectId.GameplayScreen, ScreenInjectId.GameplayScreenView);
            
                _uiController.Initialize(_screens, _startScreenType);
                foreach (IView screen in _screens)
                {
                    if (screen is GameplayScreenView gameplayScreenView)
                    {
                        _gameplaySessionManager.Initialize(gameplayScreenView);
                    }
                }

                if (_startScreenType == typeof(GameplayScreenView))
                {
                    _gameplaySessionManager.Start();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async UniTask BindScreen<TView>(ResourceObjectId screenResourceObjectId, ScreenInjectId screenId)
            where TView : IView
        {
            GameObject screenPrefab = await _resourcesLoader.Load(screenResourceObjectId);
            GameObject screenGo = _container.InstantiatePrefab(screenPrefab);
            
            TView screen = screenGo.GetComponentInChildren<TView>(true);
            _screens.Add(screen);

            _container.Bind<IView>().FromInstance(screen).AsCached();
            _container.Bind<IView>()
                .WithId(screenId)
                .FromInstance(screen)
                .AsCached();
            
            screenGo.GetComponent<Canvas>().worldCamera = _camera;
        }
    }
}
