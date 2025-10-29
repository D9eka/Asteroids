using UnityEngine;

namespace _Project.Scripts.Movement.RotationProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/RotationProviderConfig", fileName = "RotationProviderConfig")]
    public class RotationProviderConfig : ScriptableObject
    {
        [Header("ProviderType")]
        [field: SerializeField] public RotationProviderType RotationProviderType { get; protected set; }
    }
}