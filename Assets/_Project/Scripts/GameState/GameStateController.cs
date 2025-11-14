using System;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.GameplayRestart;
using Asteroids.Scripts.SaveService;
using Asteroids.Scripts.UI;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.GameState
{
    public class GameStateController : IGameStateController, IDisposable
    {
        private readonly IPlayerController _playerController;
        private readonly IPauseSystem _pauseSystem;
        private readonly IScoreSaveHandler _scoreSaveHandler;
        private readonly IGameplayRestarterService _gameplayRestarterService;
        private readonly Subject<Unit> _playerDeath = new Subject<Unit>();
        
        public IObservable<Unit> PlayerDeath => _playerDeath;

        [Inject]
        public GameStateController(
            IPlayerController playerController,
            IPauseSystem pauseSystem,
            IScoreSaveHandler scoreSaveHandler,
            IGameplayRestarterService gameplayRestarterService)
        {
            _playerController  = playerController;
            _pauseSystem = pauseSystem;
            _scoreSaveHandler = scoreSaveHandler;
            _gameplayRestarterService = gameplayRestarterService;

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
            _gameplayRestarterService.Restart();
        }

        public void HandleExitRequest()
        {
            _scoreSaveHandler.SaveCurrentScore();
            _gameplayRestarterService.Reset();
        }
    }
}