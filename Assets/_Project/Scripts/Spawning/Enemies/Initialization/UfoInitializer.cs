using Asteroids.Scripts.Effects;
using Asteroids.Scripts.Audio.Sounds.Weapon;
using Asteroids.Scripts.Audio;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Enemies;
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
        private readonly WeaponShotAudioSpawner _weaponShotAudioSpawner;
        private readonly BulletGunEffectSpawner _bulletGunEffectSpawner;

        [Inject]
        public UfoInitializer(ICollisionService collisionService, IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker spawnBoundaryTracker, IPauseSystem pauseSystem, IProjectileFactory projectileFactory, 
            IWeaponUpdater weaponUpdater, WeaponShotAudioSpawner weaponShotAudioSpawner,
            BulletGunEffectSpawner bulletGunEffectSpawner)
            : base(collisionService, movementConfigurator, spawnBoundaryTracker, pauseSystem)
        {
            _projectileFactory = projectileFactory;
            _weaponUpdater = weaponUpdater;
            _weaponShotAudioSpawner = weaponShotAudioSpawner;
            _bulletGunEffectSpawner = bulletGunEffectSpawner;
        }

        public override void Initialize(Ufo ufo, UfoTypeConfig config)
        {
            base.Initialize(ufo, config);
            if (ufo.Initialized) return;
            ufo.BulletGun.Initialize(ufo.gameObject, CollisionService, config.BulletGunConfig, _projectileFactory);
            _weaponUpdater.AddWeapon(ufo.BulletGun);
            _weaponShotAudioSpawner.AddWeapon(ufo.BulletGun);
            _bulletGunEffectSpawner.AddWeapon(ufo.BulletGun);
            ufo.Initialized = true;
        }
    }
}