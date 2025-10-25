using UnityEngine;

namespace _Project.Scripts.Camera
{
    public interface ICameraBoundsUpdater
    {
        Vector2 MinBounds { get; }
        Vector2 MaxBounds { get; }
    }
}