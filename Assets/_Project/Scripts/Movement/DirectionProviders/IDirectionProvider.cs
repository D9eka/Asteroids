using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders
{
    public interface IDirectionProvider
    {
        public Vector2 GetDirection(Transform self);
    }
}