using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public abstract class EnemyInitializer<TEnemy, TConfig> : IEnemyInitializer<TEnemy, TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        protected readonly ICollisionService CollisionService;
        protected readonly IEnemyMovementConfigurator MovementConfigurator;
        protected readonly ISpawnBoundaryTracker SpawnBoundaryTracker;
        protected readonly IPauseSystem PauseSystem;

        [Inject]
        public EnemyInitializer(ICollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker spawnBoundaryTracker,
            IPauseSystem pauseSystem)
        {
            CollisionService = collisionService;
            MovementConfigurator = movementConfigurator;
            SpawnBoundaryTracker = spawnBoundaryTracker;
            PauseSystem = pauseSystem;
        }

        public virtual void Initialize(TEnemy enemy, TConfig config)
        {
            enemy.SetType(config.Type);
            enemy.CollisionHandler.Initialize(CollisionService);
            SpawnBoundaryTracker.RegisterObject(enemy.Transform);
            MovementConfigurator.Configure(enemy, enemy.Transform.position, config);
            PauseSystem.Register(enemy);
        }
    }
}