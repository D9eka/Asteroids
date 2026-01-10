using System.Collections.Generic;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Cysharp.Threading.Tasks;
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
            List<IEnemyProvider> providers = await CreateProviders();
            _container.Bind<List<IEnemyProvider>>().FromInstance(providers).AsSingle();
            _enemySpawnConfigRuntime.Initialize(providers);
            
            _container.Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle()
                .NonLazy();
            
            _container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().NonLazy();
            _container.Resolve<TickableManager>().Add(_container.Resolve<EnemySpawner>());
            _pauseSystem.Register(_container.Resolve<EnemySpawner>());
        }

        private async UniTask<List<IEnemyProvider>> CreateProviders()
        {
            List<IEnemyProvider> providers = new List<IEnemyProvider>();

            foreach (EnemyTypeSpawnConfig spawnConfig in _enemyConfigProvider.EnemySpawnConfig.Enemies)
            {
                IEnemyProvider provider = await CreateProvider(spawnConfig);
                providers.Add(provider);
                if (spawnConfig.Config is AsteroidTypeConfig asteroidConfig)
                {
                    await CreateAsteroidFragmentFactory(asteroidConfig.AsteroidFragmentSpawnConfig);
                }
            }

            return providers;
        }

        private async UniTask<IEnemyProvider> CreateProvider(EnemyTypeSpawnConfig spawnConfig)
        {
            EnemyType enemyType = spawnConfig.Config.Type;

            IEnemyProviderFactory factory = _container.ResolveId<IEnemyProviderFactory>(enemyType);
            return await factory.Create(spawnConfig);
        }

        private async UniTask CreateAsteroidFragmentFactory(AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            _container.Bind<IEnemyProviderFactory>()
                .WithId(EnemyType.AsteroidFragment)
                .To<EnemyProviderFactory<AsteroidFragment, EnemyTypeConfig>>()
                .AsSingle();
            
            IEnemyProvider provider = await CreateProvider(spawnConfig);

            IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> asteroidFragmentProvider = 
                provider as IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig>;
            
            _container.BindInterfacesAndSelfTo<DefaultEnemyInitializer>().AsSingle();
            _container.BindInterfacesAndSelfTo<AsteroidFragmentFactory>().AsSingle().WithArguments(asteroidFragmentProvider);
        }
    }
}