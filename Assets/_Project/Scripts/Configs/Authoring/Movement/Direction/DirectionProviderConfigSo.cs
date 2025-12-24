using Asteroids.Scripts.Movement.DirectionProviders;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Movement.Direction
{
    [CreateAssetMenu(menuName = "Configs/Movement/Direction/DirectionProviderConfig", fileName = "DirectionProviderConfig")]
    public class DirectionProviderConfigSo : ScriptableObject
    {
        [Header("Velocity")]
        [field: SerializeField] public float MinSpeed { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        
        [Header("ProviderType")]
        [field: SerializeField] public DirectionProviderType DirectionProviderType { get; protected set; }
    }
}