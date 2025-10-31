﻿using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public class PooledEnemyProvider<TEnemy, TConfig> : IPooledEnemyProvider<TEnemy, TConfig>
        where TEnemy : MonoBehaviour, IEnemy
        where TConfig : EnemyTypeSpawnConfig
    {
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;
        private readonly GenericPool<TEnemy> _pool;
        
        public TConfig Config { get; private set; }
        public float Probability => Config.SpawnProbability;
        public float SpawnInterval => Config.SpawnInterval;

        public PooledEnemyProvider(IEnemyLifecycleManager enemyLifecycleManager, GenericPool<TEnemy> pool, TConfig config)
        {
            _enemyLifecycleManager = enemyLifecycleManager;
            _pool = pool;
            Config = config;
        }

        TEnemy IPooledEnemyProvider<TEnemy, TConfig>.Spawn(Vector2 position)
        {
            TEnemy enemy = _pool.Spawn(position);
            _enemyLifecycleManager.Register(enemy, _pool);
            return enemy;
        }
    }
}