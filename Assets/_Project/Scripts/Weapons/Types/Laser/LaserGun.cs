using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Services.Raycast;
using _Project.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;

namespace _Project.Scripts.Weapons.Types.Laser
{
    public class LaserGun : MonoBehaviour, IWeapon, IDamageSource
    {
        [SerializeField] private Transform _laserStartPoint;
        
        private LaserGunConfig _config;
        private ILineRenderer _lineRenderer;
        private IRaycastService _raycastService;
        private ICollisionService _collisionService;
        private DamageInfo _damageInfo;
        private int _currentCharges;
        private float _shootCooldown;
        private float _chargesCooldown;
        private float _laserTime;
        private bool _isShooting;

        public bool CanShoot => _currentCharges > 0 && _shootCooldown <= 0 && !_isShooting;
        
        public void Initialize(GameObject damageInstigator, LaserGunConfig config, ILineRenderer lineRenderer, 
            IRaycastService raycastService, ICollisionService collisionService)
        {
            _config = config;
            _lineRenderer = lineRenderer;
            _raycastService = raycastService;
            _collisionService = collisionService;
            
            _damageInfo = new DamageInfo(_config.DamageType, damageInstigator);
            _currentCharges = _config.MaxCharges;
            _lineRenderer.Disable();
        }
        
        public void Shoot()
        {
            if (!CanShoot) return;
            _currentCharges--;
            _shootCooldown = _config.FireRate;
            _chargesCooldown = _config.RechargeRate;
            _isShooting = true;
            _laserTime = _config.LaserDuration;
            _lineRenderer.Enable();
        }

        public void Recharge(float deltaTime)
        {
            if (_isShooting)
            {
                UpdateLaser(deltaTime);
            }
            else
            {
                _shootCooldown -= deltaTime;
                _chargesCooldown -= deltaTime;
                if (_chargesCooldown <= 0 && _currentCharges < _config.MaxCharges)
                {
                    _currentCharges++;
                    _chargesCooldown = _config.RechargeRate;
                }
            }
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

        public DamageInfo GetDamageInfo()
        {
            return _damageInfo;
        }
    }
}