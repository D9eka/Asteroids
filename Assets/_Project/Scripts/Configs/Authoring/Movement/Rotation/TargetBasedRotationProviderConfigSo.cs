using Asteroids.Scripts.Movement.RotationProviders;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Movement.Rotation
{
    [CreateAssetMenu(menuName = "Configs/Movement/Rotation/TargetBasedRotationProviderConfig", 
        fileName = "TargetBasedRotationProviderConfig")]
    public class TargetBasedRotationProviderConfigSo : RotationProviderConfigSo
    {
        [field: SerializeField] public float RotationSpeed { get; private set; }
        
        public TargetBasedRotationProviderConfigSo()
        {
            RotationProviderType = Scripts.Movement.RotationProviders.RotationProviderType.TargetBased;
        }
    }
}