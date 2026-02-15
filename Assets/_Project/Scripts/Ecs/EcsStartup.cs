using System;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Ecs.Collision.Systems;
using Asteroids.Scripts.Ecs.Systems;
using Asteroids.Scripts.Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Ecs
{
    public class EcsStartup : IInitializable, IFixedTickable, IDisposable
    {
        private readonly DiContainer _container;
        private readonly EcsWorld _world;
        
        IEcsSystems _systems;

        public EcsStartup(DiContainer container, EcsWorld world)
        {
            _container = container;
            _world = world;
        }
        
        public void Initialize()
        {
            _systems = new EcsSystems(_world);
#if UNITY_EDITOR
            _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
            _systems
                .Add(new InputWriteSystem(_container.Resolve<IPlayerInput>()))
                .Add(new ConfigSyncSystem(_container.Resolve<IPlayerConfigProvider>()))
                .Add(new PlayerMovementSystem())
                .Add(new TargetDirectionSystem())
                .Add(new LinearMovementSystem())
                .Add(new VelocityRotationSystem())
                .Add(new TargetRotationSystem())
                .Add(new DamageSystem())
                .Add(new LifeTimeSystem())
                .Add(new DestroySystem(_container.Resolve<EntityViewRegistry>()))
                .Init();
        }
        
        public void FixedTick()
        {
            _systems?.Run();
        }


        public void Dispose()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}