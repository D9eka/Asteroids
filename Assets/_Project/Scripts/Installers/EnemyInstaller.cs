using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Pause;
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
            Container.Bind<EnemySpawnConfig>().FromInstance(_enemySpawnConfig).AsSingle();
            
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PoolableLifecycleManager<IPoolable>>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyLifecycleManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnBoundaryTracker>().AsSingle();
            Container.Bind<SpawnPointGenerator>().AsSingle();
            Container.Bind<IEnemyMovementConfigurator>()
                .To<EnemyMovementConfigurator>()
                .AsSingle()
                .WithArguments(_playerTransform);
            
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
    }
}
