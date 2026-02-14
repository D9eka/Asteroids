using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Leopotam.EcsLite;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public class AsteroidInitializer : EnemyInitializer<Asteroid, AsteroidTypeConfig>
    {
        private readonly IAsteroidFragmentFactory _fragmentsFactory;

        [Inject]
        public AsteroidInitializer(EcsWorld ecsWorld, EnemyCollisionService collisionService, IEnemyMovementConfigurator movementConfigurator, 
            ISpawnBoundaryTracker spawnBoundaryTracker, IPauseSystem pauseSystem, 
            IAsteroidFragmentFactory fragmentsFactory, EntityViewRegistry entityViewRegistry)
            : base(ecsWorld, collisionService, movementConfigurator, spawnBoundaryTracker, pauseSystem, entityViewRegistry)
        {
            _fragmentsFactory = fragmentsFactory;
        }
        
        public override void Initialize(Asteroid asteroid, AsteroidTypeConfig config)
        {
            base.Initialize(asteroid, config);
            if (asteroid.Initialized) return;
            asteroid.Initialize(_fragmentsFactory, config);
        }
    }
}