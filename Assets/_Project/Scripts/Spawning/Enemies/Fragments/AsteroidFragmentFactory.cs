using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Movement.DirectionProviders;
using _Project.Scripts.Movement.RotationProviders;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Common.Pooling;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Initialization;
using _Project.Scripts.Spawning.Enemies.Movement;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Fragments
{
    public class AsteroidFragmentFactory : IAsteroidFragmentFactory
    {
        private readonly ICollisionService _collisionService;
        private readonly GenericPool<AsteroidFragment> _pool;
        private readonly IEnemyMovementConfigurator _movementConfigurator;
        private readonly SpawnBoundaryTracker _boundaryTracker;
        private readonly DefaultEnemyInitializer _initializer;

        [Inject]
        public AsteroidFragmentFactory(
            ICollisionService collisionService,
            GenericPool<AsteroidFragment> pool,
            IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker boundaryTracker,
            DefaultEnemyInitializer initializer)
        {
            _collisionService = collisionService;
            _pool = pool;
            _movementConfigurator = movementConfigurator;
            _boundaryTracker = boundaryTracker;
            _initializer = initializer;
        }

        public void SpawnFragments(Vector2 center, float asteroidSpeed, AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            int count = Random.Range(spawnConfig.MinFragments, spawnConfig.MaxFragments + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnFragment(center, asteroidSpeed, spawnConfig);
            }
        }

        private void SpawnFragment(Vector2 center, float asteroidSpeed, AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            var offset = Random.insideUnitCircle * 0.5f;
            var pos = center + new Vector2(offset.x, offset.y);

            var direction = (pos - center).normalized;
            if (direction == Vector2.zero) direction = Random.insideUnitSphere.normalized;
            var speed = asteroidSpeed * spawnConfig.FragmentSpeedMultiplier;

            var fragment = _pool.Spawn(pos);
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