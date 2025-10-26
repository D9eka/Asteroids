using _Project.Scripts.Movement;
using _Project.Scripts.Movement.Core;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerMovement : MovementBase
    {
        [Header("Player Movement Settings")]
        [SerializeField] private float _thrustForce = 5f;
        [SerializeField] private float _rotationSpeed = 200f;

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