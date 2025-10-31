using UnityEngine;

namespace Asteroids.Scripts.Movement.RotationProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/Movement/Rotation/TargetBasedRotationProviderConfig", 
        fileName = "TargetBasedRotationProviderConfig")]
    public class TargetBasedRotationProviderConfig : RotationProviderConfig
    {
        [field: SerializeField] public float RotationSpeed { get; private set; }
        
        public TargetBasedRotationProviderConfig()
        {
            RotationProviderType = RotationProviderType.TargetBased;
        }
    }
}