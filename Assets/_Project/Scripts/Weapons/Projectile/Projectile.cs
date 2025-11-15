using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CollisionHandler), typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour, IProjectile, IDamageable
    {
        private float _speed;
        private float _lifeTime;
        private bool _isEnabled;
        private DamageInfo _damageInfo;
        
        private Rigidbody2D _rb;
        private CollisionHandler _collisionHandler;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collisionHandler = GetComponent<CollisionHandler>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_isEnabled) return;
            
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
                OnDespawned();
        }

        public void Initialize(ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService)
        {
            _speed = data.Speed;
            _lifeTime = data.LifeTime;
            _spriteRenderer.sprite = data.Sprite;
            _damageInfo = damageInfo;
            _collisionHandler.Initialize(collisionService);
            
            _rb.linearVelocity = transform.up * _speed;
            gameObject.SetActive(true);
            _isEnabled = true;
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnDespawned();
        }

        public DamageInfo GetDamageInfo()
        {
            return _damageInfo;
        }

        public void Pause()
        {
            _isEnabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }

        public void Resume()
        {
            _rb.linearVelocity = transform.up * _speed;
            _isEnabled = true;
        }
    }
}