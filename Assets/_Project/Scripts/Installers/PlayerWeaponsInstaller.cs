using _Project.Scripts.Player.Weapons;
using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Projectile;
using _Project.Scripts.Weapons.Services.Raycast;
using _Project.Scripts.Weapons.Types.BulletGun;
using _Project.Scripts.Weapons.Types.Laser;
using _Project.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class PlayerWeaponsInstaller : MonoInstaller
    {
        [SerializeField] private BulletGun _bulletGun;
        [SerializeField] private BulletGunConfig _bulletGunConfig;
        [SerializeField] private Projectile _playerProjectilePrefab;
        [Space]
        [SerializeField] private LaserGun _laserGun;
        [SerializeField] private LaserGunConfig _laserGunConfig;
        [SerializeField] private SpriteLineRenderer _lineRenderer;

        public override void InstallBindings()
        {
            BindProjectiles();
            BindBulletGun();
            BindLaserGun();
            
            Container.Bind<IWeapon[]>().WithId("PlayerWeapons")
                .FromInstance(new IWeapon[]{ _bulletGun, _laserGun })
                .AsCached();
            
            Container.BindInterfacesAndSelfTo<PlayerWeaponsHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WeaponUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerWeaponsInitializer>().AsSingle().NonLazy();
        }

        private void BindProjectiles()
        {
            Container.BindMemoryPool<Projectile, ProjectilePool<Projectile>>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_playerProjectilePrefab)
                .UnderTransformGroup("Projectiles");
            
            Container.Bind<IProjectileFactory>()
                .To<ProjectileFactory>()
                .AsSingle();
        }

        private void BindBulletGun()
        {
            Container.Bind<BulletGunConfig>()
                .FromInstance(_bulletGunConfig)
                .AsCached()
                .WhenInjectedInto<PlayerWeaponsInitializer>();

            Container.Bind<IWeapon>()
                .WithId("PlayerBulletGun")
                .To<BulletGun>()
                .FromInstance(_bulletGun)
                .AsCached();
        }

        private void BindLaserGun()
        {
            Container.Bind<LaserGunConfig>()
                .FromInstance(_laserGunConfig)
                .AsCached()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<ILineRenderer>()
                .FromInstance(_lineRenderer)
                .AsCached()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<IRaycastService>()
                .To<RaycastService>()
                .AsSingle()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<IWeapon>()
                .WithId("PlayerLaserGun")
                .To<LaserGun>()
                .FromInstance(_laserGun)
                .AsCached();
            Container.Bind<ILaserGun>()
                .WithId("PlayerLaserGun")
                .To<LaserGun>()
                .FromInstance(_laserGun)
                .AsCached();
        }
    }
}