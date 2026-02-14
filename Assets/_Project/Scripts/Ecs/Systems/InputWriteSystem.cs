using _Project.Scripts.Ecs.Components;
using Asteroids.Scripts.Input;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    public class InputWriteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IPlayerInput _playerInput;
        
        private EcsFilter _playerFilter;
        private EcsPool<PlayerInputComponent> _inputPool;

        public InputWriteSystem(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<PlayerInputComponent>().End();
            _inputPool = world.GetPool<PlayerInputComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref PlayerInputComponent playerInput = ref _inputPool.Get(entity);
                playerInput.ThrustInput = _playerInput.Move.y;
                playerInput.RotationInput = _playerInput.Move.x;
                playerInput.IsFiring = _playerInput.IsFiring;       
                playerInput.SwitchWeapon = _playerInput.NeedSwitchWeapon;
            }
        }
    }
}