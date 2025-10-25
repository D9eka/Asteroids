using _Project.Scripts.Core;
using _Project.Scripts.Weapons;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private Movement _movement;
        private IWeapon _weapon;
        private float _moveInput;
        private float _rotateInput;

        private void FixedUpdate()
        {
            _movement.Move(_moveInput);
            _movement.Rotate(_rotateInput);
        }

        public void Initialize(Movement movement, IWeapon weapon)
        {
            _movement = movement;
            _weapon = weapon;
        }

        public void SetInputs(float move, float rotate)
        {
            _moveInput = move;
            _rotateInput = rotate;
        }

        public void Attack()
        {
            _weapon?.Shoot();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}