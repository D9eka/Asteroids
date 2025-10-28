using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Spawning.Pooling;
using UnityEngine;

namespace _Project.Scripts.Weapons.Projectile
{
    public interface IProjectile : IDamageSource, IPoolable
    {
        public void Initialize(ProjectileData data, DamageType damageType, ICollisionService collisionService);
    }
}