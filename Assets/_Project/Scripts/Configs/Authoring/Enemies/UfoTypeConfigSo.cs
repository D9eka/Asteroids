using Asteroids.Scripts.Configs.Authoring.Weapons.BulletGun;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Enemies
{
    [CreateAssetMenu(menuName = "Configs/Enemy/UfoTypeConfig", fileName = "UfoTypeConfig")]
    public class UfoTypeConfigSo : EnemyTypeConfigSo
    {
        [field: SerializeField] public BulletGunConfigSo BulletGunConfigSo  { get; private set; }
    }
}