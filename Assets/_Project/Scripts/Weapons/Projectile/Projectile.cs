using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CollisionHandler))]
    public class Projectile : NetworkBehaviour, IProjectile, IDamageable
    {
        private ProjectilePool _pool;
        private float _speed;
        private float _lifeTime;
        private bool _isEnabled;
        private DamageInfo _damageInfo;
        
        private Rigidbody2D _rb;
        private CollisionHandler _collisionHandler;
        
        [Networked] private Vector2 NetPosition { get; set; }
        [Networked] private float NetRotation { get; set; }
        
        public bool Enabled => gameObject.activeSelf;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collisionHandler = GetComponent<CollisionHandler>();
        }

        private void Update()
        {
            if (!_isEnabled || !Object.HasStateAuthority) return;
            
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
                _pool.Despawn(this);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;
            
            NetPosition = transform.position;
            NetRotation = _rb.rotation;
        }

        public override void Render()
        {
            if (Object.HasStateAuthority)
                return;

            transform.SetPositionAndRotation(NetPosition, Quaternion.Euler(0f, 0f, NetRotation));
        }

        public void Initialize(ProjectilePool pool,
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService)
        {
            _pool = pool;
            _speed = config.Speed;
            _lifeTime = config.LifeTime;
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
            _pool.Despawn(this);
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