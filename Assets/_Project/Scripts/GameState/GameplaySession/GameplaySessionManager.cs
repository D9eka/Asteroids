using Asteroids.Scripts.Analytics;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Score;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.UI;
using Asteroids.Scripts.UI.Screens;
using UnityEngine;
using Zenject;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.GameState.GameplaySession
{
    public class GameplaySessionManager : IGameplaySessionManager
    {
        private readonly IPlayerController _playerController;
        private readonly Vector2 _playerStartPosition;
        private readonly IScoreService _score;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _lifecycleManager;
        private readonly IUIController _uiController;
        private readonly IAnalyticsService _analyticsService;
        
        private IView _gameplayView;

        [Inject]
        public GameplaySessionManager(
            IPlayerController playerController,
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerStartPosition,
            IScoreService score,
            IPauseSystem pauseSystem,
            IPoolableLifecycleManager<Pooling_IPoolable> lifecycleManager, 
            IUIController uiController,
            IAnalyticsService analyticsService)
        {
            _playerController = playerController;
            _playerStartPosition = playerStartPosition;
            _score = score;
            _pauseSystem = pauseSystem;
            _lifecycleManager = lifecycleManager;
            _uiController = uiController;
            _analyticsService = analyticsService;
        }

        public void Initialize(IView gameplayView)
        {
            _gameplayView = gameplayView;
        }

        public void Start()
        {
            _score.ResetScore();
            _pauseSystem.Resume();
            _uiController.OpenScreen(_gameplayView);
            _analyticsService.SendStartGameEvent();
        }

        public void Reset()
        {
            _lifecycleManager.ClearAll();
            _analyticsService.SendEndGameEvent();
            
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