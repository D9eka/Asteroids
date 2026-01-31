using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Spawning.Enemies.Fragments;
using Asteroids.Scripts.WarpSystem;
using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class Asteroid : NetworkBehaviour, IEnemy, IWarpable
    {
        public event Action<GameObject, IEnemy> OnKilled;

        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        
        private IAsteroidFragmentFactory _fragmentsFactory;
        private AsteroidFragmentTypeSpawnConfig _fragmentSpawnConfig;
        
        [Networked] private Vector2 NetPosition { get; set; }
        [Networked] private float NetRotation { get; set; }
        
        public Transform Transform => transform;
        public bool Enabled => gameObject.activeSelf;
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
        
        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            NetPosition = transform.position;
            NetRotation = transform.rotation.eulerAngles.z;
        }

        public override void Render()
        {
            if (Object.HasStateAuthority)
                return;

            transform.SetPositionAndRotation(NetPosition, Quaternion.Euler(0f, 0f, NetRotation));
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (!Object.HasStateAuthority)
                return;
            
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