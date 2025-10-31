using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Core;
using Asteroids.Scripts.Spawning.Enemies.Initialization;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawnConfig _enemySpawnConfig;
        [SerializeField] private Transform _playerTransform;

        public override void InstallBindings()
        {
            Container.Bind<EnemySpawnConfig>().FromInstance(_enemySpawnConfig).AsSingle();
            
            Container.Bind<ICollisionService>().To<EnemyCollisionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PoolableLifecycleManager<Pooling_IPoolable>>().AsSingle();
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
