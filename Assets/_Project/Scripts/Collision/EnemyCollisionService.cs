using Asteroids.Scripts.Ecs;
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
        
        public override bool CanDestroy(IEcsEntity targetEntity)
        {
            if (_playerTagPool.Has(targetEntity.Id)) return true;
            if (!_projectileTagPool.Has(targetEntity.Id) || !_ownerComponentsPool.Has(targetEntity.Id)) return false;
            int ownerEntity = _ownerComponentsPool.Get(targetEntity.Id).OwnerEntity;
            return IsEntityAlive(ownerEntity) && _playerTagPool.Has(ownerEntity);
        }

        public override bool ShouldTakeDamageOnHit(int sourceEntityId)
        {
            return _projectileTagPool.Has(sourceEntityId);
        }
    }
}