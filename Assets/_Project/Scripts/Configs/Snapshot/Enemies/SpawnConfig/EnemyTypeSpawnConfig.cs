using System;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig
{
    [Serializable]
    public class EnemyTypeSpawnConfig
    {
        [field:SerializeField] public EnemyTypeConfig Config { get; private set; }

        [field:SerializeField] public float SpawnProbability { get; private set; }
        [field:SerializeField] public float SpawnInterval { get; private set; }

        [field:SerializeField] public float SpawnDistanceOutsideBounds { get; private set; }

        [field:SerializeField] public int PoolSize { get; private set; }

        public EnemyTypeSpawnConfig(EnemyTypeConfig config, float spawnProbability, float spawnInterval, 
            float spawnDistanceOutsideBounds, int poolSize)
        {
            Config = config;
            SpawnProbability = spawnProbability;
            SpawnInterval = spawnInterval;
            SpawnDistanceOutsideBounds = spawnDistanceOutsideBounds;
            PoolSize = poolSize;
        }
    }
}