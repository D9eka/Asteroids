using _Project.Scripts.Collision;
using _Project.Scripts.Spawning.Pooling;
using UnityEngine;

namespace _Project.Scripts.Weapons.Projectile
{
    public interface IProjectile : IPoolable
    {
        public void Initialize(ProjectileData data, ICollisionService collisionService);
    }
}