using UnityEngine;

namespace _Project.Scripts.Movement.RotationProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/TargetBasedRotationProviderConfig", 
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