using UnityEngine;

namespace _Project.Scripts.WarpSystem
{
    public interface IBoundsWarp
    {
        Vector2 MinBounds { get; }
        Vector2 MaxBounds { get; }
        
        void WarpObject(Transform obj);
    }
}