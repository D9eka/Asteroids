using Asteroids.Scripts.Configs.Authoring.Weapons.Projectile;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Weapons.BulletGun
{
    [CreateAssetMenu(fileName = "BulletGunConfig", menuName = "Weapons/BulletGunConfig")]
    public class BulletGunConfigSo : WeaponConfigSo
    {
        [field: SerializeField] public ProjectileConfig ProjectileConfig { get; private set; }
    }
}