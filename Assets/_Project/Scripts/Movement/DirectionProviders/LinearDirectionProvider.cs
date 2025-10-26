using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders
{
    public class LinearDirectionProvider : IDirectionProvider
    {
        private readonly Vector2 _direction;

        public LinearDirectionProvider(Vector2 direction)
        {
            _direction = direction;
        }

        public Vector2 GetDirection(Transform self)
        {
            return _direction;
        }
    }
}