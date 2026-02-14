using Asteroids.Scripts.Effects;
using Asteroids.Scripts.Audio.Sounds.Weapon;
using Asteroids.Scripts.Audio;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Leopotam.EcsLite;
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
        public UfoInitializer(EcsWorld ecsWorld, EnemyCollisionService collisionService, IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker spawnBoundaryTracker, IPauseSystem pauseSystem, IProjectileFactory projectileFactory, 
            IWeaponUpdater weaponUpdater, WeaponShotAudioSpawner weaponShotAudioSpawner,
            BulletGunEffectSpawner bulletGunEffectSpawner, EntityViewRegistry entityViewRegistry)
            : base(ecsWorld, collisionService, movementConfigurator, spawnBoundaryTracker, pauseSystem, entityViewRegistry)
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
            ufo.BulletGun.Initialize(ufo, CollisionService, config.BulletGunConfig, _projectileFactory);
            _weaponUpdater.AddWeapon(ufo.BulletGun);
            _weaponShotAudioSpawner.AddWeapon(ufo.BulletGun);
            _bulletGunEffectSpawner.AddWeapon(ufo.BulletGun);
            ufo.Initialized = true;
        }
    }
}