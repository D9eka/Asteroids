using System;
using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Player;
using _Project.Scripts.Weapons.Projectile;
using UnityEngine;

namespace _Project.Scripts.Score
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
            Debug.Log($"[{killer.name}] killed [{enemy.Transform.name}] +{points} pts (Total: {TotalScore})");
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