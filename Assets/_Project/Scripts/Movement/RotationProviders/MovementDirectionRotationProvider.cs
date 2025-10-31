using UnityEngine;

namespace Asteroids.Scripts.Movement.RotationProviders
{
    public class MovementDirectionRotationProvider : IRotationProvider
    {
        private readonly Rigidbody2D _rigidbody;
        
        public float CurrentRotation
        {
            get
            {
                Vector2 velocity = _rigidbody.linearVelocity;
                if (velocity.sqrMagnitude < 0.01f) return _rigidbody.transform.rotation.z;

                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
                return angle;
            }
        }
        
        public MovementDirectionRotationProvider(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }
    }
}