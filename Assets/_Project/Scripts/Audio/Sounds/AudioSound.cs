using System.Collections;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;

namespace Asteroids.Scripts.Audio.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSound : MonoBehaviour, IPoolable
    {
        private AudioSoundPool _pool;
        private AudioSource _audioSource;

        public bool Enabled => gameObject.activeSelf;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(AudioSoundPool pool, AudioClip clip)
        {
            _pool = pool;
            
            _audioSource.clip = clip;
            _audioSource.loop = false;
            _audioSource.Play();
            StartCoroutine(ReturnWhenFinished());
        }

        private IEnumerator ReturnWhenFinished()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
        
            _pool.Despawn(this);
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);
    }
}