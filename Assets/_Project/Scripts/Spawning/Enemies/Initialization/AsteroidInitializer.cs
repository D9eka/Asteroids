using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Pause;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Fragments;
using _Project.Scripts.Spawning.Enemies.Movement;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class AsteroidInitializer : EnemyInitializer<Asteroid, AsteroidTypeConfig>
    {
        private readonly IAsteroidFragmentFactory _fragmentsFactory;

        [Inject]
        public AsteroidInitializer(ICollisionService collisionService, IEnemyMovementConfigurator movementConfigurator, 
            SpawnBoundaryTracker spawnBoundaryTracker, IPauseSystem pauseSystem, 
            IAsteroidFragmentFactory fragmentsFactory)
            : base(collisionService, movementConfigurator, spawnBoundaryTracker, pauseSystem)
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