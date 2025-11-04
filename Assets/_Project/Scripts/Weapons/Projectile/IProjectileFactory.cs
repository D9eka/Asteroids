using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public interface IProjectileFactory
    {
        public void Create(Vector2 position, Quaternion rotation, 
            ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService);
    }
}