using System;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Player;
using Asteroids.Scripts.Movement.Core;
using UnityEngine;

namespace Asteroids.Scripts.Player.Movement
{
    public class PlayerMovement : MovementBase, IPlayerMovement, IDisposable
    {
        private IPlayerConfigProvider _playerConfigProvider;
        private float _thrustForce;
        private float _rotationSpeed;

        public void Initialize(IPlayerConfigProvider playerConfigProvider)
        {
            _playerConfigProvider = playerConfigProvider;
            ApplyConfig();
            _playerConfigProvider.OnPlayerConfigUpdated += ApplyConfig;
        }

        public void Move(float input)
        {
            if (input <= 0f) return;
            ApplyForce(transform.up * (_thrustForce * input));
        }

        public void Rotate(float input)
        {
            if (Mathf.Abs(input) < 0.01f) return;

            float newRotation = Rigidbody.rotation - input * _rotationSpeed * Time.fixedDeltaTime;
            ApplyRotation(newRotation);
        }
        
        public void Dispose()
        {
            _playerConfigProvider.OnPlayerConfigUpdated -= ApplyConfig;
        }

        private void ApplyConfig()
        {
            PlayerMovementConfig movementConfig = _playerConfigProvider.PlayerConfig.MovementConfig;
            _thrustForce = movementConfig.ThrustForce;
            _rotationSpeed = movementConfig.RotationSpeed;
            SetVelocity(_thrustForce);
        }
    }
}