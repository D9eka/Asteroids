using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ProjectilePool<Projectile> _pool;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _lifecycleManager;

        [Inject]
        public ProjectileFactory(ProjectilePool<Projectile> pool, IPauseSystem pauseSystem,
            IPoolableLifecycleManager<Pooling_IPoolable> lifecycleManager)
        {
            _pool = pool;
            _pauseSystem = pauseSystem;
            _lifecycleManager = lifecycleManager;
        }

        public IProjectile Create(Vector2 position, Quaternion rotation, 
            ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService)
        {
            Projectile projectile = _pool.Spawn(position, rotation, data, damageInfo, collisionService);
            _pauseSystem.Register(projectile);
            _lifecycleManager.Register(projectile, _pool);
            return projectile;
        }
    }
}