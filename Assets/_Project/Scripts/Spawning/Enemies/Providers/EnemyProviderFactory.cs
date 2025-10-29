using System;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Common.Pooling;
using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public class EnemyProviderFactory<TEnemy, TConfig> : IEnemyProviderFactory
        where TEnemy : MonoBehaviour, IEnemy
        where TConfig : EnemyTypeConfig
    {
        private readonly DiContainer _container;

        public Type ConfigType => typeof(TConfig);

        public EnemyProviderFactory(DiContainer container)
        {
            _container = container;
        }

        public IEnemyProvider Create(EnemyTypeConfig config)
        {
            var typedConfig = (TConfig)config;

            _container.BindMemoryPool<TEnemy, GenericPool<TEnemy>>()
                .WithInitialSize(typedConfig.PoolSize)
                .FromComponentInNewPrefab(typedConfig.Prefab)
                .UnderTransformGroup($"{typeof(TEnemy).Name}s");

            var pool = _container.Resolve<GenericPool<TEnemy>>();
            return new PooledEnemyProvider<TEnemy, TConfig>(pool, typedConfig);
        }
    }
}