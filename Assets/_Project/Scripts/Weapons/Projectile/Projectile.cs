using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Fusion;
using Unity.Collections;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CollisionHandler))]
    public class Projectile : NetworkBehaviour, IProjectile, IDamageable
    {
        private float _speed;
        private float _lifeTime;
        private bool _isEnabled;
        private DamageInfo _damageInfo;
        private IPauseSystem _pauseSystem;
        
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

        public void SetPauseSystem(IPauseSystem pauseSystem)
        {
            _pauseSystem = pauseSystem;
        }

        private void Update()
        {
            if (!_isEnabled || !Object.HasStateAuthority) return;
            
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
                Runner.Despawn(Object);
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

        public void Initialize(ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService)
        {
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
            if (!Object.HasStateAuthority)
                return;

            Runner.Despawn(Object);
        }

        public DamageInfo GetDamageInfo()
        {
            return _damageInfo;
        }

        public void Pause()
        {
            _isEnabled = false;
            if (_rb == null)
                return;
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }

        public void Resume()
        {
            if (_rb == null)
                return;
            _rb.linearVelocity = transform.up * _speed;
            _isEnabled = true;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _pauseSystem?.Unregister(this);
            _isEnabled = false;
        }
    }
}
