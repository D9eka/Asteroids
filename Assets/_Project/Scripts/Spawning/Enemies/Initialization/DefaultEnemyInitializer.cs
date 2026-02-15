using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public class DefaultEnemyInitializer : EnemyInitializer<IEnemy, EnemyTypeConfig>
    {
        public DefaultEnemyInitializer(EcsWorld ecsWorld, EnemyCollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator, IPauseSystem pauseSystem, EntityViewRegistry entityViewRegistry) 
            : base(ecsWorld, collisionService, movementConfigurator, pauseSystem, entityViewRegistry)
        {
        }
    }
}