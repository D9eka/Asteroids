using System;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Weapons
{
    [Serializable]
    public class WeaponConfig
    {
        [field: SerializeField] public DamageType DamageType { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }

        public WeaponConfig(DamageType damageType, float fireRate)
        {
            DamageType = damageType;
            FireRate = fireRate;
        }
    }
}