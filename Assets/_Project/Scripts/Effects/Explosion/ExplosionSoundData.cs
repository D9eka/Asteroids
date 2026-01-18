using UnityEngine;

namespace Asteroids.Scripts.Effects.Explosion
{
    [CreateAssetMenu(menuName = "Asteroids/ExplosionSoundData", fileName = "ExplosionSoundData")]
    public class ExplosionSoundData : ScriptableObject
    {
        [field:SerializeField] public AudioClip[] ExplosionSounds { get; private set; }
    }
}