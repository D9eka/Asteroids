using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Projectile : MonoBehaviour, IProjectile, IDamageable
    {
        private ProjectilePool _pool;
        
        private CollisionHandler _collisionHandler;
        
        public Transform Transform => transform;
        public int Id { get; private set; }
        public bool Enabled => this != null && gameObject.activeSelf;

        private void Awake()
        {
            _collisionHandler = GetComponent<CollisionHandler>();
        }

        public void Initialize(ProjectilePool pool, int id, ICollisionService collisionService)
        {
            _pool = pool;
            Id = id;
            _collisionHandler.Initialize(collisionService);
            
            gameObject.SetActive(true);
        }
        
        public void SetId(int id)
        {
            Id = id;
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            _pool.Despawn(this);
        }
    }
}