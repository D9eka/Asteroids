using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Advertisement;
using Asteroids.Scripts.Analytics;
using Asteroids.Scripts.Camera;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Authoring.Player;
using Asteroids.Scripts.Configs.Authoring.Score;
using Asteroids.Scripts.Configs.Authoring.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Authoring.Weapons.LaserGun;
using Asteroids.Scripts.Configs.Mapping;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Core.GameExit;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Input;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Player.Input;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.RemoteConfigs;
using Asteroids.Scripts.SaveService;
using Asteroids.Scripts.Score;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Asteroids.Scripts.UI;
using Asteroids.Scripts.UI.Screens.GameplayScreen;
using Asteroids.Scripts.UI.Screens.MainScreen;
using Asteroids.Scripts.UI.Screens.ReviveScreen;
using Asteroids.Scripts.WarpSystem;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using Asteroids.Scripts.Weapons.Services.Raycast;
using UnityEngine;
using Zenject;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

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
        [SerializeField] private PlayerMovementDataSo _movementDataSo;
        [SerializeField] private BulletGunConfigSo _bulletGunConfigSo;
        [SerializeField] private LaserGunConfigSo _laserGunConfigSo;
        [Space]
        [Header("Enemies")]
        [SerializeField] private EnemySpawnConfigSo _enemySpawnConfigSo;
        [Space]
        [Header("Score")]
        [SerializeField] private ScoreConfigSo _scoreConfigSo;
        [Space] 
        [Header("Advertisement")] 
        [SerializeField] private string _adAppId;
        [SerializeField] private string _interstitialAdId;
        [SerializeField] private string _revivalAdId;
        
        public override void InstallBindings()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(_camera).AsSingle();

            Container.BindInterfacesTo<UnityAddressableLoader>().AsSingle();

            InstallRemoteConfigService();
            InstallAdvertisementService();
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

        private void InstallRemoteConfigService()
        {
            Container.BindInterfacesTo<FirebaseRemoteConfigService>().AsSingle();
            Container.Bind<ConfigDataMapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameConfigProvider>().AsSingle()
                .WithArguments(_movementDataSo, _bulletGunConfigSo, _laserGunConfigSo, _enemySpawnConfigSo, _scoreConfigSo);
        }

        private void InstallAdvertisementService()
        {
#if UNITY_EDITOR
            Container.BindInterfacesTo<TestAdvertisementService>()
                .AsSingle();
#else
            Container.BindInterfacesTo<LPlayAdvertisementService>()
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
            
            Container.Bind<ICollisionService>()
                .WithId(CollisionServiceInjectId.Player)
                .To<PlayerCollisionService>()
                .AsCached();
            
            Container.Bind<Vector2>()
                .WithId(Vector2InjectId.PlayerStartPos)
                .FromInstance(_playerSpawnPosition)
                .AsSingle();
            
            BindPlayerWeapons();

            Container.BindInterfacesTo<PlayerControllerInitializer>().AsSingle().NonLazy();
        }

        private void BindPlayerWeapons()
        {
            Container.BindInterfacesTo<WeaponUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<RaycastService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerWeaponsConfigRuntime>().AsSingle();
            Container.Bind<PlayerWeaponsInitializer>().AsSingle();
        }

        private void InstallEnemies()
        {
            Container.Bind<EnemySpawnConfigSo>().FromInstance(_enemySpawnConfigSo).AsSingle();
            
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesTo<PoolableLifecycleManager<Pooling_IPoolable>>().AsSingle();
            Container.BindInterfacesTo<EnemyLifecycleManager>().AsSingle();
            Container.BindInterfacesTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<SpawnPointGenerator>().AsSingle();
            Container.Bind<IEnemyMovementConfigurator>()
                .To<EnemyMovementConfigurator>()
                .AsSingle();
            
            BindEnemy<Ufo, UfoTypeConfig>(EnemyType.Ufo, typeof(UfoInitializer));
            BindEnemy<Asteroid, AsteroidTypeConfig>(EnemyType.Asteroid, typeof(AsteroidInitializer));

            Container.BindInterfacesAndSelfTo<EnemySpawnConfigRuntime>().AsSingle();
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
            Container.BindInterfacesTo<ScoreService>().AsSingle();
            Container.BindInterfacesTo<ScoreTracker>().AsSingle();
            Container.BindInterfacesTo<ScoreConfigRuntime>().AsSingle().NonLazy();
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