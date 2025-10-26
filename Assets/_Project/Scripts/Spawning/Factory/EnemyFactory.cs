using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Config;
using _Project.Scripts.Spawning.Core;
using _Project.Scripts.Spawning.Movement;
using _Project.Scripts.Spawning.Providers;
using UnityEngine;

namespace _Project.Scripts.Spawning.Factory
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly List<IEnemyProvider> _enemyProviders;
        private readonly EnemySpawnPointGenerator _spawnPointGenerator;
        private readonly EnemyMovementConfigurator _movementConfigurator;
        private readonly SpawnBoundaryTracker _spawnBoundaryTracker;

        public EnemyFactory(
            List<IEnemyProvider> enemyProviders,
            EnemySpawnPointGenerator spawnPointGenerator,
            EnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker spawnBoundaryTracker)
        {
            _enemyProviders = enemyProviders;
            _spawnPointGenerator = spawnPointGenerator;
            _movementConfigurator = movementConfigurator;
            _spawnBoundaryTracker = spawnBoundaryTracker;
        }

        public IEnemy Spawn()
        {
            IEnemyProvider provider = ChooseRandomProvider();

            if (provider is not IPooledEnemyProvider pooledProvider)
                throw new InvalidOperationException($"Provider {provider.GetType().Name} не поддерживает EnemyTypeConfig");

            EnemyTypeConfig config = pooledProvider.Config;
            Vector2 spawnPos = _spawnPointGenerator.GetRandomPositionOutsideBounds(config.SpawnDistanceOutsideBounds);

            IEnemy enemy = provider.Spawn(spawnPos);

            if (enemy is MonoBehaviour mono)
                _spawnBoundaryTracker.RegisterObject(mono.transform);

            _movementConfigurator.Configure(enemy, spawnPos, config);

            return enemy;
        }

        private IEnemyProvider ChooseRandomProvider()
        {
            float totalWeight = _enemyProviders.Sum(p => p.Probability);
            float roll = UnityEngine.Random.value * totalWeight;

            float cumulative = 0;
            foreach (var provider in _enemyProviders)
            {
                cumulative += provider.Probability;
                if (roll <= cumulative)
                    return provider;
            }

            return _enemyProviders.Last();
        }
    }
}
