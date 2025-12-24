using System;
using System.Threading.Tasks;
using Asteroids.Scripts.Addressable;
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
        private readonly IAddressableLoader _addressableLoader;
        private readonly IEnemyLifecycleManager _lifecycleManager;

        public EnemyProviderFactory(DiContainer container, IAddressableLoader addressableLoader,
            IEnemyLifecycleManager lifecycleManager)
        {
            _container = container;
            _addressableLoader = addressableLoader;
            _lifecycleManager = lifecycleManager;
        }

        public async Task<IEnemyProvider> Create(EnemyTypeSpawnConfig spawnConfig)
        {
            TConfig typedConfig = spawnConfig.Config as TConfig;
            if (typedConfig == null)
                throw new ArgumentException($"Config is not of type {typeof(TConfig)}");
            
            Task<GameObject> task = _addressableLoader.Load<GameObject>(spawnConfig.Config.PrefabId);
            await task;

            _container.BindMemoryPool<TEnemy, ObjectPool<TEnemy>>()
                .WithInitialSize(spawnConfig.PoolSize)
                .FromComponentInNewPrefab(task.Result)
                .UnderTransformGroup($"{typeof(TEnemy).Name}s");

            var pool = _container.Resolve<ObjectPool<TEnemy>>();
            return new PooledEnemyProvider<TEnemy, EnemyTypeSpawnConfig>(_lifecycleManager, pool, spawnConfig);
        }
    }
}