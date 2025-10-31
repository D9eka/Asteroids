using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Movement.DirectionProviders;
using _Project.Scripts.Movement.RotationProviders;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Common.Pooling;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Initialization;
using _Project.Scripts.Spawning.Enemies.Movement;
using _Project.Scripts.Spawning.Enemies.Pooling;
using _Project.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Fragments
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