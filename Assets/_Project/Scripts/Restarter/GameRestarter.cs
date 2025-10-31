using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Score;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Restarter
{
    public class GameRestarter : IGameRestarter
    {
        private readonly IPlayerController _playerController;
        private readonly Vector2 _playerStartPosition;
        private readonly IScoreService _score;
        private readonly IPauseSystem _pause;

        [Inject]
        public GameRestarter(
            IPlayerController playerController,
            IScoreService score,
            IPauseSystem pause)
        {
            _playerController = playerController;
            _playerStartPosition = playerController.Transform.position;
            _score = score;
            _pause = pause;
        }

        public void Restart()
        {
            _score.ResetScore();
            _playerController.Transform.position = _playerStartPosition;
            _playerController.Transform.gameObject.SetActive(true);

            _pause.Resume();
        }
    }
}