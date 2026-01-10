using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies
{
    [Serializable]
    public class UfoTypeConfig : EnemyTypeConfig
    {
        [field: SerializeField] public BulletGunConfig BulletGunConfig  { get; private set; }

        public UfoTypeConfig(AddressableId prefabId, EnemyType type, 
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig, 
            BulletGunConfig bulletGunConfig) : base(prefabId, type, directionProviderConfig, rotationProviderConfig)
        {
            BulletGunConfig = bulletGunConfig;
        }

        public UfoTypeConfig() : base(
            AddressableId.Ufo, EnemyType.Ufo, new TargetDirectionProviderConfig(0.5f, 1f, 2f),  new TargetBasedRotationProviderConfig(2f))
        {
            BulletGunConfig = new BulletGunConfig(DamageType.Bullet, 0.5f, new ProjectileConfig(7f, 5f));
        }
    }
}