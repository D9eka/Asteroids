using UnityEngine;

namespace _Project.Scripts.Movement.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovementBase : MonoBehaviour, IMovable
    {
        [Header("Base Movement Settings")]
        [SerializeField] protected float _maxVelocity = 10f;

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
            _maxVelocity = velocity;
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
            if (Rigidbody.linearVelocity.magnitude > _maxVelocity)
                Rigidbody.linearVelocity = Rigidbody.linearVelocity.normalized * _maxVelocity;
        }
    }
}