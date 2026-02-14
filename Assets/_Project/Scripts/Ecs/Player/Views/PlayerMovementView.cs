using Asteroids.Scripts.Ecs.Components;
using Asteroids.Scripts.Movement;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementView : MovementBase
    {
        private int _entityId;
        private EcsPool<PositionComponent> _positionsPool;
        private EcsPool<PlayerMovementStatsComponent> _movementStatsPool;
        private EcsPool<PlayerTransformDataComponent> _transformDataPool;
        private EcsPool<PlayerMovementResultComponent> _movementResultPool;

        public void Initialize(EcsWorld world, int entityId)
        {
            _entityId = entityId;
            _positionsPool = world.GetPool<PositionComponent>();
            _movementStatsPool = world.GetPool<PlayerMovementStatsComponent>();
            _transformDataPool = world.GetPool<PlayerTransformDataComponent>();
            _movementResultPool = world.GetPool<PlayerMovementResultComponent>();
        }
        
        private void FixedUpdate()
        {
            ref PositionComponent positionComponent = ref _positionsPool.Get(_entityId);
            positionComponent.Position = transform.position;
            
            SetVelocity(_movementStatsPool.Get(_entityId).ThrustForce);
            
            ref PlayerTransformDataComponent playerTransformDataComponent = ref _transformDataPool.Get(_entityId);
            playerTransformDataComponent.Forward = transform.up;
            
            PlayerMovementResultComponent playerMovementResultComponent = _movementResultPool.Get(_entityId);
            ApplyForce(playerMovementResultComponent.Force);
            ApplyRotation(Rigidbody.rotation + playerMovementResultComponent.RotationDelta * Time.fixedDeltaTime);
        }
    }
}