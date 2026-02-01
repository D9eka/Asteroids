using System;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Score;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.UI;
using Asteroids.Scripts.UI.Screens;
using UniRx;
using UnityEngine;
using Zenject;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.GameState.GameplaySession
{
    public class GameplaySessionManager : IGameplaySessionManager
    {
        private readonly Vector2 _playerStartPosition;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _lifecycleManager;
        private readonly IUIController _uiController;
        private readonly Subject<Unit> _gameStarted = new Subject<Unit>();
        
        private IPlayerController _playerController;
        private IView _gameplayView;

        public IObservable<Unit> GameStarted => _gameStarted;

        [Inject]
        public GameplaySessionManager(
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerStartPosition,
            IPauseSystem pauseSystem,
            IPoolableLifecycleManager<Pooling_IPoolable> lifecycleManager, 
            IUIController uiController)
        {
            _playerStartPosition = playerStartPosition;
            _pauseSystem = pauseSystem;
            _lifecycleManager = lifecycleManager;
            _uiController = uiController;
        }

        public void Initialize(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Initialize(IView gameplayView)
        {
            _gameplayView = gameplayView;
        }

        public void Start()
        {
            _pauseSystem.Resume();
            if (_gameplayView != null)
            {
                _uiController.OpenScreen(_gameplayView);
            }
            _gameStarted.OnNext(Unit.Default);
        }

        public void Reset()
        {
            _lifecycleManager.ClearAll();
            
            _playerController.Transform.position = _playerStartPosition;
            _playerController.Transform.rotation = Quaternion.identity;
        }

        public void Restart()
        {
            Reset();
            Start();
        }
    }
}
