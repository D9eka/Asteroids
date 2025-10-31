using Asteroids.Scripts.Weapons.Types.BulletGun;
using UnityEngine;

namespace Asteroids.Scripts.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/Enemy/UfoTypeConfig", fileName = "UfoTypeConfig")]
    public class UfoTypeConfig : EnemyTypeConfig
    {
        [field: SerializeField] public BulletGunConfig BulletGunConfig  { get; private set; }
    }
}