using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Scripts.Input
{
    public class PlayerInputReader : IPlayerInput, IInitializable, IDisposable
    {
        private PlayerInputActions _inputActions;
        
        public Vector2 Move { get; private set; }
        public bool IsFiring { get; private set; }
        
        public bool NeedSwitchWeapon {get; private set;}

        public void Initialize()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.Enable();

            _inputActions.Player.Move.performed += OnMove;
            _inputActions.Player.Move.canceled += OnMove;

            _inputActions.Player.Fire.started += OnFireStarted;
            _inputActions.Player.Fire.canceled += OnFireCanceled;

            _inputActions.Player.SwitchWeapon.started += OnSwitchWeaponStarted;
            _inputActions.Player.SwitchWeapon.canceled += OnChangeWeaponCanceled;
        }

        public void Dispose()
        {
            _inputActions.Player.Move.performed -= OnMove;
            _inputActions.Player.Move.canceled -= OnMove;
            
            _inputActions.Player.Fire.started -= OnFireStarted;
            _inputActions.Player.Fire.canceled -= OnFireCanceled;
            
            _inputActions.Player.SwitchWeapon.started -= OnSwitchWeaponStarted;
            _inputActions.Player.SwitchWeapon.canceled -= OnChangeWeaponCanceled;

            _inputActions.Dispose();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void OnFireStarted(InputAction.CallbackContext context)
        {
            IsFiring = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            IsFiring = false;
        }

        private void OnSwitchWeaponStarted(InputAction.CallbackContext context)
        {
            NeedSwitchWeapon = true;
        }

        private void OnChangeWeaponCanceled(InputAction.CallbackContext context)
        {
            NeedSwitchWeapon = false;
        }
    }
}