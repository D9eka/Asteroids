using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/DirectionProviderConfig", fileName = "DirectionProviderConfig")]
    public class DirectionProviderConfig : ScriptableObject
    {
        [Header("Speed")]
        [field: SerializeField] public float MinSpeed { get; set; }
        [field: SerializeField] public float MaxSpeed { get; set; }
        
        [Header("ProviderType")]
        [field: SerializeField] public DirectionProviderType DirectionProviderType { get; set; }
    }
}