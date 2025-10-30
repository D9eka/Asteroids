using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Movement;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class DefaultEnemyInitializer : EnemyInitializer<IEnemy, EnemyTypeConfig>
    {
        public DefaultEnemyInitializer(ICollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator, SpawnBoundaryTracker spawnBoundaryTracker) : base(
            collisionService, movementConfigurator, spawnBoundaryTracker)
        {
        }
    }
}