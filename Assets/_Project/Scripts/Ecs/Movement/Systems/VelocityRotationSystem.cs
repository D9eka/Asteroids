using Asteroids.Scripts.Ecs.Movement.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Movement.Systems
{
    public class VelocityRotationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SPRITE_UP_OFFSET = 90f;
        
        private EcsFilter _velocityMovementFilter;
        private EcsPool<VelocityResultComponent> _velocityMovementPool;
        private EcsPool<RotationResultComponent> _rotationMovementPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _velocityMovementFilter = world.Filter<VelocityRotationTag>()
                .Inc<VelocityResultComponent>()
                .Inc<RotationResultComponent>()
                .End();
            _velocityMovementPool = world.GetPool<VelocityResultComponent>();
            _rotationMovementPool = world.GetPool<RotationResultComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _velocityMovementFilter)
            {
                VelocityResultComponent velocityResult = _velocityMovementPool.Get(entity);
                ref RotationResultComponent rotationResult = ref _rotationMovementPool.Get(entity);
                
                rotationResult.Angle = Mathf.Atan2(velocityResult.Velocity.y, velocityResult.Velocity.x) * Mathf.Rad2Deg - SPRITE_UP_OFFSET;
            }
        }
    }
}