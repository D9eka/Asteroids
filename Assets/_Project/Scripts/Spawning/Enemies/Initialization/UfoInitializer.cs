using Asteroids.Scripts.Effects;
using Asteroids.Scripts.Audio.Sounds.Weapon;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Ecs.Warp.Components;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Ecs.Weapons.Services;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Leopotam.EcsLite;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public class UfoInitializer : EnemyInitializer<Ufo, UfoTypeConfig>
    {
        private readonly IProjectileFactory _projectileFactory;
        private readonly WeaponShotAudioSpawner _weaponShotAudioSpawner;
        private readonly BulletGunEffectSpawner _bulletGunEffectSpawner;
        private readonly WeaponViewRegistry _weaponViewRegistry;

        [Inject]
        public UfoInitializer(EcsWorld ecsWorld, EnemyCollisionService collisionService, 
            IEnemyMovementConfigurator movementConfigurator, IPauseSystem pauseSystem, 
            EntityViewRegistry entityViewRegistry, IProjectileFactory projectileFactory, 
            WeaponShotAudioSpawner weaponShotAudioSpawner, BulletGunEffectSpawner bulletGunEffectSpawner, 
            WeaponViewRegistry weaponViewRegistry)
            : base(ecsWorld, collisionService, movementConfigurator, pauseSystem, entityViewRegistry)
        {
            _projectileFactory = projectileFactory;
            _weaponShotAudioSpawner = weaponShotAudioSpawner;
            _bulletGunEffectSpawner = bulletGunEffectSpawner;
            _weaponViewRegistry = weaponViewRegistry;
        }

        public override void Initialize(Ufo ufo, UfoTypeConfig config)
        {
            base.Initialize(ufo, config);
            EcsWorld.GetPool<DestroyOnOutOfBoundsTag>().Add(ufo.Id);
            if (ufo.Initialized) return;
            BulletGun bulletGun = ufo.BulletGun;
            BulletGunConfig bulletGunConfig = config.BulletGunConfig;
            
            int weaponEntity = EcsWorld.NewEntity();
            ref WeaponOwnerComponent weaponOwnerComponent = ref EcsWorld.GetPool<WeaponOwnerComponent>().Add(weaponEntity);
            weaponOwnerComponent.OwnerEntity = ufo.Id;
            ref BulletGunComponent bulletGunComponent = ref EcsWorld.GetPool<BulletGunComponent>().Add(weaponEntity);
            bulletGunComponent.FireRate = bulletGunConfig.FireRate;
            EcsWorld.GetPool<ActiveWeaponTag>().Add(weaponEntity);
            _weaponViewRegistry.Register(weaponEntity, bulletGun);
            
            bulletGun.Initialize(ufo, CollisionService, config.BulletGunConfig, _projectileFactory);
            _weaponShotAudioSpawner.AddWeapon(bulletGun);
            _bulletGunEffectSpawner.AddWeapon(bulletGun);
            ufo.Initialize(EcsWorld, weaponEntity);
            ufo.Initialized = true;
        }
    }
}