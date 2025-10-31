using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders
{
    public class TargetDirectionProvider : IDirectionProvider
    {
        private readonly Transform _target;
        private readonly float _updateInterval;
        
        private Vector2 _direction;
        private float _lastUpdateTime;

        public TargetDirectionProvider(Vector2 direction, Transform target, float updateInterval)
        {
            _direction = direction;
            _target = target;
            _updateInterval = updateInterval;
            _lastUpdateTime = Time.time;
        }

        public Vector2 GetDirection(Transform self)
        {
            if (Time.time - _lastUpdateTime >= _updateInterval)
            {
                UpdateDirection(self);
                _lastUpdateTime = Time.time;
            }

            return _direction;
        }

        private void UpdateDirection(Transform self)
        {
            if (_target != null)
            {
                _direction = ((Vector2)_target.position - (Vector2)self.position).normalized;
            }
        }
    }
}