using _Project.Scripts.Movement.Core;
using UnityEngine;

namespace _Project.Scripts.Player.Movement
{
    public class PlayerMovement : MovementBase, IPlayerMovement
    {
        private float _thrustForce;
        private float _rotationSpeed;

        public void Initialize(PlayerMovementData data)
        {
            _thrustForce = data.ThrustForce;
            _rotationSpeed = data.RotationSpeed;
            
            SetVelocity(_thrustForce);
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
    }
}