using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public class UfoInitializer : EnemyInitializer<Ufo, UfoTypeConfig>
    {
        private readonly IProjectileFactory _projectileFactory;
        private readonly IWeaponUpdater _weaponUpdater;

        [Inject]
        public UfoInitializer(ICollisionService collisionService, IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker spawnBoundaryTracker, IPauseSystem pauseSystem, IProjectileFactory projectileFactory, 
            IWeaponUpdater weaponUpdater)
            : base(collisionService, movementConfigurator, spawnBoundaryTracker, pauseSystem)
        {
            _projectileFactory = projectileFactory;
            _weaponUpdater = weaponUpdater;
        }

        public override void Initialize(Ufo ufo, UfoTypeConfig config)
        {
            base.Initialize(ufo, config);
            if (ufo.Initialized) return;
            ufo.BulletGun.Initialize(ufo.gameObject, CollisionService, config.BulletGunConfig, _projectileFactory);
            _weaponUpdater.AddWeapon(ufo.BulletGun);
            ufo.Initialized = true;
        }
    }
}