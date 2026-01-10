using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies
{
    [Serializable]
    public class AsteroidTypeConfig : EnemyTypeConfig
    {
        [field: SerializeField] public AsteroidFragmentTypeSpawnConfig AsteroidFragmentSpawnConfig { get; private set; }

        public AsteroidTypeConfig(AddressableId prefabId, EnemyType type,
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig, 
            AsteroidFragmentTypeSpawnConfig asteroidFragmentSpawnConfig) : 
            base(prefabId, type, directionProviderConfig, rotationProviderConfig)
        {
            AsteroidFragmentSpawnConfig = asteroidFragmentSpawnConfig;
        }
        
        public AsteroidTypeConfig() : 
            base(AddressableId.Asteroid, EnemyType.Asteroid, 
                new LinearDirectionProviderConfig(2, 5), new MovementBasedRotationProviderConfig())
        {
            AsteroidFragmentSpawnConfig = new AsteroidFragmentTypeSpawnConfig();
        }
    }
}