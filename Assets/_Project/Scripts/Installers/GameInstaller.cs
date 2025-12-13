using System;
using _Project.Scripts.Advertisement;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Analytics;
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
using Asteroids.Scripts.UI.Screens.EndGameScreen;
using Asteroids.Scripts.UI.Screens.GameplayScreen;
using Asteroids.Scripts.UI.Screens.MainScreen;
using Asteroids.Scripts.WarpSystem;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
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
        [Space]
        [Header("Player")]
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
        [Header("Advertisement")] 
        [SerializeField] private string _adAppId;
        [SerializeField] private string _interstitialAdId;
        [SerializeField] private string _revivalAdId;
        
        public override void InstallBindings()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(_camera).AsSingle();

            Container.BindInterfacesTo<UnityAddressableLoader>().AsSingle();
            
            InstallAdvertisementSystem();
            InstallBoundsSystem();
            InstallProjectilePool();
            InstallPlayer();
            InstallEnemies();
            InstallSaveSystem();
            InstallScoreSystem();
            InstallAnalyticsSystem();
            InstallGameplaySystems();
            InstallUI();
        }

        private void InstallAdvertisementSystem()
        {
#if UNITY_EDITOR
            Container.BindInterfacesTo<TestAdvertisementSystem>()
                .AsSingle();
#else
            Container.BindInterfacesTo<LPlayAdvertisementSystem>()
                .AsSingle()
                .WithArguments(_adAppId, _interstitialAdId, _revivalAdId);
#endif
        }

        private void InstallBoundsSystem()
        {
            Container.Bind<float>().WithId(FloatInjectId.BoundsMargin).FromInstance(_boundsMargin).AsCached();
            Container.Bind<ICameraBoundsUpdater>().To<CameraBoundsUpdater>().AsSingle().NonLazy();
            Container.Bind<IBoundsWarp>().To<CameraBoundsWarp>().AsSingle().NonLazy();
            Container.BindInterfacesTo<BoundsManager>().AsSingle().NonLazy();
        }

        private void InstallProjectilePool()
        {
            Container.BindInterfacesTo<ProjectileFactory>().AsSingle();
            Container.BindInterfacesTo<ProjectileFactoryInitializer>().AsSingle();
        }

        private void InstallPlayer()
        {
            Container.BindInterfacesTo<PlayerInputReader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
            Container.Bind<PlayerMovementData>().FromInstance(_movementData).AsSingle();
            
            Container.Bind<ICollisionService>()
                .WithId(CollisionServiceInjectId.Player)
                .To<PlayerCollisionService>()
                .AsCached();
            
            Container.Bind<Vector2>()
                .WithId(Vector2InjectId.PlayerStartPos)
                .FromInstance(_playerSpawnPosition)
                .AsSingle();
            
            BindPlayerWeapons();

            Container.Bind<PlayerWeaponsInitializer>().AsSingle();
            Container.BindInterfacesTo<PlayerControllerInitializer>().AsSingle().NonLazy();
        }

        private void BindPlayerWeapons()
        {
            Container.Bind<BulletGunConfig>()
                .FromInstance(_bulletGunConfig)
                .AsSingle()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            Container.Bind<LaserGunConfig>()
                .FromInstance(_laserGunConfig)
                .AsSingle()
                .WhenInjectedInto<PlayerWeaponsInitializer>();
            
            Container.BindInterfacesTo<WeaponUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<RaycastService>().AsSingle();
        }

        private void InstallEnemies()
        {
            Container.Bind<EnemySpawnConfig>().FromInstance(_enemySpawnConfig).AsSingle();
            
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesTo<PoolableLifecycleManager<IPoolable>>().AsSingle();
            Container.BindInterfacesTo<EnemyLifecycleManager>().AsSingle();
            Container.BindInterfacesTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<SpawnPointGenerator>().AsSingle();
            Container.Bind<IEnemyMovementConfigurator>()
                .To<EnemyMovementConfigurator>()
                .AsSingle();
            
            BindEnemy<Ufo, UfoTypeConfig>(EnemyType.Ufo, typeof(UfoInitializer));
            BindEnemy<Asteroid, AsteroidTypeConfig>(EnemyType.Asteroid, typeof(AsteroidInitializer));

            Container.BindInterfacesTo<EnemyProvidersInstaller>().AsSingle().NonLazy();
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

            Container.BindInterfacesTo(initializerType).AsSingle();
            Container.BindInterfacesTo<EnemyInitializerAdapter<TEnemy, TConfig>>().AsSingle();
        }

        private void InstallSaveSystem()
        {
            Container.BindInterfacesTo<PlayerPrefsSaveService>().AsSingle();
        }

        private void InstallScoreSystem()
        {
            Container.BindInterfacesTo<ScoreService>().AsSingle().WithArguments(_scoreConfig);
            Container.BindInterfacesTo<ScoreTracker>().AsSingle();
        }

        private void InstallAnalyticsSystem()
        {
            Container.BindInterfacesTo<AnalyticsCollector>().AsSingle();
            Container.BindInterfacesTo<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesTo<AnalyticsController>().AsSingle();
        }

        private void InstallGameplaySystems()
        {
            Container.BindInterfacesTo<GameplaySessionManager>().AsSingle();
            Container.BindInterfacesTo<GameExitService>().AsSingle();
            Container.BindInterfacesTo<GameStateController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PauseSystem>().AsSingle();
        }

        private void InstallUI()
        {
            Container.BindInterfacesTo<UIController>().AsSingle();
            Container.BindInterfacesTo<PlayerParamsService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<ReviveScreenViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayScreenViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainScreenViewModel>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ReviveFlowController>().AsSingle();
            
            Container
                .BindInterfacesTo<ScreensInitializer>()
                .AsSingle()
                .WithArguments(typeof(MainScreenView))
                .NonLazy();
        }
    }
}