using System;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.SaveService;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.GameState
{
    public class GameStateController : IGameStateController, IDisposable
    {
        private readonly IPauseSystem _pauseSystem;
        private readonly IScoreTracker _scoreSaveHandler;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly Subject<Unit> _playerDeath = new Subject<Unit>();
        private readonly Subject<Unit> _playerRevive = new Subject<Unit>();
        
        private IPlayerController _playerController;
        
        public IObservable<Unit> PlayerDeath => _playerDeath;
        public IObservable<Unit> PlayerRevive => _playerRevive;

        [Inject]
        public GameStateController(
            IPauseSystem pauseSystem,
            IScoreTracker scoreSaveHandler,
            IGameplaySessionManager gameplaySessionManager)
        {
            _pauseSystem = pauseSystem;
            _scoreSaveHandler = scoreSaveHandler;
            _gameplaySessionManager = gameplaySessionManager;
        }

        public void Initialize(IPlayerController playerController)
        {
            _playerController = playerController;
            _playerController.OnKilled += HandlePlayerDeath;
        }

        public void Dispose()
        {
            _playerController.OnKilled -= HandlePlayerDeath;
        }

        public void HandlePlayerDeath()
        {
            _pauseSystem.Pause();
            _playerDeath.OnNext(Unit.Default);
        }
        
        public void HandleRevivalRequest()
        {
            _pauseSystem.Resume();
            _playerRevive.OnNext(Unit.Default);
        }

        public void HandleRestartRequest()
        {
            _scoreSaveHandler.SaveCurrentScore();
            _gameplaySessionManager.Restart();
        }

        public void HandleExitRequest()
        {
            _scoreSaveHandler.SaveCurrentScore();
            _gameplaySessionManager.Reset();
        }
    }
}