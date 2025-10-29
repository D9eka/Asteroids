using _Project.Scripts.Movement.DirectionProviders;
using _Project.Scripts.Movement.RotationProviders;
using UnityEngine;

namespace _Project.Scripts.Movement.Core
{
    public class Movement : MovementBase
    {
        private IDirectionProvider _directionProvider;
        private IRotationProvider _rotationProvider;

        private void FixedUpdate()
        {
            if (_directionProvider == null) return;

            Vector2 dir = _directionProvider.GetDirection(transform);
            ApplyVelocity(dir * Velocity);

            if (_rotationProvider == null) return;
            
            float targetRotation = _rotationProvider.CurrentRotation;
            ApplyRotation(targetRotation);
        }

        public void SetDirectionProvider(IDirectionProvider provider)
        {
            _directionProvider = provider;
        }

        public void SetRotationProvider(IRotationProvider provider)
        {
            _rotationProvider = provider;
        }
    }
}