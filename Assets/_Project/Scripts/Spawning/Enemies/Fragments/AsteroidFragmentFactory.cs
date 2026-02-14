using Asteroids.Scripts.Ecs.Components;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Fragments
{
    public class AsteroidFragmentFactory : IAsteroidFragmentFactory
    {
        private readonly EcsWorld _ecsWorld;
        private readonly IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> _enemyProvider;
        private readonly IEnemyMovementConfigurator _movementConfigurator;
        private readonly ISpawnBoundaryTracker _boundaryTracker;
        private readonly DefaultEnemyInitializer _initializer;

        [Inject]
        public AsteroidFragmentFactory(
            EcsWorld ecsWorld,
            IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> provider,
            IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker boundaryTracker,
            DefaultEnemyInitializer initializer)
        {
            _ecsWorld = ecsWorld;
            _enemyProvider = provider;
            _movementConfigurator = movementConfigurator;
            _boundaryTracker = boundaryTracker;
            _initializer = initializer;
        }

        public void SpawnFragments(Vector2 center, Vector2 hitDirection, int asteroidId, 
            AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            EcsPool<VelocityMovementComponent> velocityMovementComponentPool = _ecsWorld.GetPool<VelocityMovementComponent>();
            float asteroidSpeed = velocityMovementComponentPool.Get(asteroidId).Velocity;
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

            Vector2 pos = center + randomOffset * spawnConfig.FragmentPositionOffsetModifier;
            AsteroidFragment fragment = _enemyProvider.Spawn(pos);
            _initializer.Initialize(fragment, spawnConfig.Config);
            
            EcsPool<LinearDirectionComponent> linearDirectionComponentPool = _ecsWorld.GetPool<LinearDirectionComponent>();
            ref LinearDirectionComponent linearDirectionComponent = ref linearDirectionComponentPool.Get(fragment.Id);
            linearDirectionComponent.Direction = direction;

            EcsPool<VelocityMovementComponent> velocityMovementComponentPool = _ecsWorld.GetPool<VelocityMovementComponent>();
            ref VelocityMovementComponent velocityMovementComponent = ref velocityMovementComponentPool.Get(fragment.Id);
            velocityMovementComponent.Velocity = speed;
        }
    }
}