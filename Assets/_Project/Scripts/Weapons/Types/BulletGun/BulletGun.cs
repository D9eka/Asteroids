using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.BulletGun
{
    public class BulletGun : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _firePoint;
        
        private ICollisionService _collisionService;
        private BulletGunConfig _config;
        private IProjectileFactory _projectileFactory;
        private DamageInfo _damageInfo;
        private float _cooldown;

        public bool CanShoot => _cooldown <= 0f;
        
        public void Initialize(GameObject damageInstigator,
            ICollisionService collisionService, BulletGunConfig config, IProjectileFactory projectileFactory)
        {
            _collisionService = collisionService;
            _config = config;
            _projectileFactory = projectileFactory;
            
            _damageInfo = new DamageInfo(_config.DamageType, damageInstigator);
        }

        public void Shoot()
        {
            if (!CanShoot || _firePoint == null) return;
            _projectileFactory.Create(_firePoint.position, _firePoint.rotation,
                _config.ProjectileData, _damageInfo, _collisionService);
            _cooldown = _config.FireRate;
        }

        public void Recharge(float deltaTime)
        {
            if (_cooldown > 0)
                _cooldown -= deltaTime;
        }

        public DamageInfo GetDamageInfo()
        {
            return _damageInfo;
        }
    }
}