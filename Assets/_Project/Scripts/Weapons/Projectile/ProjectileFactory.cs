using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Components;
using Asteroids.Scripts.Ecs.Views;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly EcsWorld _ecsWorld;
        private readonly IPauseSystem _pauseSystem;
        private readonly IPoolableLifecycleManager<IPoolable> _lifecycleManager;
        
        private ProjectilePool _pool;
        
        private EcsPool<LinearDirectionComponent> _linearDirectionPool;
        private EcsPool<VelocityMovementComponent> _velocityMovementPool;
        private EcsPool<VelocityResultComponent> _velocityResultPool;

        [Inject]
        public ProjectileFactory(EcsWorld ecsWorld, IPauseSystem pauseSystem, 
            IPoolableLifecycleManager<IPoolable> lifecycleManager)
        {
            _ecsWorld = ecsWorld;
            _pauseSystem = pauseSystem;
            _lifecycleManager = lifecycleManager;
        }

        public void Initialize(ProjectilePool pool)
        {
            _pool = pool;
            _linearDirectionPool = _ecsWorld.GetPool<LinearDirectionComponent>();
            _velocityMovementPool = _ecsWorld.GetPool<VelocityMovementComponent>();
            _velocityResultPool = _ecsWorld.GetPool<VelocityResultComponent>();
        }

        public void Create(Vector2 position, Quaternion rotation, 
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService)
        {
            if (_pool == null) return;
            
            int projectileEntity = _ecsWorld.NewEntity();
            ProjectilePoolData data = new ProjectilePoolData(
                projectileEntity, position, rotation, config, collisionService);
            Projectile projectile = _pool.Spawn(data);
            
            ref LinearDirectionComponent linearDirectionComponent = ref _linearDirectionPool.Add(projectileEntity);
            linearDirectionComponent.Direction = projectile.transform.up;
            ref VelocityMovementComponent velocityMovementComponent = ref _velocityMovementPool.Add(projectileEntity);
            velocityMovementComponent.Velocity = config.Speed;
            _velocityResultPool.Add(projectileEntity);
            
            _lifecycleManager.Register(projectile, _pool);
            
            _ecsWorld.GetPool<ProjectileTag>().Add(projectileEntity);
            _ecsWorld.GetPool<DestroyOnHitTag>().Add(projectileEntity);
            EcsPool<DamageSourceComponent> damageSourcesPool = _ecsWorld.GetPool<DamageSourceComponent>();
            ref DamageSourceComponent damageSourceComponent = ref damageSourcesPool.Add(projectileEntity);
            damageSourceComponent.Type = damageInfo.Type;
            EcsPool<OwnerComponent> ownerComponentsPool = _ecsWorld.GetPool<OwnerComponent>();
            ref OwnerComponent ownerComponent = ref ownerComponentsPool.Add(projectileEntity);
            ownerComponent.OwnerEntity = damageInfo.InstigatorEntity.Id;
            _pauseSystem.Register(projectile.GetComponent<ProjectileMovementView>());
            EcsPool<LifeTimeComponent> lifeTimesPool = _ecsWorld.GetPool<LifeTimeComponent>();
            ref LifeTimeComponent lifeTimeComponent = ref lifeTimesPool.Add(projectileEntity);
            lifeTimeComponent.RemainingTime = config.LifeTime;
        }
    }
}