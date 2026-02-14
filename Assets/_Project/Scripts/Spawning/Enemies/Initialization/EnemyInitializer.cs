using Asteroids.Scripts.Ecs.Views;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Leopotam.EcsLite;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public abstract class EnemyInitializer<TEnemy, TConfig> : IEnemyInitializer<TEnemy, TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        protected readonly EcsWorld EcsWorld;
        protected readonly ICollisionService CollisionService;
        protected readonly IEnemyMovementConfigurator MovementConfigurator;
        protected readonly ISpawnBoundaryTracker SpawnBoundaryTracker;
        protected readonly IPauseSystem PauseSystem;

        [Inject]
        public EnemyInitializer(EcsWorld ecsWorld,
            ICollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker spawnBoundaryTracker,
            IPauseSystem pauseSystem)
        {
            EcsWorld = ecsWorld;
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
            if (enemy is IPausable pausable)
            {
                PauseSystem.Register(pausable);
            }
            
            int enemyEntity = EcsWorld.NewEntity();
            enemy.SetId(enemyEntity);
            MovementConfigurator.Configure(enemyEntity, enemy, enemy.Transform.position, config);
            EnemyMovementView enemyMovementView = enemy.Transform.gameObject.GetComponent<EnemyMovementView>();
            enemyMovementView.Initialize(EcsWorld, enemyEntity);
            PauseSystem.Register(enemyMovementView);
        }
    }
}