using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Spawning.Enemies.Core;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class AsteroidFragment : MonoBehaviour, IEnemy
    {
        public event Action<DamageInfo, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        
        private IEnemyFactory _fragmentsFactory;
        
        public Transform Transform => transform;
        public bool Enabled => this != null && gameObject.activeSelf;
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

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnKilled?.Invoke(damageInfo, this);
        }
    }
}