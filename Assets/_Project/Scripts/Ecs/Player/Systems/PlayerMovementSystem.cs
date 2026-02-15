using Asteroids.Scripts.Ecs.Player.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Player.Systems
{
    public class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsPool<PlayerMovementStatsComponent> _movementStatsPool;
        private EcsPool<PlayerTransformDataComponent> _transformDataPool;
        private EcsPool<PlayerMovementResultComponent> _movementResultPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<PlayerInputComponent>()
                .Inc<PlayerMovementStatsComponent>()
                .Inc<PlayerTransformDataComponent>()
                .Inc<PlayerMovementResultComponent>()
                .End();
            _inputPool = world.GetPool<PlayerInputComponent>();
            _movementStatsPool = world.GetPool<PlayerMovementStatsComponent>();
            _transformDataPool = world.GetPool<PlayerTransformDataComponent>();
            _movementResultPool = world.GetPool<PlayerMovementResultComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref PlayerInputComponent inputComponent = ref _inputPool.Get(entity);
                ref PlayerMovementStatsComponent playerMovementStatsComponent = ref _movementStatsPool.Get(entity);
                PlayerTransformDataComponent playerTransformDataComponent = _transformDataPool.Get(entity);
                ref PlayerMovementResultComponent playerMovementResultComponent = ref _movementResultPool.Get(entity);

                Move(inputComponent.ThrustInput, 
                    ref playerMovementStatsComponent, playerTransformDataComponent, ref playerMovementResultComponent);
                Rotate(inputComponent.RotationInput, ref playerMovementStatsComponent, ref playerMovementResultComponent);
            }
        }
        
        private void Move(float input, 
            ref PlayerMovementStatsComponent playerMovementStatsComponent, 
            PlayerTransformDataComponent playerTransformDataComponent, 
            ref PlayerMovementResultComponent playerMovementResultComponent)
        {
            playerMovementResultComponent.Force = Vector2.zero;
            if (input <= 0f) return;
            
            playerMovementResultComponent.Force = playerTransformDataComponent.Forward * 
                (playerMovementStatsComponent.ThrustForce * input);
        }

        private void Rotate(float input, 
            ref PlayerMovementStatsComponent playerMovementStatsComponent,
            ref PlayerMovementResultComponent playerMovementResultComponent)
        {
            playerMovementResultComponent.RotationDelta = 0;
            if (Mathf.Abs(input) < 0.01f) return;

            playerMovementResultComponent.RotationDelta = - 
                input * playerMovementStatsComponent.RotationSpeed;
        }
    }
}