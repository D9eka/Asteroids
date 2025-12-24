using Asteroids.Scripts.Movement.RotationProviders;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Movement.Rotation
{
    [CreateAssetMenu(menuName = "Configs/Movement/Rotation/RotationProviderConfig", fileName = "RotationProviderConfig")]
    public class RotationProviderConfigSo : ScriptableObject
    {
        [Header("ProviderType")]
        [field: SerializeField] public RotationProviderType RotationProviderType { get; protected set; }
    }
}