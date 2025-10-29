using UnityEngine;

namespace _Project.Scripts.Movement.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovementBase : MonoBehaviour, IMovable
    {
        public float Velocity { get; private set; } = Mathf.Infinity;

        protected Rigidbody2D Rigidbody;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public virtual void Stop()
        {
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
        }

        public void SetVelocity(float velocity)
        {
            Velocity = velocity;
        }

        protected void ApplyVelocity(Vector2 velocity)
        {
            Rigidbody.linearVelocity = velocity;
            LimitSpeed();
        }

        protected void ApplyForce(Vector2 force)
        {
            Rigidbody.AddForce(force);
            LimitSpeed();
        }

        protected void ApplyRotation(float angle)
        {
            Rigidbody.MoveRotation(angle);
        }

        private void LimitSpeed()
        {
            if (Rigidbody.linearVelocity.magnitude > Velocity)
                Rigidbody.linearVelocity = Rigidbody.linearVelocity.normalized * Velocity;
        }
    }
}