using Asteroids.Scripts.Pause;
using UnityEngine;

namespace Asteroids.Scripts.Movement.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovementBase : MonoBehaviour, IPausable
    {
        public float Velocity { get; private set; } = Mathf.Infinity;

        protected Rigidbody2D Rigidbody;
        
        private bool _isPaused;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public virtual void Pause()
        {
            _isPaused = true;
            Rigidbody.totalForce = Vector2.zero;
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
        }

        public void Resume()
        {
            _isPaused = false;
            SetVelocity(Velocity);
        }

        public void SetVelocity(float velocity)
        {
            Velocity = velocity;
        }

        protected void ApplyVelocity(Vector2 velocity)
        {
            if (_isPaused) return;
            
            Rigidbody.linearVelocity = velocity;
            LimitSpeed();
        }

        protected void ApplyForce(Vector2 force)
        {
            if (_isPaused) return;
            
            Rigidbody.AddForce(force);
            LimitSpeed();
        }

        protected void ApplyRotation(float angle)
        {
            if (_isPaused) return;
            
            Rigidbody.MoveRotation(angle);
        }

        private void LimitSpeed()
        {
            if (Rigidbody.linearVelocity.magnitude > Velocity)
                Rigidbody.linearVelocity = Rigidbody.linearVelocity.normalized * Velocity;
        }
    }
}