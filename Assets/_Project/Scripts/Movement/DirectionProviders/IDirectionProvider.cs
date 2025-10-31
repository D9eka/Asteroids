using UnityEngine;

namespace Asteroids.Scripts.Movement.DirectionProviders
{
    public interface IDirectionProvider
    {
        public Vector2 GetDirection(Transform self);
    }
}