using Asteroids.Scripts.Enemies.Config;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/Spawn/Enemy/EnemyTypeSpawnConfig", fileName = "EnemyTypeSpawnConfig")]
    public class EnemyTypeSpawnConfig : ScriptableObject
    {
        [Header("Base")] 
        [field: SerializeField] public EnemyTypeConfig Config { get; private set; }

        [Header("Spawn Parameters")]
        [field:SerializeField] public float SpawnProbability { get; private set; }
        [field:SerializeField] public float SpawnInterval { get; private set; }

        [Header("Spawn Offset")]
        [field:SerializeField] public float SpawnDistanceOutsideBounds { get; private set; }

        [Header("Pooling")]
        [field:SerializeField] public int PoolSize { get; private set; }
    }
}