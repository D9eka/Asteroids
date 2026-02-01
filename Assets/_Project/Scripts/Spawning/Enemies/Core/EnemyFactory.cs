using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using UnityEngine;
using Zenject;
using _Project.Scripts.Multiplayer;
using Fusion;

namespace Asteroids.Scripts.Spawning.Enemies.Core
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly SpawnPointGenerator _spawnPointGenerator;
        private readonly List<IEnemyInitializerBase> _initializers;
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;

        [Inject]
        public EnemyFactory(
            List<IEnemyProvider> enemyProviders,
            SpawnPointGenerator spawnPointGenerator,
            List<IEnemyInitializerBase> initializers,
            NetworkEventsRouter networkEventsRouter,
            IEnemyLifecycleManager enemyLifecycleManager)
        {
            _spawnPointGenerator = spawnPointGenerator;
            _initializers = initializers;
            _networkEventsRouter = networkEventsRouter;
            _enemyLifecycleManager = enemyLifecycleManager;
        }

        public void Spawn(IEnemyProvider provider)
        {
            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null || !runner.IsServer)
                return;

            (IEnemy enemy, EnemyTypeSpawnConfig config) = SpawnFromProvider(provider);
            SetupEnemy(enemy, config);
            _enemyLifecycleManager.Register(enemy, null);
        }

        private (IEnemy enemy, EnemyTypeSpawnConfig config) SpawnFromProvider(IEnemyProvider provider)
        {
            if (provider is not IPooledEnemyProvider<IEnemy, EnemyTypeSpawnConfig> pooledProvider)
                throw new InvalidOperationException("Provider must be pooled");

            EnemyTypeSpawnConfig spawnConfig = pooledProvider.Config;
            Vector2 spawnPos = _spawnPointGenerator.GetRandomPositionOutsideBounds(spawnConfig.SpawnDistanceOutsideBounds);
            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null)
                throw new InvalidOperationException("NetworkRunner is not available for enemy spawn");

            if (pooledProvider.Prefab == null)
                throw new InvalidOperationException("Enemy prefab is not set for pooled provider");

            NetworkObject obj = runner.Spawn(
                pooledProvider.Prefab,
                spawnPos,
                Quaternion.identity);

            IEnemy enemy = obj.GetComponent<IEnemy>();

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
