using _Project.Scripts.Input;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player.Input
{
    public class PlayerInputHandler : ITickable
    {
        private readonly IPlayerController _playerController;
        private readonly IPlayerInput _input;
        
        private Vector2 _lastMove;

        public PlayerInputHandler(IPlayerController playerController, IPlayerInput input)
        {
            _playerController = playerController;
            _input = input;
        }

        public void Tick()
        {
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
            }
        }
    }
}