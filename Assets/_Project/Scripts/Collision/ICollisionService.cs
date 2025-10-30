using _Project.Scripts.Damage;
using UnityEngine;

namespace _Project.Scripts.Collision
{
    public interface ICollisionService
    {
        public void OnHit(GameObject origin, GameObject target);
        public bool CanDestroy(IDamageable target);
        public bool ShouldTakeDamageOnHit(IDamageable self);
    }
}