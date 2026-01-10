using System;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Player
{
    [Serializable]
    public class PlayerConfig
    {
        [field:SerializeField] public PlayerMovementConfig MovementConfig { get; private set; }
        [field:SerializeField] public BulletGunConfig BulletGunConfig { get; private set; }
        [field:SerializeField] public LaserGunConfig LaserGunConfig { get; private set; }

        public PlayerConfig(PlayerMovementConfig movementConfig, BulletGunConfig bulletGunConfig, LaserGunConfig laserGunConfig)
        {
            MovementConfig = movementConfig;
            BulletGunConfig = bulletGunConfig;
            LaserGunConfig = laserGunConfig;
        }

        public PlayerConfig()
        {
            MovementConfig = new PlayerMovementConfig();
            BulletGunConfig = new BulletGunConfig(DamageType.Bullet, 0.35f, new ProjectileConfig(7f, 5f));
            LaserGunConfig = new LaserGunConfig(DamageType.Laser, 0.3f, 0.5f, 1f, 3, 15f);
        }
    }
}