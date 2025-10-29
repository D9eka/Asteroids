using _Project.Scripts.Weapons.Types.BulletGun;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/UfoTypeConfig", fileName = "UfoTypeConfig")]
    public class UfoTypeConfig : EnemyTypeConfig
    {
        [field: SerializeField] public BulletGunConfig BulletGunConfig  { get; private set; }
    }
}