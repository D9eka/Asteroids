using System.Collections.Generic;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Weapons.Projectile;
using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public class ScoreService : IScoreService
    {
        private readonly IReadOnlyDictionary<EnemyType, int> _config;
        private readonly ReactiveProperty<int> _totalScore = new ReactiveProperty<int>(0);

        public IReadOnlyReactiveProperty<int> TotalScore => _totalScore;

        public ScoreService(ScoreConfig config)
        {
            _config = config.ScoreByConfig;
        }

        public void AddScore(GameObject killer, IEnemy enemy)
         {
            if (!CanAddScoreToKiller(killer)) return;
            
            int points = CalculatePoints(enemy);
            _totalScore.Value += points;
        }

        public void ResetScore()
        {
            _totalScore.Value = 0;
        }

        private int CalculatePoints(IEnemy enemy)
        {
            return _config[enemy.Type];
        }

        private bool CanAddScoreToKiller(GameObject killer)
        {
            return killer.TryGetComponent<IPlayerController>(out _) ||
                   (killer.TryGetComponent(out Projectile projectile) &&
                    projectile.TryGetComponent<IPlayerController>(out _));
        }
    }
}