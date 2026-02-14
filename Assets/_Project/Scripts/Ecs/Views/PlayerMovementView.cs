using _Project.Scripts.Ecs.Components;
using Asteroids.Scripts.Movement.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementView : MovementBase
    {
        private int _entityId;
        private EcsPool<MovementStatsComponent> _movementStatsPool;
        private EcsPool<TransformDataComponent> _transformDataPool;
        private EcsPool<MovementResultComponent> _movementResultPool;

        public void Initialize(EcsWorld world, int entityId)
        {
            _entityId = entityId;
            _movementStatsPool = world.GetPool<MovementStatsComponent>();
            _transformDataPool = world.GetPool<TransformDataComponent>();
            _movementResultPool = world.GetPool<MovementResultComponent>();
        }
        
        private void FixedUpdate()
        {
            SetVelocity(_movementStatsPool.Get(_entityId).ThrustForce);
            
            ref TransformDataComponent transformDataComponent = ref _transformDataPool.Get(_entityId);
            transformDataComponent.Forward = transform.up;
            transformDataComponent.CurrentRotation = Rigidbody.rotation;
            
            MovementResultComponent movementResultComponent = _movementResultPool.Get(_entityId);
            ApplyForce(movementResultComponent.Force);
            ApplyRotation(Rigidbody.rotation + movementResultComponent.RotationDelta * Time.fixedDeltaTime);
        }
    }
}