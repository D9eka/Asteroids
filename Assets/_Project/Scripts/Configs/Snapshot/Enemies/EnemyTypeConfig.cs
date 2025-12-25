using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies
{
    [Serializable]
    public class EnemyTypeConfig
    {
        [field:SerializeField] public AddressableId PrefabId { get; private set; }
        [field:SerializeField] public EnemyType Type { get; private set; }
        [field:SerializeField] public int Score { get; private set; }

        [field: SerializeReference] public DirectionProviderConfig DirectionProviderConfig { get; private set; }
        [field: SerializeReference] public RotationProviderConfig RotationProviderConfig { get; private set; }

        public EnemyTypeConfig(AddressableId prefabId, EnemyType type, int score, 
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig)
        {
            PrefabId = prefabId;
            Type = type;
            Score = score;
            DirectionProviderConfig = directionProviderConfig;
            RotationProviderConfig = rotationProviderConfig;
        }
    }
}