using _Project.Scripts.Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    public class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsPool<MovementStatsComponent> _movementStatsPool;
        private EcsPool<TransformDataComponent> _transformDataPool;
        private EcsPool<MovementResultComponent> _movementResultPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<PlayerInputComponent>()
                .Inc<MovementStatsComponent>()
                .Inc<TransformDataComponent>()
                .Inc<MovementResultComponent>()
                .End();
            _inputPool = world.GetPool<PlayerInputComponent>();
            _movementStatsPool = world.GetPool<MovementStatsComponent>();
            _transformDataPool = world.GetPool<TransformDataComponent>();
            _movementResultPool = world.GetPool<MovementResultComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref PlayerInputComponent inputComponent = ref _inputPool.Get(entity);
                ref MovementStatsComponent movementStatsComponent = ref _movementStatsPool.Get(entity);
                TransformDataComponent transformDataComponent = _transformDataPool.Get(entity);
                ref MovementResultComponent movementResultComponent = ref _movementResultPool.Get(entity);

                Move(inputComponent.ThrustInput, 
                    ref movementStatsComponent, transformDataComponent, ref movementResultComponent);
                Rotate(inputComponent.RotationInput, ref movementStatsComponent, ref movementResultComponent);
            }
        }
        
        private void Move(float input, 
            ref MovementStatsComponent movementStatsComponent, 
            TransformDataComponent transformDataComponent, 
            ref MovementResultComponent movementResultComponent)
        {
            movementResultComponent.Force = Vector2.zero;
            if (input <= 0f) return;
            
            movementResultComponent.Force = transformDataComponent.Forward * 
                (movementStatsComponent.ThrustForce * input);
        }

        private void Rotate(float input, 
            ref MovementStatsComponent movementStatsComponent,
            ref MovementResultComponent movementResultComponent)
        {
            movementResultComponent.RotationDelta = 0;
            if (Mathf.Abs(input) < 0.01f) return;

            movementResultComponent.RotationDelta = - 
                input * movementStatsComponent.RotationSpeed;
        }
    }
}