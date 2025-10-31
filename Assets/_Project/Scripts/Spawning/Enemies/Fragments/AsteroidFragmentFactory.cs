using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Movement.DirectionProviders;
using Asteroids.Scripts.Movement.RotationProviders;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Fragments
{
    public class AsteroidFragmentFactory : IAsteroidFragmentFactory
    {
        private readonly IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> _enemyProvider;
        private readonly IEnemyMovementConfigurator _movementConfigurator;
        private readonly SpawnBoundaryTracker _boundaryTracker;
        private readonly DefaultEnemyInitializer _initializer;

        [Inject]
        public AsteroidFragmentFactory(
            IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> provider,
            IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker boundaryTracker,
            DefaultEnemyInitializer initializer)
        {
            _enemyProvider = provider;
            _movementConfigurator = movementConfigurator;
            _boundaryTracker = boundaryTracker;
            _initializer = initializer;
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
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector2 direction = (hitDirection + randomOffset).normalized;
    
            if (direction == Vector2.zero) direction = Random.insideUnitSphere.normalized;
            float speed = asteroidSpeed * spawnConfig.FragmentSpeedMultiplier;

            Vector2 pos = center + randomOffset * spawnConfig.FragmentPositionOffsetModefier;
            AsteroidFragment fragment = _enemyProvider.Spawn(pos);
            _initializer.Initialize(fragment, spawnConfig.Config);

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