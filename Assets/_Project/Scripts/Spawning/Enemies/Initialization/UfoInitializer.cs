using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Projectile;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class UfoInitializer : IEnemyInitializer<Ufo, UfoTypeConfig>
    {
        private readonly IProjectileFactory _projectileFactory;
        private readonly ICollisionService _collisionService;
        private readonly IWeaponUpdater _weaponUpdater;

        [Inject]
        public UfoInitializer(IProjectileFactory projectileFactory, ICollisionService collisionService, IWeaponUpdater weaponUpdater)
        {
            _projectileFactory = projectileFactory;
            _collisionService = collisionService;
            _weaponUpdater = weaponUpdater;
        }

        public void Initialize(Ufo ufo, UfoTypeConfig config)
        {
            ufo.CollisionHandler.Initialize(_collisionService);
            ufo.BulletGun.Initialize(_collisionService, config.BulletGunConfig, _projectileFactory);
            _weaponUpdater.AddWeapon(ufo.BulletGun);
        }
    }
}