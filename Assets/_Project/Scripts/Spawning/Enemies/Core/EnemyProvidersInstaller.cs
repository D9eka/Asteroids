using System.Collections.Generic;
using System.Threading.Tasks;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Core
{
    public class EnemyProvidersInstaller : IInitializable
    {
        private readonly IPauseSystem _pauseSystem;
        private readonly DiContainer _container;
        private readonly IEnemyConfigProvider _enemyConfigProvider;
        private readonly EnemySpawnConfigRuntime _enemySpawnConfigRuntime;

        public EnemyProvidersInstaller(DiContainer container, IPauseSystem pauseSystem, IEnemyConfigProvider enemyConfigProvider, EnemySpawnConfigRuntime enemySpawnConfigRuntime)
        {
            _container = container;
            _pauseSystem = pauseSystem;
            _enemyConfigProvider = enemyConfigProvider;
            _enemySpawnConfigRuntime = enemySpawnConfigRuntime;
        }
        
        public async void Initialize()
        {
            Task<List<IEnemyProvider>> createProvidersTask = CreateProviders();
            await createProvidersTask;
            _container.Bind<List<IEnemyProvider>>().FromInstance(createProvidersTask.Result).AsSingle();
            _enemySpawnConfigRuntime.Initialize(createProvidersTask.Result);
            
            _container.Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle()
                .NonLazy();
            
            _container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().NonLazy();
            _container.Resolve<TickableManager>().Add(_container.Resolve<EnemySpawner>());
            _pauseSystem.Register(_container.Resolve<EnemySpawner>());
        }

        private async Task<List<IEnemyProvider>> CreateProviders()
        {
            List<IEnemyProvider> providers = new List<IEnemyProvider>();

            foreach (EnemyTypeSpawnConfig spawnConfig in _enemyConfigProvider.EnemySpawnConfig.Enemies)
            {
                Task<IEnemyProvider> createProviderTask = CreateProvider(spawnConfig);
                await createProviderTask;
                providers.Add(createProviderTask.Result);
                if (spawnConfig.Config is AsteroidTypeConfig asteroidConfig)
                {
                    await CreateAsteroidFragmentFactory(asteroidConfig.AsteroidFragmentSpawnConfig);
                }
            }

            return providers;
        }

        private async Task<IEnemyProvider> CreateProvider(EnemyTypeSpawnConfig spawnConfig)
        {
            EnemyType enemyType = spawnConfig.Config.Type;

            IEnemyProviderFactory factory = _container.ResolveId<IEnemyProviderFactory>(enemyType);
            Task<IEnemyProvider> createProviderTask = factory.Create(spawnConfig);
            await createProviderTask;
            return createProviderTask.Result;
        }

        private async Task CreateAsteroidFragmentFactory(AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            _container.Bind<IEnemyProviderFactory>()
                .WithId(EnemyType.AsteroidFragment)
                .To<EnemyProviderFactory<AsteroidFragment, EnemyTypeConfig>>()
                .AsSingle();
            
            Task<IEnemyProvider> createProviderTask = CreateProvider(spawnConfig);
            await createProviderTask;

            IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> asteroidFragmentProvider = 
                createProviderTask.Result as IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig>;
            
            _container.BindInterfacesAndSelfTo<DefaultEnemyInitializer>().AsSingle();
            _container.BindInterfacesAndSelfTo<AsteroidFragmentFactory>().AsSingle().WithArguments(asteroidFragmentProvider);
        }
    }
}