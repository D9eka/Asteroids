using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using _Project.Scripts.Multiplayer;
using Fusion;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly IPauseSystem _pauseSystem;
        private readonly NetworkEventsRouter _networkEventsRouter;
        
        private NetworkObject _projectilePrefab;

        [Inject]
        public ProjectileFactory(IPauseSystem pauseSystem, NetworkEventsRouter networkEventsRouter)
        {
            _pauseSystem = pauseSystem;
            _networkEventsRouter = networkEventsRouter;
        }

        public void Initialize(NetworkObject projectilePrefab)
        {
            _projectilePrefab = projectilePrefab;
        }

        public void Create(Vector2 position, Quaternion rotation, 
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService)
        {
            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null || !runner.IsServer || _projectilePrefab == null)
                return;

            NetworkObject obj = runner.Spawn(
                _projectilePrefab,
                position,
                rotation,
                null,
                (r, spawned) =>
                {
                    Projectile projectile = spawned.GetComponent<Projectile>();
                    projectile.SetPauseSystem(_pauseSystem);
                    projectile.Initialize(config, damageInfo, collisionService);
                    _pauseSystem.Register(projectile);
                });
        }
    }
}
