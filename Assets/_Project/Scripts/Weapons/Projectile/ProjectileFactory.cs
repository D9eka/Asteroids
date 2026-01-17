using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<IPoolable> _lifecycleManager;
        
        private ProjectilePool _pool;

        [Inject]
        public ProjectileFactory(IPauseSystem pauseSystem, IPoolableLifecycleManager<IPoolable> lifecycleManager)
        {
            _pauseSystem = pauseSystem;
            _lifecycleManager = lifecycleManager;
        }

        public void Initialize(ProjectilePool pool)
        {
            _pool = pool;
        }

        public void Create(Vector2 position, Quaternion rotation, 
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService)
        {
            if (_pool == null) return;
            
            Projectile projectile = _pool.Spawn(position, rotation, config, damageInfo, collisionService);
            _pauseSystem.Register(projectile);
            _lifecycleManager.Register(projectile, _pool);
        }
    }
}