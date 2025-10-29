using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/DirectionProviderConfig", fileName = "DirectionProviderConfig")]
    public class DirectionProviderConfig : ScriptableObject
    {
        [Header("Velocity")]
        [field: SerializeField] public float MinSpeed { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        
        [Header("ProviderType")]
        [field: SerializeField] public DirectionProviderType DirectionProviderType { get; protected set; }
    }
}