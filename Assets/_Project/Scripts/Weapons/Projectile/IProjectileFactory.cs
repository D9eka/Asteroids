using _Project.Scripts.Collision;
using UnityEngine;

namespace _Project.Scripts.Weapons.Projectile
{
    public interface IProjectileFactory
    {
        public IProjectile Create(Vector2 position, Quaternion rotation, ProjectileData data, ICollisionService collisionService);
    }
}