using UnityEngine;

namespace Asteroids.Scripts.Audio.Sounds.Weapon
{
    [CreateAssetMenu(menuName = "Asteroids/WeaponAudioData", fileName = "WeaponAudioData")]
    public class WeaponAudioData : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] BulletGunShoutSounds { get; private set; }
        [field: SerializeField] public AudioClip[] LaserGunShoutSounds { get; private set; }
    }
}