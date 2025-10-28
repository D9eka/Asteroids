using _Project.Scripts.Player.Movement;
using _Project.Scripts.Player.Weapons;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private IPlayerMovement _movement;
        private IWeaponHandler _weaponHandler;
        private float _moveInput;
        private float _rotateInput;
        
        public Transform Transform => transform;

        private void FixedUpdate()
        {
            _movement.Move(_moveInput);
            _movement.Rotate(_rotateInput);
        }

        public void Initialize(IPlayerMovement movement, IWeaponHandler weaponHandler)
        {
            _movement = movement;
            _weaponHandler = weaponHandler;
        }

        public void SetInputs(float move, float rotate)
        {
            _moveInput = move;
            _rotateInput = rotate;
        }

        public void Attack()
        {
            _weaponHandler.CurrentWeapon?.Shoot();
        }

        public void SwitchWeapon()
        {
            _weaponHandler.SwitchWeapon();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}