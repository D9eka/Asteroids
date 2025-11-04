using System;
using Asteroids.Scripts.Camera;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.Input;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Player.Input;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.Restarter;
using Asteroids.Scripts.Score;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Asteroids.Scripts.UI;
using Asteroids.Scripts.WarpSystem;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Camera")]
        [SerializeField] private UnityEngine.Camera _camera;
        [Space]
        [Header("BoundsSystem")]
        [SerializeField] private float _boundsMargin;
        [Space]
        [Header("ProjectilePool")]
        [SerializeField] private Projectile _projectilePrefab;
        [Space]
        [Header("Player")]
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private Vector2 _playerSpawnPosition;
        [SerializeField] private PlayerMovementData _movementData;
        [SerializeField] private BulletGunConfig _bulletGunConfig;
        [SerializeField] private LaserGunConfig _laserGunConfig;
        [Space]
        [Header("Enemies")]
        [SerializeField] private EnemySpawnConfig _enemySpawnConfig;
        [Space]
        [Header("Score")]
        [SerializeField] private ScoreConfig _scoreConfig;
        [Space]
        [Header("UI")] 
        [SerializeField] private GameUIView _gameUIViewPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(_camera).AsSingle();
            
            InstallBoundsSystem();
            InstallProjectilePool();
            InstallPlayer();
            InstallEnemies();
            InstallGameplaySystems();
            InstallUI();
        }

        private void InstallBoundsSystem()
        {
            Container.Bind<float>().WithId(InjectId.BoundsMargin).FromInstance(_boundsMargin).AsCached();
            Container.Bind<ICameraBoundsUpdater>().To<CameraBoundsUpdater>().AsSingle().NonLazy();
            Container.Bind<IBoundsWarp>().To<CameraBoundsWarp>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BoundsManager>().AsSingle().NonLazy();
        }

        private void InstallProjectilePool()
        {
            Container.BindMemoryPool<Projectile, ProjectilePool<Projectile>>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_projectilePrefab)
                .UnderTransformGroup("Projectiles");
            
            Container.Bind<IProjectileFactory>()
                .To<ProjectileFactory>()
                .AsSingle();
        }
        
        private void InstallPlayer()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputReader>().AsSingle();
            
            var playerGo = Container.InstantiatePrefab(_playerPrefab.gameObject);
            playerGo.transform.position = _playerSpawnPosition;
            
            BulletGun bulletGun = playerGo.GetComponentInChildren<BulletGun>();
            LaserGun laserGun = playerGo.GetComponentInChildren<LaserGun>();
            BindPlayerWeapons(bulletGun, laserGun);
            
            PlayerController playerController = playerGo.GetComponent<PlayerController>();
            PlayerMovement playerMovement = playerGo.GetComponent<PlayerMovement>();
            Container.Bind<Vector2>().WithId(InjectId.PlayerStartPos).FromInstance(_playerSpawnPosition).AsSingle();
            Container.Bind<Transform>().WithId(InjectId.PlayerTransform).FromInstance(playerGo.transform).AsSingle();
            Container.Bind<IPlayerController>().FromInstance(playerController).AsSingle();
            Container.Bind<ICollisionService>()
                .WithId(InjectId.PlayerCollisionService)
                .To<PlayerCollisionService>()
                .AsCached();
            Container.Bind<ICollisionHandler>()
                .WithId(InjectId.PlayerCollisionHandler)
                .FromInstance(playerController.GetComponent<CollisionHandler>())
                .AsCached();
            Container.Bind<PlayerMovementData>().FromInstance(_movementData).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovement>().FromInstance(playerMovement).AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerControllerInitializer>().AsSingle().NonLazy();
        }

        private void BindPlayerWeapons(BulletGun bulletGun, LaserGun laserGun)
        {
            BindBulletGun(bulletGun);
            BindLaserGun(laserGun);
            
            Container.Bind<IWeapon[]>().WithId(InjectId.PlayerWeapons)
                .FromInstance(new IWeapon[]{ bulletGun, laserGun })
                .AsCached();
            
            Container.BindInterfacesAndSelfTo<PlayerWeaponsHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WeaponUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerWeaponsInitializer>().AsSingle().NonLazy();
        }

        private void BindBulletGun(BulletGun bulletGun)
        {
            Container.Bind<BulletGunConfig>()
                .FromInstance(_bulletGunConfig)
                .AsCached()
                .WhenInjectedInto<PlayerWeaponsInitializer>();

            Container.Bind<IWeapon>()
                .WithId(InjectId.PlayerBulletGun)
                .To<BulletGun>()
                .FromInstance(bulletGun)
                .AsCached();
        }

        private void BindLaserGun(LaserGun laserGun)
        {
            SpriteLineRenderer lineRenderer = laserGun.GetComponentInChildren<SpriteLineRenderer>();
            
            Container.Bind<LaserGunConfig>()
                .FromInstance(_laserGunConfig)
                .AsCached()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<ILineRenderer>()
                .FromInstance(lineRenderer)
                .AsCached()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<IRaycastService>()
                .To<RaycastService>()
                .AsSingle()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<IWeapon>()
                .WithId(InjectId.PlayerLaserGun)
                .To<LaserGun>()
                .FromInstance(laserGun)
                .AsCached();
            Container.Bind<ILaserGun>()
                .WithId(InjectId.PlayerLaserGun)
                .To<LaserGun>()
                .FromInstance(laserGun)
                .AsCached();
        }

        private void InstallEnemies()
        {
            Container.Bind<EnemySpawnConfig>().FromInstance(_enemySpawnConfig).AsSingle();
            
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PoolableLifecycleManager<IPoolable>>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyLifecycleManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<SpawnPointGenerator>().AsSingle();
            Container.Bind<IEnemyMovementConfigurator>()
                .To<EnemyMovementConfigurator>()
                .AsSingle();
            
            BindEnemy<Ufo, UfoTypeConfig>(EnemyType.Ufo, typeof(UfoInitializer));
            BindEnemy<Asteroid, AsteroidTypeConfig>(EnemyType.Asteroid, typeof(AsteroidInitializer));

            Container.BindInterfacesAndSelfTo<EnemyProvidersInstaller>().AsSingle().NonLazy();
        }
        
        private void BindEnemy<TEnemy, TConfig>(
            EnemyType enemyType,
            Type initializerType)
            where TEnemy : MonoBehaviour, IEnemy
            where TConfig : EnemyTypeConfig
        {
            Container.Bind<IEnemyProviderFactory>()
                .WithId(enemyType)
                .To<EnemyProviderFactory<TEnemy, TConfig>>()
                .AsSingle()
                .WithArguments(enemyType);

            Container.BindInterfacesAndSelfTo(initializerType).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyInitializerAdapter<TEnemy, TConfig>>().AsSingle();
        }

        private void InstallGameplaySystems()
        {
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().WithArguments(_scoreConfig);
            Container.BindInterfacesAndSelfTo<GameRestarter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PauseSystem>().AsSingle();
        }

        private void InstallUI()
        {
            Container.BindInterfacesAndSelfTo<PlayerParamsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameUIViewModel>().AsSingle();
            var uiView = Container.InstantiatePrefab(_gameUIViewPrefab);
            uiView.GetComponent<Canvas>().worldCamera = _camera;
        }
    }
}