using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.Laser
{
    public class LaserGun : MonoBehaviour, ILaserGun
    {
        public event Action<IWeapon> OnShoot;
        
        [SerializeField] private Transform _laserStartPoint;
        
        private LaserGunConfig _config;
        private ILineRenderer _lineRenderer;
        private IRaycastService _raycastService;
        private ICollisionService _collisionService;
        private DamageInfo _damageInfo;
        private float _chargesCooldown;
        private float _laserTime;
        private bool _isShooting;
        
        public int CurrentCharges { get; private set; }
        public float ShootCooldown { get; private set; }

        public Transform Transform => transform;
        public bool CanShoot => CurrentCharges > 0 && ShootCooldown <= 0 && !_isShooting;
        
        public void Initialize(GameObject damageInstigator, LaserGunConfig config, ILineRenderer lineRenderer, 
            IRaycastService raycastService, ICollisionService collisionService)
        {
            _config = config;
            _lineRenderer = lineRenderer;
            _raycastService = raycastService;
            _collisionService = collisionService;
            
            _damageInfo = new DamageInfo(_config.DamageType, damageInstigator);
            CurrentCharges = _config.MaxCharges;
            _lineRenderer.Disable();
        }

        public void Shoot()
        {
            if (!CanShoot) return;
            CurrentCharges--;
            ShootCooldown = _config.FireRate;
            _chargesCooldown = _config.RechargeRate;
            _isShooting = true;
            _laserTime = _config.LaserDuration;
            _lineRenderer.Enable();
            OnShoot?.Invoke(this);
        }

        public void Recharge(float deltaTime)
        {
            if (_isShooting)
            {
                UpdateLaser(deltaTime);
                return;
            }
            if (ShootCooldown > 0) ShootCooldown -= deltaTime;
            if (CurrentCharges < _config.MaxCharges)
            {
                _chargesCooldown -= deltaTime;
                if (_chargesCooldown <= 0)
                {
                    CurrentCharges++;
                    _chargesCooldown = _config.RechargeRate;
                }
            }
        }
        
        public DamageInfo GetDamageInfo()
        {
            return _damageInfo;
        }

        public void ApplyConfig(LaserGunConfig config)
        {
            _config = config;
        }

        private void UpdateLaser(float deltaTime)
        {
            _laserTime -= deltaTime;
            Vector2 origin = _laserStartPoint.position;
            Vector2 direction = transform.up;
            Vector2 endPosition = origin + direction * _config.MaxDistance;

            if (_raycastService.TryRaycast(origin, direction, _config.MaxDistance, out RaycastHit2D hit))
            {
                endPosition = hit.transform.position;
                Debug.DrawRay(origin, direction * hit.distance, Color.red);
                _collisionService.OnHit(gameObject, hit.collider.gameObject);
            }

            _lineRenderer.UpdateLine(origin, endPosition);

            if (_laserTime <= 0)
            {
                _isShooting = false;
                _lineRenderer.Disable();
            }
        }

    }
}