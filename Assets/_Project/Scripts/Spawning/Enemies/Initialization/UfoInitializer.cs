using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Movement;
using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Projectile;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class UfoInitializer : EnemyInitializer<Ufo, UfoTypeConfig>
    {
        private readonly IProjectileFactory _projectileFactory;
        private readonly ICollisionService _collisionService;
        private readonly IWeaponUpdater _weaponUpdater;

        [Inject]
        public UfoInitializer(ICollisionService collisionService, IEnemyMovementConfigurator movementConfigurator,
            SpawnBoundaryTracker spawnBoundaryTracker, IProjectileFactory projectileFactory, 
            IWeaponUpdater weaponUpdater)
            : base(collisionService, movementConfigurator, spawnBoundaryTracker)
        {
            _projectileFactory = projectileFactory;
            _weaponUpdater = weaponUpdater;
        }

        public override void Initialize(Ufo ufo, UfoTypeConfig config)
        {
            base.Initialize(ufo, config);
            ufo.BulletGun.Initialize(ufo.gameObject, _collisionService, config.BulletGunConfig, _projectileFactory);
            _weaponUpdater.AddWeapon(ufo.BulletGun);
        }
    }
}