using _Project.Scripts.Collision;
using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Projectile;
using UnityEngine;

namespace _Project.Scripts.Weapons.Types.BulletGun
{
    public class BulletGun : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _firePoint;
        
        private ICollisionService _collisionService;
        private BulletGunConfig _config;
        private IProjectileFactory _projectileFactory;
        private float _cooldown;

        public bool CanShoot => _cooldown <= 0f;
        
        public void Initialize(ICollisionService collisionService, BulletGunConfig config, IProjectileFactory projectileFactory)
        {
            _collisionService = collisionService;
            _config = config;
            _projectileFactory = projectileFactory;
        }

        public void Shoot()
        {
            if (!CanShoot) return;
            _projectileFactory.Create(_firePoint.position, _firePoint.rotation, _config.ProjectileData, _collisionService);
            _cooldown = _config.FireRate;
        }

        public void Recharge(float deltaTime)
        {
            if (_cooldown > 0)
                _cooldown -= deltaTime;
        }
    }
}