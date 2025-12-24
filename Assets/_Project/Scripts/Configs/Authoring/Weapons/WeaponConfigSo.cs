using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Weapons
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/WeaponConfig")]
    public class WeaponConfigSo : ScriptableObject
    {
        [field: SerializeField] public DamageType DamageType { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
    }
}