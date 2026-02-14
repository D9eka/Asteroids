using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Collision
{
    public abstract class CollisionService : ICollisionService, IInitializable
    {
        protected readonly EcsWorld EcsWorld;
        
        private EcsPool<DamageSourceComponent> _damageSourcePool;
        private EcsPool<DamageEventComponent> _damageEventPool;
        
        protected CollisionService(EcsWorld ecsWorld)
        {
            EcsWorld = ecsWorld;
        }
        
        public virtual void Initialize()
        {
            _damageSourcePool = EcsWorld.GetPool<DamageSourceComponent>();
            _damageEventPool = EcsWorld.GetPool<DamageEventComponent>();
        }

        public virtual void OnHit(GameObject origin, GameObject target)
        {
            var originDamageSource = origin.GetComponent<IEcsEntity>();
            var targetDestroyable = target.GetComponent<IEcsEntity>();

            if (originDamageSource == null || targetDestroyable == null) return;
            if (originDamageSource.Id < 0 || targetDestroyable.Id < 0) return;
            if (!CanDestroy(targetDestroyable.Id)) return;

            DamageType damageType = _damageSourcePool.Get(originDamageSource.Id).Type;
            AddDamageEvent(targetDestroyable.Id, originDamageSource.Id, damageType);
            if (ShouldTakeDamageOnHit(originDamageSource.Id))
            {
                DamageType targetDamageSourceType = _damageSourcePool.Get(targetDestroyable.Id).Type;
                AddDamageEvent(originDamageSource.Id, targetDestroyable.Id, targetDamageSourceType);
            }
        }

        public abstract bool CanDestroy(int targetEntityId);
        public abstract bool ShouldTakeDamageOnHit(int sourceEntityId);

        private void AddDamageEvent(int targetId, int originId, DamageType damageType)
        {
            if (!_damageEventPool.Has(targetId))
            {
                ref DamageEventComponent damageEvent = ref _damageEventPool.Add(targetId);
                damageEvent.SourceEntity = originId;
                damageEvent.DamageType = damageType;
            }
        }
    }
}