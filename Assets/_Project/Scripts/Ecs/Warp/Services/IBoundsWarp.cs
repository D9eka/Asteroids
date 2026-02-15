using UnityEngine;

namespace Asteroids.Scripts.Ecs.Warp.Services
{
    public interface IBoundsWarp
    {
        Vector2 MinBounds { get; }
        Vector2 MaxBounds { get; }
        float BoundsMargin { get; }
        
        void WarpObject(Transform obj);
    }
}