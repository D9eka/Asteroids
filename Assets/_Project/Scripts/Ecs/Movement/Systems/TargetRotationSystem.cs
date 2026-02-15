using Asteroids.Scripts.Ecs.Movement.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Movement.Systems
{
    public class TargetRotationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SPRITE_UP_OFFSET = 90f;
        
        private EcsFilter _targetRotationFilter;
        private EcsPool<TargetRotationComponent> _targetRotationPool;
        private EcsPool<PositionComponent> _positionPool;
        private EcsPool<RotationResultComponent> _rotationResultPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _targetRotationFilter = world.Filter<TargetRotationComponent>()
                .Inc<PositionComponent>()
                .Inc<RotationResultComponent>()
                .End();
            _targetRotationPool = world.GetPool<TargetRotationComponent>();
            _positionPool = world.GetPool<PositionComponent>();
            _rotationResultPool = world.GetPool<RotationResultComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _targetRotationFilter)
            {
                TargetRotationComponent targetRotationComponent = _targetRotationPool.Get(entity);
                PositionComponent positionComponent = _positionPool.Get(entity);
                ref RotationResultComponent rotationResultComponent = ref _rotationResultPool.Get(entity);
                PositionComponent targetPositionComponent = _positionPool.Get(targetRotationComponent.TargetEntity);
                
                Vector2 direction = (targetPositionComponent.Position - positionComponent.Position).normalized;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - SPRITE_UP_OFFSET;
                rotationResultComponent.Angle = Mathf.LerpAngle(rotationResultComponent.Angle, targetAngle, 
                    targetRotationComponent.RotationSpeed * Time.deltaTime);
            }
        }
    }
}