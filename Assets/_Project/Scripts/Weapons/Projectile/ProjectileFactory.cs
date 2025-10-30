using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ProjectilePool<Projectile> _pool;

        [Inject]
        public ProjectileFactory(ProjectilePool<Projectile> pool)
        {
            _pool = pool;
        }

        public IProjectile Create(Vector2 position, Quaternion rotation, 
            ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService)
        {
            var projectile = _pool.Spawn(position, rotation, data, damageInfo, collisionService);
            return projectile;
        }
    }
}