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
        private readonly IPlayerController _playerController;
        private readonly IPauseSystem _pauseSystem;
        private readonly IScoreSaveHandler _scoreSaveHandler;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly Subject<Unit> _playerDeath = new Subject<Unit>();
        
        public IObservable<Unit> PlayerDeath => _playerDeath;

        [Inject]
        public GameStateController(
            IPlayerController playerController,
            IPauseSystem pauseSystem,
            IScoreSaveHandler scoreSaveHandler,
            IGameplaySessionManager gameplaySessionManager)
        {
            _playerController  = playerController;
            _pauseSystem = pauseSystem;
            _scoreSaveHandler = scoreSaveHandler;
            _gameplaySessionManager = gameplaySessionManager;

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