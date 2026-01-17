using UnityEngine;

namespace Asteroids.Scripts.Audio.Sounds
{
    public class AudioSoundFactory
    {
        private AudioSoundPool _pool;

        public void Initialize(AudioSoundPool pool)
        {
            _pool = pool;
        }
        
        public void Create(Vector3 position, AudioClip clip)
        {
            _pool.Spawn(position, clip);
        }
    }
}