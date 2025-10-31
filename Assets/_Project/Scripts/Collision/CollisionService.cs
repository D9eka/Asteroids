using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Collision
{
    public abstract class CollisionService : ICollisionService
    {
        public virtual void OnHit(GameObject origin, GameObject target)
        {
            var originDamageSource = origin.GetComponent<IDamageSource>();
            var targetDestroyable = target.GetComponent<IDamageable>();

            if (originDamageSource == null || targetDestroyable == null) return;
            if (!CanDestroy(targetDestroyable)) return;
            
            DamageInfo damageInfo = originDamageSource.GetDamageInfo();
            targetDestroyable.TakeDamage(damageInfo);
            
            if (origin.TryGetComponent(out IDamageable originDamageable) && ShouldTakeDamageOnHit(originDamageable))
            {
                originDamageable.TakeDamage(new DamageInfo(DamageType.Collide, target));
            }
        }

        public abstract bool CanDestroy(IDamageable target);
        public abstract bool ShouldTakeDamageOnHit(IDamageable self);
    }
}