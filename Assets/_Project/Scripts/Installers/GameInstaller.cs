using System;
using Asteroids.Scripts.Camera;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Core.GameExit;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Input;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Player.Input;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.SaveService;
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
using Asteroids.Scripts.UI.Screens;
using Asteroids.Scripts.UI.Screens.GameplayScreen;
using Asteroids.Scripts.UI.Screens.MainScreen;
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
        [SerializeField] private MainScreenView _mainScreenViewPrefab;
        [SerializeField] private GameplayScreenView _gameplayScreenViewPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(_camera).AsSingle();
            
            InstallBoundsSystem();
            InstallProjectilePool();
            InstallPlayer();
            InstallEnemies();
            InstallSaveSystem();
            InstallScoreSystem();
            InstallGameplaySystems();
            InstallUI();
        }

        private void InstallBoundsSystem()
        {
            Container.Bind<float>().WithId(FloatInjectId.BoundsMargin).FromInstance(_boundsMargin).AsCached();
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
            Container.Bind<Vector2>().WithId(Vector2InjectId.PlayerStartPos).FromInstance(_playerSpawnPosition).AsSingle();
            Container.Bind<Transform>().WithId(TransformInjectId.Player).FromInstance(playerGo.transform).AsSingle();
            Container.Bind<IPlayerController>().FromInstance(playerController).AsSingle();
            Container.Bind<ICollisionService>()
                .WithId(CollisionServiceInjectId.Player)
                .To<PlayerCollisionService>()
                .AsCached();
            Container.Bind<ICollisionHandler>()
                .WithId(CollisionHandlerInjectId.Player)
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
            
            Container.Bind<IWeapon[]>().WithId(WeaponInjectId.PlayerWeapons)
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
                .WithId(WeaponInjectId.PlayerBulletGun)
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
                .WithId(WeaponInjectId.PlayerLaserGun)
                .To<LaserGun>()
                .FromInstance(laserGun)
                .AsCached();
            Container.Bind<ILaserGun>()
                .WithId(WeaponInjectId.PlayerLaserGun)
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
                .AsSingle();

            Container.BindInterfacesAndSelfTo(initializerType).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyInitializerAdapter<TEnemy, TConfig>>().AsSingle();
        }
        
        private void InstallSaveSystem()
        {
            Container.BindInterfacesAndSelfTo<PlayerPrefsSaveService>().AsSingle();
        }

        private void InstallScoreSystem()
        {
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().WithArguments(_scoreConfig);
            Container.BindInterfacesAndSelfTo<ScoreTracker>().AsSingle();
        }

        private void InstallGameplaySystems()
        {
            Container.BindInterfacesAndSelfTo<GameplaySessionManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameExitService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PauseSystem>().AsSingle();
        }

        private void InstallUI()
        {
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerParamsService>().AsSingle().NonLazy();
            
            BindScreen<GameplayScreenView, GameplayScreenViewModel>(_gameplayScreenViewPrefab.gameObject, 
                ScreenInjectId.GameplayScreenView);
            BindScreen<MainScreenView, MainScreenViewModel>(_mainScreenViewPrefab.gameObject, 
                ScreenInjectId.MainScreenView);
            
            Container
                .BindInterfacesAndSelfTo<ScreensInitializer>()
                .AsSingle()
                .WithArguments(typeof(MainScreenView))
                .NonLazy();
        }
        
        private void BindScreen<TView, TViewModel>(GameObject prefab, ScreenInjectId screenId)
            where TView : IView
            where TViewModel : IViewModel
        {
            Container.BindInterfacesAndSelfTo<TViewModel>().AsSingle();
            
            var screenGo = Container.InstantiatePrefab(prefab);
            var screen = screenGo.GetComponent<TView>();

            Container.Bind<IView>().FromInstance(screen).AsCached();
            Container.Bind<IView>()
                .WithId(screenId)
                .FromInstance(screen)
                .AsCached();
            
            screenGo.GetComponent<Canvas>().worldCamera = _camera;
        }

    }
}