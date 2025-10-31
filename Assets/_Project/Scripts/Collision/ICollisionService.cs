using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Collision
{
    public interface ICollisionService
    {
        public void OnHit(GameObject origin, GameObject target);
        public bool CanDestroy(IDamageable target);
        public bool ShouldTakeDamageOnHit(IDamageable self);
    }
}