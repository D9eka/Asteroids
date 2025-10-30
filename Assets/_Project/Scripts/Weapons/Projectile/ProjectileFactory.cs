using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Pause;
using _Project.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = _Project.Scripts.Spawning.Common.Pooling.IPoolable;

namespace _Project.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ProjectilePool<Projectile> _pool;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<IPoolable> _lifecycleManager;

        [Inject]
        public ProjectileFactory(ProjectilePool<Projectile> pool, IPauseSystem pauseSystem,
            IPoolableLifecycleManager<IPoolable> lifecycleManager)
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