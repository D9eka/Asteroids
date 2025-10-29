using System;
using System.Collections.Generic;
using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Common.Core;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Core;
using _Project.Scripts.Spawning.Enemies.Initialization;
using _Project.Scripts.Spawning.Enemies.Movement;
using _Project.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawnConfig _enemyConfig;
        [SerializeField] private Transform _playerTransform;

        public override void InstallBindings()
        {
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<SpawnPointGenerator>().AsSingle();
            Container.Bind<EnemyMovementConfigurator>().AsSingle().WithArguments(_playerTransform);
            
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
                .To<EnemyProviderFactory<Ufo, UfoTypeConfig>>()
                .AsSingle();

            Container.Bind<IEnemyInitializer<Ufo, UfoTypeConfig>>()
                .To<UfoInitializer>()
                .AsSingle();

            Container.Bind<IEnemyInitializerBase>()
                .To<EnemyInitializerAdapter<Ufo, UfoTypeConfig>>()
                .AsSingle();
        }

        private void BindAsteroid()
        {
            Container.Bind<IEnemyProviderFactory>()
                .To<EnemyProviderFactory<Asteroid, EnemyTypeConfig>>()
                .AsSingle();

            Container.Bind<IEnemyInitializer<IEnemy, EnemyTypeConfig>>()
                .To<EnemyInitializer>()
                .AsSingle();

            Container.Bind<IEnemyInitializerBase>()
                .To<EnemyInitializerAdapter<IEnemy, EnemyTypeConfig>>()
                .AsSingle();
        }

        private List<IEnemyProvider> CreateProviders()
        {
            var providers = new List<IEnemyProvider>();
            var factories = Container.ResolveAll<IEnemyProviderFactory>();

            var factoryMap = new Dictionary<Type, IEnemyProviderFactory>();
            foreach (var factory in factories)
                factoryMap[factory.ConfigType] = factory;

            foreach (var config in _enemyConfig.Enemies)
            {
                var configType = config.GetType();

                if (factoryMap.TryGetValue(configType, out var factory))
                {
                    var provider = factory.Create(config);
                    providers.Add(provider);
                }
                else
                {
                    Debug.LogWarning($"No provider factory found for config type {configType.Name}");
                }
            }

            return providers;
        }
    }
}
