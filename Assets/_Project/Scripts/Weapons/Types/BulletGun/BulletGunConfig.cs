using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.BulletGun
{
    [CreateAssetMenu(fileName = "BulletGunConfig", menuName = "Weapons/BulletGunConfig")]
    public class BulletGunConfig : WeaponConfig
    {
        [field: SerializeField] public ProjectileData ProjectileData { get; private set; }
    }
}