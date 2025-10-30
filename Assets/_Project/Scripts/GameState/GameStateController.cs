using _Project.Scripts.Pause;
using _Project.Scripts.Player;
using _Project.Scripts.Restarter;
using _Project.Scripts.Spawning.Common.Pooling;
using _Project.Scripts.UI;
using Zenject;
using IPoolable = _Project.Scripts.Spawning.Common.Pooling.IPoolable;

namespace _Project.Scripts.GameState
{
    public class GameStateController : IGameStateController
    {
        private readonly IPlayerController _playerController;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<IPoolable> _lifecycleManager;
        private readonly IGameRestarter _gameRestarter;
        private readonly IRestartGameButton _restartButton;

        [Inject]
        public GameStateController(
            IPlayerController playerController,
            IPauseSystem pauseSystem,
            IPoolableLifecycleManager<IPoolable> lifecycleManager,
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