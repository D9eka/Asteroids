using System;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun
{
    [Serializable]
    public class BulletGunConfig : WeaponConfig
    {
        [field: SerializeField] public ProjectileConfig ProjectileConfig { get; private set; }

        public BulletGunConfig(DamageType damageType, float fireRate, ProjectileConfig projectileConfig) : 
            base(damageType, fireRate)
        {
            ProjectileConfig = projectileConfig;
        }
    }
}