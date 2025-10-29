using _Project.Scripts.Movement.DirectionProviders.Config;
using _Project.Scripts.Movement.RotationProviders.Config;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/EnemyTypeConfig", fileName = "EnemyTypeConfig")]
    public class EnemyTypeConfig : ScriptableObject
    {
        [Header("Base")]
        [field:SerializeField] public GameObject Prefab { get; private set; }

        [Header("Spawn Parameters")]
        [field:SerializeField] public float SpawnProbability { get; private set; }
        [field:SerializeField] public float SpawnInterval { get; private set; }

        [Header("Movement")]
        [field: SerializeField] public DirectionProviderConfig DirectionProviderConfig { get; private set; }
        [field: SerializeField] public RotationProviderConfig RotationProviderConfig { get; private set; }

        [Header("Spawn Offset")]
        [field:SerializeField] public float SpawnDistanceOutsideBounds { get; private set; }

        [Header("Pooling")]
        [field:SerializeField] public int PoolSize { get; private set; }
    }
}