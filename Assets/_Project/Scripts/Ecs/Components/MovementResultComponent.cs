using UnityEngine;

namespace _Project.Scripts.Ecs.Components
{
    public struct MovementResultComponent
    {
        public Vector2 Force;
        public float RotationDelta;
    }
}