using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
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
        
        public AsteroidFragmentTypeSpawnConfig() : base(
            new EnemyTypeConfig(AddressableId.AsteroidFragment, EnemyType.AsteroidFragment, 
                new LinearDirectionProviderConfig(2, 5), new MovementBasedRotationProviderConfig()), 
            0.9f, 2f, 2f, 10)
        {
            MinFragments = 2;
            MaxFragments = 4;
            FragmentPositionOffsetModifier = 0.25f;
            FragmentSpeedMultiplier = 2f;
        }
    }
}