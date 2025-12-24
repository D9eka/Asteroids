using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Weapons.LaserGun
{
    [CreateAssetMenu(fileName = "LaserGunConfig", menuName = "Weapons/LaserGunConfig")]
    public class LaserGunConfigSo : WeaponConfigSo
    {
        [field: SerializeField] public float LaserDuration { get; private set; }
        [field: SerializeField] public float RechargeRate { get; private set; }
        [field: SerializeField] public int MaxCharges { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; }
    }
}