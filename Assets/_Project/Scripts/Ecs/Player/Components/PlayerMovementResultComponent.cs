using UnityEngine;

namespace Asteroids.Scripts.Ecs.Components
{
    public struct PlayerMovementResultComponent
    {
        public Vector2 Force;
        public float RotationDelta;
    }
}