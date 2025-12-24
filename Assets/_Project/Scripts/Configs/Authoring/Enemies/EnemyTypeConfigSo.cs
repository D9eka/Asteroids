using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Configs.Authoring.Movement.Direction;
using Asteroids.Scripts.Configs.Authoring.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Enemies
{
    [CreateAssetMenu(menuName = "Configs/Enemy/EnemyTypeSpawnConfig", fileName = "EnemyTypeSpawnConfig")]
    public class EnemyTypeConfigSo : ScriptableObject
    {
        [Header("Base")]
        [field:SerializeField] public AddressableId PrefabId { get; private set; }
        [field:SerializeField] public EnemyType Type { get; private set; }
        [field:SerializeField] public int Score { get; private set; }

        [Header("Movement")]
        [field: SerializeField] public DirectionProviderConfigSo DirectionProviderConfigSo { get; private set; }
        [field: SerializeField] public RotationProviderConfigSo RotationProviderConfigSo { get; private set; }
    }
}