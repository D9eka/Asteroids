using System;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Player
{
    public class PlayerController : NetworkBehaviour, IPlayerController
    {
        public event Action<IPlayerController> OnKilled;

        [Networked] private Vector2 NetPosition { get; set; }
        [Networked] private float NetRotation { get; set; }
        
        private IPlayerMovement _movement;
        private IWeaponHandler _weaponHandler;
        private float _moveInput;
        private float _rotateInput;
        private Rigidbody2D _rigidbody;
        private bool _isDead;
        private bool _deathHandled;
        
        public Transform Transform => transform;

        public override void Spawned()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = Object.HasStateAuthority;
        }

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            _rigidbody.angularVelocity = 0f;

            if (GetInput(out _Project.Scripts.Multiplayer.Input.PlayerNetInput input))
            {
                SetInputs(input.Move.y, input.Move.x);

                if (input.Fire)
                    Attack();

                if (input.SwitchWeapon)
                    SwitchWeapon();
            }

            _movement.Move(_moveInput);
            _movement.Rotate(_rotateInput);

            NetPosition = transform.position;
            NetRotation = _rigidbody.rotation;
        }

        public override void Render()
        {
            if (Object.HasStateAuthority)
                return;

            transform.SetPositionAndRotation(NetPosition, Quaternion.Euler(0f, 0f, NetRotation));
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
            _weaponHandler?.CurrentWeapon?.Shoot();
        }

        public void SwitchWeapon()
        {
            _weaponHandler?.SwitchWeapon();
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (!Object.HasStateAuthority)
                return;

            if (_isDead)
                return;

            _isDead = true;
            RpcNotifyKilled();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RpcNotifyKilled()
        {
            if (_deathHandled)
                return;

            _deathHandled = true;
            _isDead = true;
            OnKilled?.Invoke(this);
            ApplyDeathState();
        }

        private void ApplyDeathState()
        {
            gameObject.SetActive(false);
            _rigidbody.linearVelocity = Vector2.zero;
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }

        public void Pause()
        {
            _movement.Pause();
        }

        public void Resume()
        {
            _movement.Resume();
        }
    }
}
