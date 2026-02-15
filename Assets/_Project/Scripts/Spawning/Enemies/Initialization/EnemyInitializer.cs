using Asteroids.Scripts.Ecs.Views;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public abstract class EnemyInitializer<TEnemy, TConfig> : IEnemyInitializer<TEnemy, TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        protected readonly EcsWorld EcsWorld;
        protected readonly ICollisionService CollisionService;
        protected readonly IEnemyMovementConfigurator MovementConfigurator;
        protected readonly ISpawnBoundaryTracker SpawnBoundaryTracker;
        protected readonly IPauseSystem PauseSystem;
        protected readonly EntityViewRegistry EntityViewRegistry;

        [Inject]
        public EnemyInitializer(EcsWorld ecsWorld,
            EnemyCollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator,
            ISpawnBoundaryTracker spawnBoundaryTracker,
            IPauseSystem pauseSystem, EntityViewRegistry entityViewRegistry)
        {
            EcsWorld = ecsWorld;
            CollisionService = collisionService;
            MovementConfigurator = movementConfigurator;
            SpawnBoundaryTracker = spawnBoundaryTracker;
            PauseSystem = pauseSystem;
            EntityViewRegistry = entityViewRegistry;
        }

        public virtual void Initialize(TEnemy enemy, TConfig config)
        {
            enemy.SetType(config.Type);
            enemy.CollisionHandler.Initialize(CollisionService);
            SpawnBoundaryTracker.RegisterObject(enemy.Transform);
            
            int enemyEntity = EcsWorld.NewEntity();
            GameObject enemyGo = enemy.Transform.gameObject;
            enemy.SetId(enemyEntity);
            MovementConfigurator.Configure(enemyEntity, enemy, enemy.Transform.position, config);
            EnemyMovementView enemyMovementView = enemyGo.GetComponent<EnemyMovementView>();
            enemyMovementView.Initialize(EcsWorld, enemyEntity);
            PauseSystem.Register(enemyMovementView);
            
            EcsWorld.GetPool<EnemyTag>().Add(enemyEntity);
            EcsWorld.GetPool<DestroyOnHitTag>().Add(enemyEntity);
            EcsPool<DamageSourceComponent> damageSourcesPool = EcsWorld.GetPool<DamageSourceComponent>();
            ref DamageSourceComponent damageSourceComponent = ref damageSourcesPool.Add(enemyEntity);
            damageSourceComponent.Type = DamageType.Collide;
            EntityViewRegistry.Register(enemyEntity, enemy);
        }
    }
}