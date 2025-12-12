using Asteroids.Scripts.Input;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player.Input
{
    public class PlayerInputHandler : ITickable
    {
        private readonly IPlayerInput _input;
        
        private IPlayerController _playerController;
        private Vector2 _lastMove;

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
            
            Vector2 move = _input.Move;
            
            Vector2 currentMove = new Vector2(move.x, Mathf.Max(0f, move.y));
            if (currentMove != _lastMove)
            {
                _playerController.SetInputs(currentMove.y, currentMove.x);
                _lastMove = currentMove;
            }

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