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
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;

        private IReadOnlyDictionary<EnemyType, int> _config;
        
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
            if (!TryGetPlayerController(killer, out PlayerController playerController))
                return;

            int points = CalculatePoints(enemy);
            playerController.AddScore(points);
        }

        private int CalculatePoints(IEnemy enemy)
        {
            return _config[enemy.Type];
        }

        private bool TryGetPlayerController(GameObject killer, out PlayerController playerController)
        {
            playerController = null;
            if (killer == null)
                return false;

            if (killer.TryGetComponent(out PlayerController directPlayer))
            {
                playerController = directPlayer;
                return true;
            }

            PlayerController parentPlayer = killer.GetComponentInParent<PlayerController>();
            if (parentPlayer != null)
            {
                playerController = parentPlayer;
                return true;
            }

            if (killer.TryGetComponent(out Projectile projectile))
            {
                PlayerController projectileOwner = projectile.GetComponentInParent<PlayerController>();
                if (projectileOwner != null)
                {
                    playerController = projectileOwner;
                    return true;
                }
            }

            return false;
        }
    }
}
