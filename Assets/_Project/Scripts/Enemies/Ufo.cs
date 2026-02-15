using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class Ufo : MonoBehaviour, IEnemy
    {
        public event Action<DamageInfo, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public BulletGun BulletGun { get; private set; }

        private EcsPool<WantsToShootTag> _wantToShootTagsPool;
        private int _bulletGunEntity;
        
        public Transform Transform => transform;
        public bool Enabled => this != null && gameObject.activeSelf;
        public bool Initialized { get; set; }
        public EnemyType Type { get; private set; }
        public int Id { get; private set; }

        public void Initialize(EcsWorld ecsWorld, int bulletGunEntity)
        {
            _wantToShootTagsPool = ecsWorld.GetPool<WantsToShootTag>();
            _bulletGunEntity = bulletGunEntity;
            if (!_wantToShootTagsPool.Has(_bulletGunEntity))
            {
                _wantToShootTagsPool.Add(_bulletGunEntity);
            }
        }

        public void SetType(EnemyType type)
        {
            Type = type;
        }
        public void SetId(int id)
        {
            Id = id;
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
            if (_wantToShootTagsPool != null && !_wantToShootTagsPool.Has(_bulletGunEntity))
            {
                _wantToShootTagsPool.Add(_bulletGunEntity);
            }
        }

        public void OnDespawned()
        {
            _wantToShootTagsPool.Del(_bulletGunEntity);
            gameObject.SetActive(false);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnKilled?.Invoke(damageInfo, this);
        }
    }
}