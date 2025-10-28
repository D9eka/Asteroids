using _Project.Scripts.Collision;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ProjectTilePool<Projectile> _pool;

        [Inject]
        public ProjectileFactory(ProjectTilePool<Projectile> pool)
        {
            _pool = pool;
        }

        public IProjectile Create(Vector2 position, Quaternion rotation, ProjectileData data, ICollisionService collisionService)
        {
            var projectile = _pool.Spawn(position, rotation, data, collisionService);
            return projectile;
        }
    }
}