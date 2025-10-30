using _Project.Scripts.Movement.DirectionProviders.Config;
using _Project.Scripts.Movement.RotationProviders.Config;
using UnityEngine;

namespace _Project.Scripts.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/Enemy/EnemyTypeSpawnConfig", fileName = "EnemyTypeSpawnConfig")]
    public class EnemyTypeConfig : ScriptableObject
    {
        [Header("Base")]
        [field:SerializeField] public GameObject Prefab { get; private set; }
        [field:SerializeField] public EnemyType Type { get; private set; }
        [field:SerializeField] public int Score { get; private set; }

        [Header("Movement")]
        [field: SerializeField] public DirectionProviderConfig DirectionProviderConfig { get; private set; }
        [field: SerializeField] public RotationProviderConfig RotationProviderConfig { get; private set; }
    }
}