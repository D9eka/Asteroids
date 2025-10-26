using _Project.Scripts.Movement.DirectionProviders;
using UnityEngine;

namespace _Project.Scripts.Movement.Core
{
    public class Movement : MovementBase
    {
        [SerializeField] private float _speed = 5f;
        
        private IDirectionProvider _directionProvider;

        private void FixedUpdate()
        {
            if (_directionProvider == null)
                return;

            Vector2 dir = _directionProvider.GetDirection(transform);
            ApplyVelocity(dir * _speed);

            if (dir.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                ApplyRotation(angle);
            }
        }

        public void SetDirectionProvider(IDirectionProvider provider)
        {
            _directionProvider = provider;
        }
    }
}