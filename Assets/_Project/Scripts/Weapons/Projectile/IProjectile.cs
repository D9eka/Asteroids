using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public interface IProjectile : IDamageSource, IPoolable, IPausable
    {
        public void Initialize(ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService);
    }
}