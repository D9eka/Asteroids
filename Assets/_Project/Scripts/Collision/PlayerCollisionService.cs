using Asteroids.Scripts.Ecs.Colliders.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Collision
{
    public class PlayerCollisionService : CollisionService
    {
        private EcsPool<EnemyTag> _enemyTagPool;
        private EcsPool<ProjectileTag> _projectileTagPool;
        private EcsPool<OwnerComponent> _ownerComponentsPool;
        
        public PlayerCollisionService(EcsWorld ecsWorld)
            : base(ecsWorld)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _enemyTagPool = EcsWorld.GetPool<EnemyTag>();
            _projectileTagPool = EcsWorld.GetPool<ProjectileTag>();
            _ownerComponentsPool = EcsWorld.GetPool<OwnerComponent>();
        }
        
        public override bool CanDestroy(int targetEntityId)
        {
            return _enemyTagPool.Has(targetEntityId) || 
                _projectileTagPool.Has(targetEntityId) && _ownerComponentsPool.Has(targetEntityId) 
                && _enemyTagPool.Has(_ownerComponentsPool.Get(targetEntityId).OwnerEntity);
        }

        public override bool ShouldTakeDamageOnHit(int sourceEntityId)
        {
            return _projectileTagPool.Has(sourceEntityId);
        }
    }
}