using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Core;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class AsteroidFragment : MonoBehaviour, IEnemy
    {
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        
        private IEnemyFactory _fragmentsFactory;
        private AsteroidTypeConfig _config;
        
        public Transform Transform => transform;

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnDespawned();
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }
    }
}