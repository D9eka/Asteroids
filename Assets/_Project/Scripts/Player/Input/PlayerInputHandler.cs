using Asteroids.Scripts.Input;
using Zenject;

namespace Asteroids.Scripts.Player.Input
{
    public class PlayerInputHandler : ITickable
    {
        private readonly IPlayerInput _input;
        
        private IPlayerController _playerController;

        public PlayerInputHandler(IPlayerInput input)
        {
            _input = input;
        }

        public void Initialize(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Tick()
        {
            if (_playerController == null) return;

            if (_input.IsFiring)
                _playerController.Attack();

            if (_input.NeedSwitchWeapon)
            {
                _playerController.SwitchWeapon();
                _input.NeedSwitchWeapon = false;
            }
        }
    }
}