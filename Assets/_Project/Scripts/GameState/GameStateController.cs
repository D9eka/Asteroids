using System;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Restarter;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.GameState
{
    public class GameStateController : IGameStateController, IDisposable
    {
        private readonly IPlayerController _playerController;
        private readonly IPauseSystem _pauseSystem;
        private readonly IGameRestarter _gameRestarter;
        private readonly IScoreSaveHandler _scoreSaveHandler;
        private readonly Subject<Unit> _playerDeath = new Subject<Unit>();
        
        public IObservable<Unit> PlayerDeath => _playerDeath;

        [Inject]
        public GameStateController(
            IPlayerController playerController,
            IPauseSystem pauseSystem,
            IGameRestarter gameRestarter)
            IScoreSaveHandler scoreSaveHandler,
        {
            _playerController  = playerController;
            _pauseSystem = pauseSystem;
            _gameRestarter = gameRestarter;
            _scoreSaveHandler = scoreSaveHandler;

            _playerController.OnKilled += HandlePlayerDeath;
        }

        public void Dispose()
        {
            _playerController.OnKilled -= HandlePlayerDeath;
        }

        public void HandlePlayerDeath()
        {
            _pauseSystem.Pause();
            _playerController.Transform.gameObject.SetActive(false);
            _playerDeath.OnNext(Unit.Default);
        }

        public void HandleRestartRequest()
        {
            _gameRestarter.Restart();
            _scoreSaveHandler.SaveCurrentScore();
        }
    }
}