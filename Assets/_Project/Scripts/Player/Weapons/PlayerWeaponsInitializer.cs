using Asteroids.Scripts.Effects;
using Asteroids.Scripts.Audio.Sounds.Weapon;
using Asteroids.Scripts.Analytics;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Ecs.Weapons.Services;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Player.Weapons
{
    public class PlayerWeaponsInitializer
    {
        private readonly IAnalyticsCollector _analyticsCollector;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IRaycastService _raycastService;
        private readonly IPlayerConfigProvider _playerConfigProvider;
        private readonly WeaponShotAudioSpawner _weaponShotAudioSpawner;
        private readonly BulletGunEffectSpawner _bulletGunEffectSpawner;
        private readonly EcsWorld _ecsWorld;
        private readonly WeaponViewRegistry _weaponViewRegistry;

        public PlayerWeaponsInitializer(IAnalyticsCollector analyticsCollector,
            IProjectileFactory projectileFactory, IRaycastService raycastService, IPlayerConfigProvider playerConfigProvider,
            WeaponShotAudioSpawner weaponShotAudioSpawner, BulletGunEffectSpawner bulletGunEffectSpawner,
            EcsWorld ecsWorld, WeaponViewRegistry weaponViewRegistry)
        {
            _analyticsCollector = analyticsCollector;
            _projectileFactory = projectileFactory;
            _raycastService = raycastService;
            _playerConfigProvider = playerConfigProvider;
            _weaponShotAudioSpawner = weaponShotAudioSpawner;
            _bulletGunEffectSpawner = bulletGunEffectSpawner;
            _ecsWorld = ecsWorld;
            _weaponViewRegistry = weaponViewRegistry;
        }

        public void Initialize(IEcsEntity instigatorEntity, ICollisionService playerCollisionService, IWeapon[] weapons, 
            ILineRenderer laserGunLineRenderer)
        {
            foreach (IWeapon weapon in weapons)
            {
                _weaponShotAudioSpawner.AddWeapon(weapon);
                
                int weaponEntity = _ecsWorld.NewEntity();
                ref WeaponOwnerComponent weaponOwnerComponent = ref _ecsWorld.GetPool<WeaponOwnerComponent>().Add(weaponEntity);
                weaponOwnerComponent.OwnerEntity = instigatorEntity.Id;
                _weaponViewRegistry.Register(weaponEntity, weapon);
                
                if (weapon is BulletGun bulletGun)
                {
                    InitializeBulletGun(weaponEntity, bulletGun, instigatorEntity, playerCollisionService);
                }
                if (weapon is LaserGun laserGun)
                {
                    InitializeLaserGun(weaponEntity, laserGun, instigatorEntity, playerCollisionService, laserGunLineRenderer);
                }
            }
        }

        private void InitializeBulletGun(int entity, BulletGun bulletGun, IEcsEntity instigatorEntity, ICollisionService playerCollisionService)
        {
            _bulletGunEffectSpawner.AddWeapon(bulletGun);
            BulletGunConfig bulletGunConfig = _playerConfigProvider.PlayerConfig.BulletGunConfig;
            ref BulletGunComponent bulletGunComponent = ref _ecsWorld.GetPool<BulletGunComponent>().Add(entity);
            bulletGunComponent.FireRate = bulletGunConfig.FireRate;
            _ecsWorld.GetPool<ActiveWeaponTag>().Add(entity);
            
            bulletGun.Initialize(instigatorEntity, playerCollisionService, bulletGunConfig, _projectileFactory);
            bulletGun.ApplyConfig(_playerConfigProvider.PlayerConfig.BulletGunConfig);
            _analyticsCollector.Initialize(bulletGun);
        }

        private void InitializeLaserGun(int entity, LaserGun laserGun, IEcsEntity instigatorEntity, ICollisionService playerCollisionService,
            ILineRenderer laserGunLineRenderer)
        {
            LaserGunConfig laserGunConfig = _playerConfigProvider.PlayerConfig.LaserGunConfig;
            ref LaserGunComponent laserGunComponent = ref _ecsWorld.GetPool<LaserGunComponent>().Add(entity);
            laserGunComponent.CurrentCharges = laserGunConfig.MaxCharges;
            laserGunComponent.MaxCharges = laserGunConfig.MaxCharges;
            laserGunComponent.FireRate = laserGunConfig.FireRate;
            laserGunComponent.RechargeRate = laserGunConfig.RechargeRate;
            laserGunComponent.Duration = laserGunConfig.LaserDuration;
            
            _raycastService.Initialize(laserGun.gameObject);
            laserGun.Initialize(entity, _ecsWorld, instigatorEntity, laserGunConfig, 
                laserGunLineRenderer, _raycastService, playerCollisionService);
            laserGun.ApplyConfig(_playerConfigProvider.PlayerConfig.LaserGunConfig);
            _analyticsCollector.Initialize(laserGun);
        }
    }
}