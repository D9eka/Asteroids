using System.Collections;
using Asteroids.Scripts.Audio.Sounds;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;

namespace Asteroids.Scripts.Effects.Explosion
{
    [RequireComponent(typeof(AudioSource))]
    public class ExplosionEffect : MonoBehaviour, IPoolable
    {
        private ExplosionEffectPool _pool;
        private AudioSource _audioSource;
        private ParticleSystem[] _particleSystems;
        
        public bool Enabled => gameObject.activeSelf;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        public void Initialize(ExplosionEffectPool pool, AudioClip audioClip)
        {
            _pool = pool;
            
            _audioSource.clip = audioClip;
            _audioSource.loop = false;
            _audioSource.Play();
            
            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Play();
            }
            StartCoroutine(ReturnWhenFinished());
        }

        private IEnumerator ReturnWhenFinished()
        {
            bool anyPlaying = true;
    
            while (anyPlaying)
            {
                anyPlaying = false;
                
                if (_audioSource.isPlaying)
                {
                    anyPlaying = true;
                    yield return null;
                }
                
                foreach (ParticleSystem ps in _particleSystems)
                {
                    if (ps.isPlaying)
                    {
                        anyPlaying = true;
                        break;
                    }
                }
                
                yield return null;
            }

            _pool.Despawn(this);
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }
        
        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
    }
}