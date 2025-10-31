using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Core
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField] public DamageType DamageType { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
    }
}