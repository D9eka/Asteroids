using _Project.Scripts.Effects;
using Asteroids.Scripts.Audio.Sounds.Weapon;
using Asteroids.Scripts.Audio;
using Asteroids.Scripts.Analytics;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;

namespace Asteroids.Scripts.Player.Weapons
{
    public class PlayerWeaponsInitializer
    {
        private readonly IWeaponUpdater _weaponUpdater;
        private readonly IAnalyticsCollector _analyticsCollector;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IRaycastService _raycastService;
        private readonly IPlayerConfigProvider _playerConfigProvider;
        private readonly PlayerWeaponsConfigRuntime _playerWeaponsConfigRuntime;
        private readonly WeaponShotAudioSpawner _weaponShotAudioSpawner;
        private readonly BulletGunEffectSpawner _bulletGunEffectSpawner;

        public PlayerWeaponsInitializer(IWeaponUpdater weaponUpdater, IAnalyticsCollector analyticsCollector,
            IProjectileFactory projectileFactory, IRaycastService raycastService, IPlayerConfigProvider playerConfigProvider,
            PlayerWeaponsConfigRuntime playerWeaponsConfigRuntime, WeaponShotAudioSpawner weaponShotAudioSpawner,
            BulletGunEffectSpawner bulletGunEffectSpawner)
        {
            _weaponUpdater = weaponUpdater;
            _analyticsCollector = analyticsCollector;
            _projectileFactory = projectileFactory;
            _raycastService = raycastService;
            _playerConfigProvider = playerConfigProvider;
            _playerWeaponsConfigRuntime = playerWeaponsConfigRuntime;
            _weaponShotAudioSpawner = weaponShotAudioSpawner;
            _bulletGunEffectSpawner = bulletGunEffectSpawner;
        }

        public void Initialize(GameObject damageInstigator, ICollisionService playerCollisionService, IWeapon[] weapons, 
            ILineRenderer laserGunLineRenderer)
        {
            foreach (IWeapon weapon in weapons)
            {
                _weaponUpdater.AddWeapon(weapon);
                _weaponShotAudioSpawner.AddWeapon(weapon);
                if (weapon is BulletGun bulletGun)
                {
                    _bulletGunEffectSpawner.AddWeapon(bulletGun);
                    BulletGunConfig bulletGunConfig =
                        _playerConfigProvider.PlayerConfig.BulletGunConfig;
                    bulletGun.Initialize(damageInstigator, playerCollisionService, bulletGunConfig, _projectileFactory);
                    _analyticsCollector.Initialize(bulletGun);
                }
                if (weapon is LaserGun laserGun)
                {
                    LaserGunConfig laserGunConfig = _playerConfigProvider.PlayerConfig.LaserGunConfig;
                    _raycastService.Initialize(laserGun.gameObject);
                    laserGun.Initialize(damageInstigator, laserGunConfig, laserGunLineRenderer, _raycastService,
                        playerCollisionService);
                    _analyticsCollector.Initialize(laserGun);
                }
            }
            _playerWeaponsConfigRuntime.Initialize(weapons);
        }
    }
}