using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning;
using _Project.Scripts.Spawning.Config;
using _Project.Scripts.Spawning.Core;
using _Project.Scripts.Spawning.Factory;
using _Project.Scripts.Spawning.Movement;
using _Project.Scripts.Spawning.Pooling;
using _Project.Scripts.Spawning.Providers;
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
            Container.BindInterfacesAndSelfTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<EnemySpawnPointGenerator>().AsSingle();
            Container.Bind<EnemyMovementConfigurator>().AsSingle().WithArguments(_playerTransform);

            var providers = CreateProviders();
            Container.Bind<List<IEnemyProvider>>().FromInstance(providers).AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().NonLazy();
        }

        private List<IEnemyProvider> CreateProviders()
        {
            var providers = new List<IEnemyProvider>();

            foreach (var config in _enemyConfig.Enemies)
            {
                if (!config.Prefab.TryGetComponent(out IEnemy enemy))
                    continue;

                switch (enemy)
                {
                    case Asteroid:
                        providers.Add(CreateProvider<Asteroid>(config));
                        break;
                    case Ufo:
                        providers.Add(CreateProvider<Ufo>(config));
                        break;
                }
            }

            return providers;
        }

        private IEnemyProvider CreateProvider<T>(EnemyTypeConfig config)
            where T : MonoBehaviour, IEnemy
        {
            Container.BindMemoryPool<T, GenericEnemyPool<T>>()
                .WithInitialSize(config.PoolSize)
                .FromComponentInNewPrefab(config.Prefab)
                .UnderTransformGroup($"{typeof(T).Name}s");

            var pool = Container.Resolve<GenericEnemyPool<T>>();

            return new PooledEnemyProvider<T>(pool, config);
        }
    }
}
