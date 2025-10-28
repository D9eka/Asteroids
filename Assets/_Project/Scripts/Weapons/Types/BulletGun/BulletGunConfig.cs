using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Projectile;
using UnityEngine;

namespace _Project.Scripts.Weapons.Types.BulletGun
{
    [CreateAssetMenu(fileName = "BulletGunConfig", menuName = "Weapons/BulletGunConfig")]
    public class BulletGunConfig : WeaponConfig
    {
        [field: SerializeField] public ProjectileData ProjectileData { get; private set; }
    }
}