using UnityEngine;

namespace Asteroids.Scripts.Movement.RotationProviders
{
    public class TargetDirectionRotationProvider : IRotationProvider
    {
        private readonly Transform _self;
        private readonly Transform _target;
        private readonly float _rotationSpeed;
        
        public float CurrentRotation
        {
            get
            {
                if (_target == null) return _self.eulerAngles.z;

                Vector2 dir = ((Vector2)_target.position - (Vector2)_self.position).normalized;
                float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                float angle = Mathf.LerpAngle(_self.eulerAngles.z, targetAngle, _rotationSpeed * Time.fixedDeltaTime);
                return angle;
            }
        }

        public TargetDirectionRotationProvider(Transform self, Transform target, float rotationSpeed)
        {
            _self = self;
            _target = target;
            _rotationSpeed = rotationSpeed;
        }
    }
}