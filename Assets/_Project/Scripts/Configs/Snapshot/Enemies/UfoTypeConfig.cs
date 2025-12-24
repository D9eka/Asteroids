using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies
{
    [Serializable]
    public class UfoTypeConfig : EnemyTypeConfig
    {
        [field: SerializeField] public BulletGunConfig BulletGunConfig  { get; private set; }

        public UfoTypeConfig(AddressableId prefabId, EnemyType type, int score, 
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig, 
            BulletGunConfig bulletGunConfig) : base(prefabId, type, score, directionProviderConfig, rotationProviderConfig)
        {
            BulletGunConfig = bulletGunConfig;
        }
    }
}