using Asteroids.Scripts.Ecs.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Systems
{
    public class LinearMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _linearMovementFilter;
        private EcsPool<LinearDirectionComponent> _linearDirectionPool;
        private EcsPool<VelocityMovementComponent> _velocityMovementPool;
        private EcsPool<VelocityResultComponent> _velocityResultPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _linearMovementFilter = world.Filter<LinearDirectionComponent>()
                .Inc<VelocityMovementComponent>()
                .Inc<VelocityResultComponent>()
                .End();
            _linearDirectionPool = world.GetPool<LinearDirectionComponent>();
            _velocityMovementPool = world.GetPool<VelocityMovementComponent>();
            _velocityResultPool = world.GetPool<VelocityResultComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _linearMovementFilter)
            {
                LinearDirectionComponent linearDirection = _linearDirectionPool.Get(entity);
                VelocityMovementComponent velocityMovement = _velocityMovementPool.Get(entity);
                ref VelocityResultComponent velocityResult = ref _velocityResultPool.Get(entity);
                velocityResult.Velocity = linearDirection.Direction * velocityMovement.Velocity;
            }
        }
    }
}