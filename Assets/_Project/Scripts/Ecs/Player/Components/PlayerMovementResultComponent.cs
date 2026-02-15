using UnityEngine;

namespace Asteroids.Scripts.Ecs.Player.Components
{
    public struct PlayerMovementResultComponent
    {
        public Vector2 Force;
        public float RotationDelta;
    }
}