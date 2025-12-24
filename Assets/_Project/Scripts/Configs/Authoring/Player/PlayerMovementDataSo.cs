using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Player
{
    [CreateAssetMenu(menuName = "Configs/Player/MovementData", fileName = "MovementData")]
    public class PlayerMovementDataSo : ScriptableObject
    {
        [field: SerializeField] public float ThrustForce { get; private set; } = 10f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 400f;
    }
}