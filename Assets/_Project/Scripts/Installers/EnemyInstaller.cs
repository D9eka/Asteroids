using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Common.Pooling;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Core;
using _Project.Scripts.Spawning.Enemies.Fragments;
using _Project.Scripts.Spawning.Enemies.Initialization;
using _Project.Scripts.Spawning.Enemies.Movement;
using _Project.Scripts.Spawning.Enemies.Pooling;
using _Project.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;
using IPoolable = _Project.Scripts.Spawning.Common.Pooling.IPoolable;

namespace _Project.Scripts.Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawnConfig _enemySpawnConfig;
        [SerializeField] private Transform _playerTransform;

        public override void InstallBindings()
        {
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PoolableLifecycleManager<IPoolable>>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyLifecycleManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<SpawnPointGenerator>().AsSingle();
            Container.Bind<IEnemyMovementConfigurator>().To<EnemyMovementConfigurator>().AsSingle().WithArguments(_playerTransform);
            
            BindUfo();
            BindAsteroid();

            var providers = CreateProviders();
            Container.Bind<List<IEnemyProvider>>().FromInstance(providers).AsSingle();

            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().NonLazy();
        }
        
        private void BindUfo()
        {
            Container.Bind<IEnemyProviderFactory>()
                .WithId(EnemyType.Ufo)
                .To<EnemyProviderFactory<Ufo, EnemyTypeConfig>>()
                .AsSingle()
                .WithArguments(EnemyType.Ufo);

            Container.Bind<IEnemyInitializer<Ufo, UfoTypeConfig>>()
                .To<UfoInitializer>()
                .AsSingle();

            Container.Bind<IEnemyInitializerBase>()
                .To<EnemyInitializerAdapter<Ufo, UfoTypeConfig>>()
                .AsSingle();
        }

        private void BindAsteroid()
        {
            EnemyTypeSpawnConfig asteroidSpawnConfig = _enemySpawnConfig.Enemies
                .FirstOrDefault(e => e.Config is AsteroidTypeConfig);
            AsteroidTypeConfig asteroidTypeConfig = (AsteroidTypeConfig)asteroidSpawnConfig.Config;

            AsteroidFragmentTypeSpawnConfig asteroidFragmentSpawnConfig = 
                asteroidTypeConfig.AsteroidFragmentSpawnConfig;
            
            Container.BindMemoryPool<AsteroidFragment, GenericPool<AsteroidFragment>>()
                .WithInitialSize(asteroidFragmentSpawnConfig.PoolSize)
                .FromComponentInNewPrefab(asteroidFragmentSpawnConfig.Config.Prefab)
                .UnderTransformGroup($"{typeof(AsteroidFragment).Name}s");
            
            Container.Bind<IAsteroidFragmentFactory>().To<AsteroidFragmentFactory>().AsSingle();
            
            Container.Bind<IEnemyProviderFactory>()
                .WithId(EnemyType.Asteroid)
                .To<EnemyProviderFactory<Asteroid, AsteroidTypeConfig>>()
                .AsSingle()
                .WithArguments(EnemyType.Asteroid);

            Container.Bind<IEnemyInitializer<Asteroid, AsteroidTypeConfig>>()
                .To<AsteroidInitializer>()
                .AsSingle();

            Container.Bind<IEnemyInitializerBase>()
                .To<EnemyInitializerAdapter<Asteroid, AsteroidTypeConfig>>()
                .AsSingle();
        }

        private List<IEnemyProvider> CreateProviders()
        {
            var providers = new List<IEnemyProvider>();

            foreach (var spawnConfig in _enemySpawnConfig.Enemies)
            {
                EnemyType enemyType = spawnConfig.Config.Type;

                var factory = Container.ResolveId<IEnemyProviderFactory>(enemyType);
                var provider = factory.Create(spawnConfig);
                providers.Add(provider);
            }

            return providers;
        }
    }
}
