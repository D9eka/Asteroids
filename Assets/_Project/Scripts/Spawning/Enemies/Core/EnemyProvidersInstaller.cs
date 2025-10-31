using System.Collections.Generic;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Core
{
    public class EnemyProvidersInstaller : IInitializable
    {
        private readonly DiContainer _container;
        private readonly EnemySpawnConfig _spawnConfig;

        public EnemyProvidersInstaller(DiContainer container, EnemySpawnConfig spawnConfig)
        {
            _container = container;
            _spawnConfig = spawnConfig;
        }
        
        public void Initialize()
        {
            List<IEnemyProvider> providers = CreateProviders();
            _container.Bind<List<IEnemyProvider>>().FromInstance(providers).AsSingle();
            
            _container.Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle()
                .NonLazy();
            
            _container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().NonLazy();
            _container.Resolve<TickableManager>().Add(_container.Resolve<EnemySpawner>());
        }

        private List<IEnemyProvider> CreateProviders()
        {
            List<IEnemyProvider> providers = new List<IEnemyProvider>();

            foreach (EnemyTypeSpawnConfig spawnConfig in _spawnConfig.Enemies)
            {
                providers.Add(CreateProvider(spawnConfig));
                if (spawnConfig.Config is AsteroidTypeConfig asteroidConfig)
                {
                    CreateAsteroidFragmentFactory(asteroidConfig.AsteroidFragmentSpawnConfig);
                }
            }

            return providers;
        }

        private IEnemyProvider CreateProvider(EnemyTypeSpawnConfig spawnConfig)
        {
            EnemyType enemyType = spawnConfig.Config.Type;

            IEnemyProviderFactory factory = _container.ResolveId<IEnemyProviderFactory>(enemyType);
            IEnemyProvider provider = factory.Create(spawnConfig);
            return provider;
        }

        private void CreateAsteroidFragmentFactory(AsteroidFragmentTypeSpawnConfig spawnConfig)
        {
            _container.Bind<IEnemyProviderFactory>()
                .WithId(EnemyType.AsteroidFragment)
                .To<EnemyProviderFactory<AsteroidFragment, EnemyTypeConfig>>()
                .AsSingle()
                .WithArguments(EnemyType.AsteroidFragment);

            IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig> asteroidFragmentProvider = CreateProvider(
                spawnConfig) as IPooledEnemyProvider<AsteroidFragment, EnemyTypeSpawnConfig>;
            
            _container.BindInterfacesAndSelfTo<DefaultEnemyInitializer>().AsSingle();
            _container.BindInterfacesAndSelfTo<AsteroidFragmentFactory>().AsSingle().WithArguments(asteroidFragmentProvider);
        }
    }
}