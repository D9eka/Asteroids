using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class Ufo : NetworkBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        [field: SerializeField] public BulletGun BulletGun { get; private set; }

        private bool _isPaused; 
        
        [Networked] private Vector2 NetPosition { get; set; }
        [Networked] private float NetRotation { get; set; }
        
        public Transform Transform => transform;
        public bool Enabled => gameObject.activeSelf;
        public bool Initialized { get; set; }
        public EnemyType Type { get; private set; }

        private void Update()
        {
            if (!_isPaused && BulletGun.CanShoot && Object.HasStateAuthority)
            {
                BulletGun.Shoot();
            }
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

        public void SetType(EnemyType type)
        {
            Type = type;
        }

        public void OnSpawned()
        {
            Resume();
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            Pause();
            gameObject.SetActive(false);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (!Object.HasStateAuthority)
                return;

            OnKilled?.Invoke(damageInfo.Instigator, this);
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }

        public void Pause()
        {
            _isPaused = true;
            Movement.Pause();
        }

        public void Resume()
        {
            _isPaused  = false;
            Movement.Resume();
        }
    }
}