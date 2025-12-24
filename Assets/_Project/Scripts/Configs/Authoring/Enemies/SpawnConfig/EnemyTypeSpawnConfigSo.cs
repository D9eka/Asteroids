using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig
{
    [CreateAssetMenu(menuName = "Configs/Spawn/Enemy/EnemyTypeSpawnConfig", fileName = "EnemyTypeSpawnConfig")]
    public class EnemyTypeSpawnConfigSo : ScriptableObject
    {
        [Header("Base")] 
        [field: SerializeField] public EnemyTypeConfigSo ConfigSo { get; private set; }

        [Header("Spawn Parameters")]
        [field:SerializeField] public float SpawnProbability { get; private set; }
        [field:SerializeField] public float SpawnInterval { get; private set; }

        [Header("Spawn Offset")]
        [field:SerializeField] public float SpawnDistanceOutsideBounds { get; private set; }

        [Header("Pooling")]
        [field:SerializeField] public int PoolSize { get; private set; }
    }
}