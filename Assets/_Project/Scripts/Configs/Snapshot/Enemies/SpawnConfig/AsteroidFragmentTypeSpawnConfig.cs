using System;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig
{
    [Serializable]
    public class AsteroidFragmentTypeSpawnConfig : EnemyTypeSpawnConfig
    {
        [field: SerializeField] public int MinFragments { get; private set; }
        [field: SerializeField] public int MaxFragments { get; private set; }
        [field: SerializeField] public float FragmentPositionOffsetModifier { get; private set; }
        [field: SerializeField] public float FragmentSpeedMultiplier { get; private set; }

        public AsteroidFragmentTypeSpawnConfig(EnemyTypeConfig config, float spawnProbability, float spawnInterval, 
            float spawnDistanceOutsideBounds, int poolSize, int minFragments, int maxFragments, 
            float fragmentPositionOffsetModifier, float fragmentSpeedMultiplier) : 
            base(config, spawnProbability, spawnInterval, spawnDistanceOutsideBounds, poolSize)
        {
            MinFragments = minFragments;
            MaxFragments = maxFragments;
            FragmentPositionOffsetModifier = fragmentPositionOffsetModifier;
            FragmentSpeedMultiplier = fragmentSpeedMultiplier;
        }
    }
}