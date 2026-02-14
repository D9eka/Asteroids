using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.WarpSystem;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class Asteroid : MonoBehaviour, IEnemy, IWarpable
    {
        public event Action<IEcsEntity, IEnemy> OnKilled;

        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        private IAsteroidFragmentFactory _fragmentsFactory;
        private AsteroidFragmentTypeSpawnConfig _fragmentSpawnConfig;
        
        public Transform Transform => transform;
        public bool Enabled => gameObject.activeSelf;
        public bool Initialized { get; set; }
        public EnemyType Type { get; private set; }
        public int Id { get; private set; }
        
        public void SetType(EnemyType type)
        {
            Type = type;
        }
        
        public void SetId(int id)
        {
            Id = id;
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
                SpawnFragments(damageInfo.InstigatorEntity);
            }
            OnKilled?.Invoke(damageInfo.InstigatorEntity, this);
        }

        private void SpawnFragments(IEcsEntity instigatorEntity)
        {
            Vector2 hitDirection = (transform.position - instigatorEntity.Transform.position).normalized;
            _fragmentsFactory.SpawnFragments(transform.position, hitDirection, Id, _fragmentSpawnConfig);
        }
    }
}