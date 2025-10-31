using Asteroids.Scripts.Camera;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.WarpSystem
{
    public class CameraBoundsWarp : IBoundsWarp
    {
        private readonly ICameraBoundsUpdater _boundsUpdater;
        private readonly float _boundsMargin;

        public Vector2 MinBounds => _boundsUpdater.MinBounds;
        public Vector2 MaxBounds => _boundsUpdater.MaxBounds;

        [Inject]
        public CameraBoundsWarp(ICameraBoundsUpdater boundsUpdater, [Inject(Id = "BoundsMargin")] float boundsMargin)
        {
            _boundsUpdater = boundsUpdater;
            _boundsMargin = boundsMargin;
        }

        public void WarpObject(Transform obj)
        {
            Vector3 pos = obj.position;

            if (pos.x < MinBounds.x - _boundsMargin)
                pos.x = MaxBounds.x + _boundsMargin;
            else if (pos.x > MaxBounds.x + _boundsMargin)
                pos.x = MinBounds.x - _boundsMargin;

            if (pos.y < MinBounds.y - _boundsMargin)
                pos.y = MaxBounds.y + _boundsMargin;
            else if (pos.y > MaxBounds.y + _boundsMargin)
                pos.y = MinBounds.y - _boundsMargin;

            obj.position = pos;
        }

    }
}