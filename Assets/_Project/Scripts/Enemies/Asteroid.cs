using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class Asteroid : MonoBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;

        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        private IAsteroidFragmentFactory _fragmentsFactory;
        private AsteroidFragmentTypeSpawnConfig _fragmentSpawnConfig;
        
        public Transform Transform => transform;
        public bool Initialized { get; set; }
        public EnemyType Type { get; private set; }
        
        public void SetType(EnemyType type)
        {
            Type = type;
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void Initialize(IAsteroidFragmentFactory fragmentsFactory, AsteroidTypeConfig asteroidTypeConfig)
        {
            _fragmentsFactory = fragmentsFactory;
            _fragmentSpawnConfig = asteroidTypeConfig.AsteroidFragmentSpawnConfig;
            Initialized = true;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Type == DamageType.Bullet)
            {
                SpawnFragments(damageInfo.Instigator);
            }
            OnKilled?.Invoke(damageInfo.Instigator, this);
        }

        private void SpawnFragments(GameObject damageInstigator)
        {
            Vector2 hitDirection = (transform.position - damageInstigator.transform.position).normalized;
            _fragmentsFactory.SpawnFragments(transform.position, hitDirection, Movement.Velocity, _fragmentSpawnConfig);
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }

        public void Pause()
        {
            Movement.Pause();
        }

        public void Resume()
        {
            Movement.Resume();
        }
    }
}