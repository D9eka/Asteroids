using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Movement.DirectionProviders;
using Asteroids.Scripts.Movement.RotationProviders;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using _Project.Scripts.Multiplayer;
using Fusion;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Fragments
{
    public class AsteroidFragmentFactory : IAsteroidFragmentFactory
    {
        private readonly IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> _enemyProvider;
        private readonly IEnemyMovementConfigurator _movementConfigurator;
        private readonly ISpawnBoundaryTracker _boundaryTracker;
        private readonly DefaultEnemyInitializer _initializer;
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;

        [Inject]
        public AsteroidFragmentFactory(
            IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> provider,
            IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker boundaryTracker,
            DefaultEnemyInitializer initializer,
            NetworkEventsRouter networkEventsRouter,
            IEnemyLifecycleManager enemyLifecycleManager)
        {
            _enemyProvider = provider;
            _movementConfigurator = movementConfigurator;
            _boundaryTracker = boundaryTracker;
            _initializer = initializer;
            _networkEventsRouter = networkEventsRouter;
            _enemyLifecycleManager = enemyLifecycleManager;
        }

        public void SpawnFragments(Vector2 center, Vector2 hitDirection, float asteroidSpeed, 
            AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            int count = Random.Range(spawnConfig.MinFragments, spawnConfig.MaxFragments + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnFragment(center, hitDirection, asteroidSpeed, spawnConfig);
            }
        }

        private void SpawnFragment(Vector2 center, Vector2 hitDirection, float asteroidSpeed, 
            AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null || !runner.IsServer)
                return;

            if (_enemyProvider.Prefab == null)
            {
                Debug.LogWarning("[AsteroidFragmentFactory] Fragment prefab is not set.");
                return;
            }

            Vector2 randomOffset = Random.insideUnitCircle;
            Vector2 direction = (hitDirection + randomOffset).normalized;
    
            if (direction == Vector2.zero) direction = Random.insideUnitSphere.normalized;
            float speed = asteroidSpeed * spawnConfig.FragmentSpeedMultiplier;

            Vector2 pos = center + randomOffset * spawnConfig.FragmentPositionOffsetModifier;
            NetworkObject obj = runner.Spawn(_enemyProvider.Prefab, pos, Quaternion.identity);
            AsteroidFragment fragment = obj.GetComponent<AsteroidFragment>();
            _initializer.Initialize(fragment, spawnConfig.Config);
            _enemyLifecycleManager.Register(fragment, null);

            IDirectionProvider directionProvider =
                _movementConfigurator.CreateDirectionProvider(spawnConfig.Config.DirectionProviderConfig, direction);
            fragment.Movement.SetDirectionProvider(directionProvider);
            
            IRotationProvider rotationProvider = 
                _movementConfigurator.CreateRotationProvider(spawnConfig.Config.RotationProviderConfig, fragment.Transform);
            fragment.Movement.SetRotationProvider(rotationProvider);
            fragment.Movement.SetVelocity(speed);

            _boundaryTracker.RegisterObject(fragment.Transform);
        }
    }
}
