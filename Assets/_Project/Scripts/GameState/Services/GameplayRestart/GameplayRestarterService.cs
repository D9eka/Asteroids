using Asteroids.Scripts.Core;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.SaveService;
using Asteroids.Scripts.Score;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.GameplayRestart
{
    public class GameplayRestarterService : IGameplayRestarterService
    {
        private readonly IPlayerController _playerController;
        private readonly Vector2 _playerStartPosition;
        private readonly IScoreService _score;
        private readonly IPauseSystem _pause;
        private readonly IPoolableLifecycleManager<IPoolable> _lifecycleManager;

        [Inject]
        public GameplayRestarterService(
            IPlayerController playerController,
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerStartPosition,
            IScoreService score,
            IPauseSystem pause,
            IPoolableLifecycleManager<IPoolable> lifecycleManager)
        {
            _playerController = playerController;
            _playerStartPosition = playerStartPosition;
            _score = score;
            _pause = pause;
            _lifecycleManager = lifecycleManager;
        }

        public void Reset()
        {
            _score.ResetScore();
            _lifecycleManager.ClearAll();
            
            _playerController.Transform.position = _playerStartPosition;
        }

        public void Restart()
        {
            Reset();

            _pause.Resume();
        }
    }
}