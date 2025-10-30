using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Initialization;
using _Project.Scripts.Spawning.Enemies.Movement;
using _Project.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Core
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly List<IEnemyProvider> _enemyProviders;
        private readonly SpawnPointGenerator _spawnPointGenerator;
        private readonly List<IEnemyInitializerBase> _initializers;

        [Inject]
        public EnemyFactory(
            List<IEnemyProvider> enemyProviders,
            SpawnPointGenerator spawnPointGenerator,
            List<IEnemyInitializerBase> initializers)
        {
            _enemyProviders = enemyProviders;
            _spawnPointGenerator = spawnPointGenerator;
            _initializers = initializers;
        }

        public IEnemy Spawn()
        {
            IEnemyProvider provider = ChooseRandomProvider();
            (IEnemy enemy, EnemyTypeSpawnConfig config) = SpawnFromProvider(provider);
            SetupEnemy(enemy, config);
            return enemy;
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

        private IEnemyProvider ChooseRandomProvider()
        {
            float totalWeight = _enemyProviders.Sum(p => p.Probability);
            float roll = UnityEngine.Random.value * totalWeight;
            float cumulative = 0;

            foreach (IEnemyProvider provider in _enemyProviders)
            {
                cumulative += provider.Probability;
                if (roll <= cumulative)
                    return provider;
            }

            return _enemyProviders.Last();
        }
    }
}