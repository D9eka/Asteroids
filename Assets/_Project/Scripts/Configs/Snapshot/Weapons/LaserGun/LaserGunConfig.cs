using System;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun
{
    [Serializable]
    public class LaserGunConfig : WeaponConfig
    {
        [field: SerializeField] public float LaserDuration { get; private set; }
        [field: SerializeField] public float RechargeRate { get; private set; }
        [field: SerializeField] public int MaxCharges { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; }

        public LaserGunConfig(DamageType damageType, float fireRate, float laserDuration, float rechargeRate, 
            int maxCharges, float maxDistance) : base(damageType, fireRate)
        {
            LaserDuration = laserDuration;
            RechargeRate = rechargeRate;
            MaxCharges = maxCharges;
            MaxDistance = maxDistance;
        }
    }
}