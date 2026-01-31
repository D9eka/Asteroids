using System;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.GameState
{
    public class GameStateController : IGameStateController, IDisposable
    {
        private readonly IPauseSystem _pauseSystem;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly Subject<Unit> _playerDeath = new Subject<Unit>();
        private readonly Subject<Unit> _playerRevive = new Subject<Unit>();
        
        private IPlayerController _playerController;
        
        public IObservable<Unit> PlayerDeath => _playerDeath;
        public IObservable<Unit> PlayerRevive => _playerRevive;

        [Inject]
        public GameStateController(
            IPauseSystem pauseSystem,
            IGameplaySessionManager gameplaySessionManager)
        {
            _pauseSystem = pauseSystem;
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
            QuitGame();
        }
        
        public void HandleRevivalRequest()
        {
            _pauseSystem.Resume();
            _playerRevive.OnNext(Unit.Default);
        }

        public void HandleRestartRequest()
        {
            _gameplaySessionManager.Restart();
        }

        public void HandleExitRequest()
        {
            _gameplaySessionManager.Reset();
        }

        private static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
