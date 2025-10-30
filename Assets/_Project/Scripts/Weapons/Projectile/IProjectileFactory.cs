using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using UnityEngine;

namespace _Project.Scripts.Weapons.Projectile
{
    public interface IProjectileFactory
    {
        public IProjectile Create(Vector2 position, Quaternion rotation, 
            ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService);
    }
}