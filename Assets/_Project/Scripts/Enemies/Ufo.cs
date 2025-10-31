using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class Ufo : MonoBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        [field: SerializeField] public BulletGun BulletGun { get; private set; }

        private bool _isPaused; 
        
        public Transform Transform => transform;
        public bool Initialized { get; set; }
        public EnemyType Type { get; private set; }

        private void Update()
        {
            if (!_isPaused && BulletGun.CanShoot)
            {
                BulletGun.Shoot();
            }
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