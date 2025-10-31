using Asteroids.Scripts.Weapons.Core;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.Laser
{
    [CreateAssetMenu(fileName = "LaserGunConfig", menuName = "Weapons/LaserGunConfig")]
    public class LaserGunConfig : WeaponConfig
    {
        [field: SerializeField] public float LaserDuration { get; private set; }
        [field: SerializeField] public float RechargeRate { get; private set; }
        [field: SerializeField] public int MaxCharges { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; }
    }
}