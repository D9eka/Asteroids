using UnityEngine;

namespace Asteroids.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/Movement/Direction/DirectionProviderConfig", fileName = "DirectionProviderConfig")]
    public class DirectionProviderConfig : ScriptableObject
    {
        [Header("Velocity")]
        [field: SerializeField] public float MinSpeed { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        
        [Header("ProviderType")]
        [field: SerializeField] public DirectionProviderType DirectionProviderType { get; protected set; }
    }
}