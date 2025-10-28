using UnityEngine;

namespace _Project.Scripts.Weapons.Core
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField] public float FireRate { get; private set; }
    }
}