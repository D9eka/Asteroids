using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Pause;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Movement;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public abstract class EnemyInitializer<TEnemy, TConfig> : IEnemyInitializer<TEnemy, TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        protected readonly ICollisionService CollisionService;
        protected readonly IEnemyMovementConfigurator MovementConfigurator;
        protected readonly SpawnBoundaryTracker SpawnBoundaryTracker;
        protected readonly IPauseSystem PauseSystem;

        [Inject]
        public EnemyInitializer(ICollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker spawnBoundaryTracker,
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