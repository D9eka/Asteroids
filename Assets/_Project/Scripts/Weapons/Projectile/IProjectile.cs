using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public interface IProjectile : IPoolable, IPausable, IEcsEntity
    {
        public void Initialize(ProjectilePool pool, int id,
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService);
    }
}