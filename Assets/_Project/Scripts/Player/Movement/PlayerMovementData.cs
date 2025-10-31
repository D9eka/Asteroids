using UnityEngine;

namespace Asteroids.Scripts.Player.Movement
{
    [CreateAssetMenu(menuName = "Configs/Player/MovementData", fileName = "MovementData")]
    public class PlayerMovementData : ScriptableObject
    {
        [field: SerializeField] public float ThrustForce { get; private set; } = 10f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 400f;
    }
}