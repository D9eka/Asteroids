using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public interface IProjectileFactory
    {
        public void Initialize(NetworkObject projectilePrefab);
        
        public void Create(Vector2 position, Quaternion rotation, 
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService);
    }
}
