using UnityEngine;

namespace Asteroids.Scripts.Damage
{
    public class DamageInfo
    {
        public DamageType Type { get; }
        public GameObject Instigator { get; }

        public DamageInfo(DamageType type, GameObject instigator = null)
        {
            Type = type;
            Instigator = instigator;
        }
    }
}