using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Restarter;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.UI;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.GameState
{
    public class GameStateController : IGameStateController
    {
        private readonly IPlayerController _playerController;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _lifecycleManager;
        private readonly IGameRestarter _gameRestarter;
        private readonly IRestartGameButton _restartButton;

        [Inject]
        public GameStateController(
            IPlayerController playerController,
            IPauseSystem pauseSystem,
            IPoolableLifecycleManager<Pooling_IPoolable> lifecycleManager,
            IGameRestarter gameRestarter,
            IRestartGameButton restartButton)
        {
            _playerController  = playerController;
            _pauseSystem = pauseSystem;
            _lifecycleManager = lifecycleManager;
            _gameRestarter = gameRestarter;
            _restartButton = restartButton;

            _playerController.OnKilled += HandlePlayerDeath;
            _restartButton.OnClick += HandleRestartRequest;
        }

        public void HandlePlayerDeath()
        {
            _pauseSystem.Pause();
            _playerController.Transform.gameObject.SetActive(false);
            _lifecycleManager.ClearAll();
            _restartButton.Show();
        }

        public void HandleRestartRequest()
        {
            _gameRestarter.Restart();
            _restartButton.Hide();
        }
    }
}