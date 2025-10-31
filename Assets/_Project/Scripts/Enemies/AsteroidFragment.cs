using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Spawning.Enemies.Config;
using Asteroids.Scripts.Spawning.Enemies.Core;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class AsteroidFragment : MonoBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        
        private IEnemyFactory _fragmentsFactory;
        private AsteroidFragmentTypeSpawnConfig _spawnConfig;
        
        public Transform Transform => transform;
        public EnemyType Type { get; private set; }

        public void SetType(EnemyType type)
        {
            Type = type;
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnKilled?.Invoke(damageInfo.Instigator, this);
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