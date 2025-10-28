using _Project.Scripts.Collision;
using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Projectile;
using _Project.Scripts.Weapons.Services.Raycast;
using _Project.Scripts.Weapons.Types.BulletGun;
using _Project.Scripts.Weapons.Types.Laser;
using _Project.Scripts.Weapons.Types.Laser.LineRenderer;
using Zenject;

namespace _Project.Scripts.Player.Weapons
{
    public class PlayerWeaponsInitializer : IInitializable
    {
        private readonly IWeapon[] _weapons;
        private readonly IProjectileFactory _projectileFactory;
        private readonly ICollisionService _collisionService;
        private readonly BulletGunConfig _bulletGunConfig;
        private readonly LaserGunConfig _laserGunConfig;
        private readonly ILineRenderer _laserGunLineRenderer;
        private readonly IRaycastService _raycastService;
        
        [Inject]
        public PlayerWeaponsInitializer(IProjectileFactory projectileFactory, 
            [Inject(Id = "PlayerWeapons")] IWeapon[] weapons,
            [Inject(Id = "PlayerCollisionService")] ICollisionService collisionService,
            BulletGunConfig bulletGunConfig,
            LaserGunConfig laserGunConfig,
            ILineRenderer laserGunLineRenderer,
            IRaycastService raycastService)
        {
            _weapons = weapons;
            _projectileFactory = projectileFactory;
            _collisionService = collisionService;
            _bulletGunConfig = bulletGunConfig;
            _laserGunConfig = laserGunConfig;
            _laserGunLineRenderer = laserGunLineRenderer;
            _raycastService = raycastService;
        }
        
        public void Initialize()
        {
            foreach (IWeapon weapon in _weapons)
            {
                if (weapon is BulletGun bulletGun)
                {
                    bulletGun.Initialize(_collisionService, _bulletGunConfig, _projectileFactory);
                }
                if (weapon is LaserGun laserGun)
                {
                    _raycastService.Initialize(_collisionService, laserGun.gameObject);
                    laserGun.Initialize(_laserGunConfig, _laserGunLineRenderer, _raycastService);
                }
            }
        }
    }
}