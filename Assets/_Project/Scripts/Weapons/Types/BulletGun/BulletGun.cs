using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.BulletGun
{
    public class BulletGun : MonoBehaviour, IWeapon
    {
        public event Action<IWeapon> OnShoot;
        
        [SerializeField] private Transform _firePoint;

        private ICollisionService _collisionService;
        private BulletGunConfig _config;
        private IProjectileFactory _projectileFactory;
        private DamageInfo _damageInfo;
        
        public Transform Transform => transform;
        
        public void Initialize(IEcsEntity instigatorEntity,
            ICollisionService collisionService, BulletGunConfig config, IProjectileFactory projectileFactory)
        {
            _collisionService = collisionService;
            _config = config;
            _projectileFactory = projectileFactory;
            
            _damageInfo = new DamageInfo(_config.DamageType, instigatorEntity);
        }

        public void Shoot()
        {
            _projectileFactory.Create(_firePoint.position, _firePoint.rotation,
                _config.ProjectileConfig, _damageInfo, _collisionService);
            OnShoot?.Invoke(this);
        }

        public void ApplyConfig(BulletGunConfig config)
        {
            _config = config;
        }
    }
}