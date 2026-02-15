using System;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Ecs.Collision.Systems;
using Asteroids.Scripts.Ecs.Lifecycle.Systems;
using Asteroids.Scripts.Ecs.Movement.Systems;
using Asteroids.Scripts.Ecs.Player.Systems;
using Asteroids.Scripts.Ecs.Warp.Systems;
using Asteroids.Scripts.Ecs.Weapons.Services;
using Asteroids.Scripts.Ecs.Weapons.Systems;
using Asteroids.Scripts.Input;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Ecs.Warp.Services;
using Leopotam.EcsLite;
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
            WeaponViewRegistry weaponViewRegistry = _container.Resolve<WeaponViewRegistry>();
            IBoundsWarp boundsWarp = _container.Resolve<IBoundsWarp>();
            
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
                .Add(new PlayerWeaponSwitchSystem())
                .Add(new PlayerWeaponWantsToShootSystem())
                .Add(new TargetDirectionSystem())
                .Add(new LinearMovementSystem())
                .Add(new VelocityRotationSystem())
                .Add(new TargetRotationSystem())
                .Add(new SpawnProtectionSystem(boundsWarp))
                .Add(new WarpSystem(boundsWarp))
                .Add(new DamageSystem())
                .Add(new LifeTimeSystem())
                .Add(new BulletGunSystem(weaponViewRegistry))
                .Add(new LaserGunSystem(weaponViewRegistry))
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