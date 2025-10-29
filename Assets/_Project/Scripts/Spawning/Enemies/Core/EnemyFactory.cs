using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Initialization;
using _Project.Scripts.Spawning.Enemies.Movement;
using _Project.Scripts.Spawning.Enemies.Providers;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Core
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly List<IEnemyProvider> _enemyProviders;
        private readonly SpawnPointGenerator _spawnPointGenerator;
        private readonly IEnemyMovementConfigurator _movementConfigurator;
        private readonly SpawnBoundaryTracker _spawnBoundaryTracker;
        private readonly List<IEnemyInitializerBase> _initializers;

        [Inject]
        public EnemyFactory(
            List<IEnemyProvider> enemyProviders,
            SpawnPointGenerator spawnPointGenerator,
            IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker spawnBoundaryTracker,
            List<IEnemyInitializerBase> initializers)
        {
            _enemyProviders = enemyProviders;
            _spawnPointGenerator = spawnPointGenerator;
            _movementConfigurator = movementConfigurator;
            _spawnBoundaryTracker = spawnBoundaryTracker;
            _initializers = initializers;
        }

        public IEnemy Spawn()
        {
            var provider = ChooseRandomProvider();
            var (enemy, config) = SpawnFromProvider(provider);

            SetupEnemy(enemy, config);
            return enemy;
        }

        private (IEnemy enemy, EnemyTypeConfig config) SpawnFromProvider(IEnemyProvider provider)
        {
            if (provider is not IPooledEnemyProvider<IEnemy, EnemyTypeConfig> pooledProvider)
                throw new InvalidOperationException("Provider must be pooled");

            var config = pooledProvider.Config;
            var spawnPos = _spawnPointGenerator.GetRandomPositionOutsideBounds(config.SpawnDistanceOutsideBounds);
            var enemy = pooledProvider.Spawn(spawnPos);

            return (enemy, config);
        }

        private void SetupEnemy(IEnemy enemy, EnemyTypeConfig config)
        {
            _spawnBoundaryTracker.RegisterObject(enemy.Transform);
            _movementConfigurator.Configure(enemy, enemy.Transform.position, config);

            foreach (var initializer in _initializers)
            {
                if (initializer.CanInitialize(enemy))
                {
                    initializer.Initialize(enemy, config);
                    break;
                }
            }
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