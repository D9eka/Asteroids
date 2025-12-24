using System;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
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
    }
}