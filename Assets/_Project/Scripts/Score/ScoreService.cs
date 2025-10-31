using System;
using System.Collections.Generic;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public class ScoreService : IScoreService
    {
        public event Action<int> OnScoreAdded;
        
        private readonly IReadOnlyDictionary<EnemyType, int> _config;
        
        private int _totalScore;

        public int TotalScore
        {
            get => _totalScore;
            set
            {
                if (value == _totalScore) return;
                _totalScore = value;
                OnScoreAdded?.Invoke(value);
            }
        }

        public ScoreService(ScoreConfig config)
        {
            _config = config.ScoreByConfig;
        }

        public void AddScore(GameObject killer, IEnemy enemy)
         {
            if (!CanAddScoreToKiller(killer)) return;
            
            int points = CalculatePoints(enemy);
            TotalScore += points;
        }

        public void ResetScore()
        {
            TotalScore = 0;
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