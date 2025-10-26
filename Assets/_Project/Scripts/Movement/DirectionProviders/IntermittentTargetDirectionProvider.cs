using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders
{
    public class IntermittentTargetDirectionProvider : IDirectionProvider
    {
        private readonly Transform _target;
        private readonly float _updateInterval;
        private readonly float _playerTargetChance;
        
        private Vector2 _direction;
        private float _lastUpdateTime;

        public IntermittentTargetDirectionProvider(Vector2 direction, Transform target, float updateInterval = 2f, 
            float playerTargetChance = 0.75f)
        {
            _direction = direction;
            _target = target;
            _updateInterval = updateInterval;
            _playerTargetChance = playerTargetChance;
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
            if (_target != null && Random.value < _playerTargetChance)
            {
                _direction = ((Vector2)_target.position - (Vector2)self.position).normalized;
            }
        }
    }
}