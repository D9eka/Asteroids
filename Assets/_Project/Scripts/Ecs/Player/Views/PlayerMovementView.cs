using Asteroids.Scripts.Ecs.Movement.Components;
using Asteroids.Scripts.Ecs.Player.Components;
using Asteroids.Scripts.Ecs.Warp.Components;
using Asteroids.Scripts.Movement;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Player.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementView : MovementBase
    {
        private int _entityId;
        private EcsPool<PositionComponent> _positionsPool;
        private EcsPool<PlayerMovementStatsComponent> _movementStatsPool;
        private EcsPool<PlayerTransformDataComponent> _transformDataPool;
        private EcsPool<PlayerMovementResultComponent> _movementResultPool;
        private EcsPool<WarpEventComponent> _warpEventPool;

        public void Initialize(EcsWorld world, int entityId)
        {
            _entityId = entityId;
            _positionsPool = world.GetPool<PositionComponent>();
            _movementStatsPool = world.GetPool<PlayerMovementStatsComponent>();
            _transformDataPool = world.GetPool<PlayerTransformDataComponent>();
            _movementResultPool = world.GetPool<PlayerMovementResultComponent>();
            _warpEventPool = world.GetPool<WarpEventComponent>();
        }

        private void FixedUpdate()
        {
            if (_warpEventPool.Has(_entityId))
            {
                Rigidbody.position = _warpEventPool.Get(_entityId).NewPosition;
            }

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