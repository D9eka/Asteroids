using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Core
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly SpawnPointGenerator _spawnPointGenerator;
        private readonly List<IEnemyInitializerBase> _initializers;

        [Inject]
        public EnemyFactory(
            List<IEnemyProvider> enemyProviders,
            SpawnPointGenerator spawnPointGenerator,
            List<IEnemyInitializerBase> initializers)
        {
            _spawnPointGenerator = spawnPointGenerator;
            _initializers = initializers;
        }

        public void Spawn(IEnemyProvider provider)
        {
            (IEnemy enemy, EnemyTypeSpawnConfig config) = SpawnFromProvider(provider);
            SetupEnemy(enemy, config);
        }

        private (IEnemy enemy, EnemyTypeSpawnConfig config) SpawnFromProvider(IEnemyProvider provider)
        {
            if (provider is not IPooledEnemyProvider<IEnemy, EnemyTypeSpawnConfig> pooledProvider)
                throw new InvalidOperationException("Provider must be pooled");

            EnemyTypeSpawnConfig spawnConfig = pooledProvider.Config;
            Vector2 spawnPos = _spawnPointGenerator.GetRandomPositionOutsideBounds(spawnConfig.SpawnDistanceOutsideBounds);
            IEnemy enemy = pooledProvider.Spawn(spawnPos);

            return (enemy, spawnConfig);
        }

        private void SetupEnemy(IEnemy enemy, EnemyTypeSpawnConfig spawnConfig)
        {
            foreach (IEnemyInitializerBase initializer in _initializers)
            {
                if (initializer.CanInitialize(enemy))
                {
                    initializer.Initialize(enemy, spawnConfig);
                    break;
                }
            }
        }
    }
}