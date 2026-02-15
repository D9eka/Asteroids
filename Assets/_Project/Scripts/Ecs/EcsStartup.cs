using System;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Ecs.Collision.Systems;
using Asteroids.Scripts.Ecs.Systems;
using Asteroids.Scripts.Input;
using Asteroids.Scripts.Pause;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Ecs
{
    public class EcsStartup : IInitializable, IFixedTickable, IDisposable, ITickableSystem
    {
        private readonly DiContainer _container;
        private readonly EcsWorld _world;
        
        IEcsSystems _coreSystems;
        IEcsSystems _gameplaySystems;
        private bool _isEnabled;

        public EcsStartup(DiContainer container, EcsWorld world)
        {
            _container = container;
            _world = world;
        }
        
        public void Initialize()
        {
            _coreSystems = new EcsSystems(_world);
            _gameplaySystems = new EcsSystems(_world);
#if UNITY_EDITOR
            _coreSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
            _gameplaySystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
            _coreSystems
                .Add(new ConfigSyncSystem(_container.Resolve<IPlayerConfigProvider>()))
                .Add(new DestroySystem(_container.Resolve<EntityViewRegistry>()))
                .Init();

            _gameplaySystems
                .Add(new InputWriteSystem(_container.Resolve<IPlayerInput>()))
                .Add(new PlayerMovementSystem())
                .Add(new TargetDirectionSystem())
                .Add(new LinearMovementSystem())
                .Add(new VelocityRotationSystem())
                .Add(new TargetRotationSystem())
                .Add(new DamageSystem())
                .Add(new LifeTimeSystem())
                .Init();
        }
        
        public void FixedTick()
        {
            _coreSystems?.Run();
            if (_isEnabled) _gameplaySystems?.Run();
        }
        
        public void Dispose()
        {
            _coreSystems.Destroy();
            _gameplaySystems.Destroy();
            _world.Destroy();
        }
        
        public void Enable() => _isEnabled = true;
        
        public void Disable() => _isEnabled = false;
    }
}