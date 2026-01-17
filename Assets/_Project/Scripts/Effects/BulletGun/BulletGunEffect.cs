using System;
using System.Collections;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;

namespace _Project.Scripts.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BulletGunEffect : MonoBehaviour, IPoolable
    {
        private BulletGunEffectPool _pool;
        private ParticleSystem _particleSystem;

        public bool Enabled => gameObject.activeSelf;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Initialize(BulletGunEffectPool pool)
        {
            _pool = pool;
            StartCoroutine(ReturnWhenFinished());
        }

        private IEnumerator ReturnWhenFinished()
        {
            yield return new WaitWhile(() => _particleSystem.isPlaying);
        
            _pool.Despawn(this);
        }
        
        public void OnSpawned()
        {
            gameObject.SetActive(true);
            _particleSystem.Play();
        }
        
        public void OnDespawned()
        {
            _particleSystem.Stop();
            gameObject.SetActive(false);
        }
    }
}