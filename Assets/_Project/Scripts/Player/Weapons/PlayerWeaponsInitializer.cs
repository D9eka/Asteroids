using Asteroids.Scripts.Analytics;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player.Weapons
{
    public class PlayerWeaponsInitializer
    {
        private readonly IWeaponUpdater _weaponUpdater;
        private readonly IAnalyticsCollector _analyticsCollector;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IRaycastService _raycastService;
        private readonly BulletGunConfig _bulletGunConfig;
        private readonly LaserGunConfig _laserGunConfig;

        public PlayerWeaponsInitializer(IWeaponUpdater weaponUpdater, IAnalyticsCollector analyticsCollector,
            IProjectileFactory projectileFactory, IRaycastService raycastService, BulletGunConfig bulletGunConfig,
            LaserGunConfig laserGunConfig)
        {
            _weaponUpdater = weaponUpdater;
            _analyticsCollector = analyticsCollector;
            _projectileFactory = projectileFactory;
            _raycastService = raycastService;
            _bulletGunConfig = bulletGunConfig;
            _laserGunConfig = laserGunConfig;
        }

        public void Initialize(GameObject damageInstigator, ICollisionService playerCollisionService, IWeapon[] weapons, 
            ILineRenderer laserGunLineRenderer)
        {
            foreach (IWeapon weapon in weapons)
            {
                _weaponUpdater.AddWeapon(weapon);
                if (weapon is BulletGun bulletGun)
                {
                    bulletGun.Initialize(damageInstigator, playerCollisionService, _bulletGunConfig, _projectileFactory);
                    _analyticsCollector.Initialize(bulletGun);
                }
                if (weapon is LaserGun laserGun)
                {
                    _raycastService.Initialize(laserGun.gameObject);
                    laserGun.Initialize(damageInstigator, _laserGunConfig, laserGunLineRenderer, _raycastService,
                        playerCollisionService);
                    _analyticsCollector.Initialize(laserGun);
                }
            }
        }
    }
}