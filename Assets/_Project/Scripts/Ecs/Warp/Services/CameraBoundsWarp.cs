using Asteroids.Scripts.Camera;
using Asteroids.Scripts.Core.InjectIds;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Ecs.Warp.Services
{
    public class CameraBoundsWarp : IBoundsWarp
    {
        private readonly ICameraBoundsUpdater _boundsUpdater;
        
        public float BoundsMargin { get; private set; }
        public Vector2 MinBounds => _boundsUpdater.MinBounds;
        public Vector2 MaxBounds => _boundsUpdater.MaxBounds;

        [Inject]
        public CameraBoundsWarp(ICameraBoundsUpdater boundsUpdater, 
            [Inject(Id = FloatInjectId.BoundsMargin)] float boundsMargin)
        {
            _boundsUpdater = boundsUpdater;
            BoundsMargin = boundsMargin;
        }

        public void WarpObject(Transform obj)
        {
            Vector3 pos = obj.position;

            if (pos.x < MinBounds.x - BoundsMargin)
                pos.x = MaxBounds.x + BoundsMargin;
            else if (pos.x > MaxBounds.x + BoundsMargin)
                pos.x = MinBounds.x - BoundsMargin;

            if (pos.y < MinBounds.y - BoundsMargin)
                pos.y = MaxBounds.y + BoundsMargin;
            else if (pos.y > MaxBounds.y + BoundsMargin)
                pos.y = MinBounds.y - BoundsMargin;

            obj.position = pos;
        }

    }
}