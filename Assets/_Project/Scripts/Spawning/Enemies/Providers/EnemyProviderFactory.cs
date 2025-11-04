using System;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public class EnemyProviderFactory<TEnemy, TConfig> : IEnemyProviderFactory
        where TEnemy : MonoBehaviour, IEnemy
        where TConfig : EnemyTypeConfig
    {
        private readonly DiContainer _container;
        private readonly IEnemyLifecycleManager _lifecycleManager;
        private readonly EnemyType _enemyType;

        public EnemyType EnemyType => _enemyType;

        public EnemyProviderFactory(DiContainer container, IEnemyLifecycleManager lifecycleManager, EnemyType enemyType)
        {
            _container = container;
            _lifecycleManager = lifecycleManager;
            _enemyType = enemyType;
        }

        public IEnemyProvider Create(EnemyTypeSpawnConfig spawnConfig)
        {
            var typedConfig = spawnConfig.Config as TConfig;
            if (typedConfig == null)
                throw new ArgumentException($"Config is not of type {typeof(TConfig)}");

            _container.BindMemoryPool<TEnemy, ObjectPool<TEnemy>>()
                .WithInitialSize(spawnConfig.PoolSize)
                .FromComponentInNewPrefab(typedConfig.Prefab)
                .UnderTransformGroup($"{typeof(TEnemy).Name}s");

            var pool = _container.Resolve<ObjectPool<TEnemy>>();
            return new PooledEnemyProvider<TEnemy, EnemyTypeSpawnConfig>(_lifecycleManager, pool, spawnConfig);
        }
    }
}