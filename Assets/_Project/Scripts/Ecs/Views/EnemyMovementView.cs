using Asteroids.Scripts.Ecs.Components;
using Asteroids.Scripts.Movement;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Views
{
    public class EnemyMovementView : MovementBase
    {
        private int _entityId;
        private EcsPool<PositionComponent> _positionPool;
        private EcsPool<VelocityResultComponent> _velocityResultPool;
        private EcsPool<RotationResultComponent> _rotationResultPool;

        public void Initialize(EcsWorld world, int entityId)
        {
            _entityId = entityId;
            _positionPool = world.GetPool<PositionComponent>();
            _velocityResultPool = world.GetPool<VelocityResultComponent>();
            _rotationResultPool = world.GetPool<RotationResultComponent>();
        }

        private void FixedUpdate()
        {
            if (_positionPool == null) return;
            
            ref PositionComponent positionComponent = ref _positionPool.Get(_entityId);
            positionComponent.Position = transform.position;
            
            VelocityResultComponent velocityResultComponent = _velocityResultPool.Get(_entityId);
            ApplyVelocity(velocityResultComponent.Velocity);
            
            if (_rotationResultPool.Has(_entityId))
            {
                RotationResultComponent rotationResult = _rotationResultPool.Get(_entityId);
                ApplyRotation(rotationResult.Angle);
            }
        }
    }
}