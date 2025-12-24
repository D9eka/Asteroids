using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Movement.Rotation
{
    public class TargetBasedRotationProviderConfig : RotationProviderConfig
    {
        [field: SerializeField] public float RotationSpeed { get; private set; }

        public TargetBasedRotationProviderConfig(float rotationSpeed)
        {
            RotationSpeed = rotationSpeed;
        }
    }
}