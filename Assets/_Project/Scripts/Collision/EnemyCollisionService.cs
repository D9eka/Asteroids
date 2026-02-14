using Asteroids.Scripts.Ecs.Colliders.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Collision
{
    public class EnemyCollisionService : CollisionService
    {
        private EcsPool<PlayerTag> _playerTagPool;
        private EcsPool<ProjectileTag> _projectileTagPool;
        private EcsPool<OwnerComponent> _ownerComponentsPool;
        
        public EnemyCollisionService(EcsWorld ecsWorld)
            : base(ecsWorld)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _playerTagPool = EcsWorld.GetPool<PlayerTag>();
            _projectileTagPool = EcsWorld.GetPool<ProjectileTag>();
            _ownerComponentsPool = EcsWorld.GetPool<OwnerComponent>();
        }
        
        public override bool CanDestroy(int targetEntityId)
        {
            return _playerTagPool.Has(targetEntityId) || 
                _projectileTagPool.Has(targetEntityId) && _ownerComponentsPool.Has(targetEntityId) 
                && _playerTagPool.Has(_ownerComponentsPool.Get(targetEntityId).OwnerEntity);
        }

        public override bool ShouldTakeDamageOnHit(int sourceEntityId)
        {
            return _projectileTagPool.Has(sourceEntityId);
        }
    }
}