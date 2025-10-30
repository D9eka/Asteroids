using System;
using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Fragments;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class Asteroid : MonoBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;

        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        private IAsteroidFragmentFactory _fragmentsFactory;
        private AsteroidFragmentTypeSpawnConfig _fragmentSpawnConfig;
        
        public Transform Transform => transform;
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
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Type == DamageType.Bullet)
            {
                SpawnFragments();
                OnKilled?.Invoke(damageInfo.Instigator, this);
            }
        }

        private void SpawnFragments()
        {
            _fragmentsFactory.SpawnFragments(transform.position, Movement.Velocity, _fragmentSpawnConfig);
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }
    }
}