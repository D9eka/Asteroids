using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Movement;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public abstract class EnemyInitializer<TEnemy, TConfig> : IEnemyInitializer<TEnemy, TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        private readonly ICollisionService _collisionService;
        private readonly IEnemyMovementConfigurator _movementConfigurator;
        private readonly SpawnBoundaryTracker _spawnBoundaryTracker;

        [Inject]
        public EnemyInitializer(ICollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker spawnBoundaryTracker)
        {
            _collisionService = collisionService;
            _movementConfigurator = movementConfigurator;
            _spawnBoundaryTracker = spawnBoundaryTracker;
        }

        public virtual void Initialize(TEnemy enemy, TConfig config)
        {
            enemy.SetType(config.Type);
            enemy.CollisionHandler.Initialize(_collisionService);
            _spawnBoundaryTracker.RegisterObject(enemy.Transform);
            _movementConfigurator.Configure(enemy, enemy.Transform.position, config);
        }
    }
}