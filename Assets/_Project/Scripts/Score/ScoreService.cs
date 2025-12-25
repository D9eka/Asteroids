using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Weapons.Projectile;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Score
{
    public class ScoreService : IScoreService, IInitializable, IDisposable
    {
        private readonly ReactiveProperty<int> _totalScore = new ReactiveProperty<int>(0);
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;

        private IReadOnlyDictionary<EnemyType, int> _config;
        
        public IReadOnlyReactiveProperty<int> TotalScore => _totalScore;

        public ScoreService(IEnemyLifecycleManager enemyLifecycleManager)
        {
            _enemyLifecycleManager = enemyLifecycleManager;
        }

        public void Initialize()
        {
            _enemyLifecycleManager.OnEnemyKilled += AddScore;
        }

        public void Dispose()
        {
            _enemyLifecycleManager.OnEnemyKilled -= AddScore;
        }
        
        public void ApplyConfig(ScoreConfig scoreConfig)
        {
            _config = scoreConfig.ScoreByConfig;
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