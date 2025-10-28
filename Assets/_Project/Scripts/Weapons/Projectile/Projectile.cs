using System;
using _Project.Scripts.Collision;
using _Project.Scripts.Core;
using _Project.Scripts.Damage;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapons.Projectile
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CollisionHandler), typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour, IProjectile, IDamageable
    {
        private float _speed;
        private float _lifeTime;
        private float _timeAlive;
        private DamageType _damageType;
        
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
            _timeAlive += Time.deltaTime;
            if (_timeAlive >= _lifeTime)
                OnDespawned();
        }

        public void Initialize(ProjectileData data, DamageType damageType, ICollisionService collisionService)
        {
            _speed = data.Speed;
            _lifeTime = data.LifeTime;
            _spriteRenderer.sprite = data.Sprite;
            _damageType = damageType;
            _collisionHandler.Initialize(collisionService);
            
            _rb.linearVelocity = transform.up * _speed;
            _timeAlive = 0f;
            gameObject.SetActive(true);
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnDespawned();
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(_damageType, gameObject);
        }
    }
}