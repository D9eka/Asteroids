using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Spawning.Common.Pooling;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public interface IProjectile : IPoolable, IEcsEntity
    {
        public void Initialize(ProjectilePool pool, int id, ICollisionService collisionService);
    }
}