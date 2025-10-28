using _Project.Scripts.Weapons.Core;
using _Project.Scripts.Weapons.Services.Raycast;
using _Project.Scripts.Weapons.Types.Laser.LineRenderer;
using UnityEngine;

namespace _Project.Scripts.Weapons.Types.Laser
{
    public class LaserGun : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _laserStartPoint;
        
        private LaserGunConfig _config;
        private ILineRenderer _lineRenderer;
        private IRaycastService _raycastService;
        private int _currentCharges;
        private float _shootCooldown;
        private float _chargesCooldown;
        private float _laserTime;
        private bool _isShooting;

        public bool CanShoot => _currentCharges > 0 && _shootCooldown <= 0 && !_isShooting;
        
        public void Initialize(LaserGunConfig config, ILineRenderer lineRenderer, IRaycastService raycastService)
        {
            _config = config;
            _lineRenderer = lineRenderer;
            _raycastService = raycastService;
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
            _laserTime = 0f;
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
            _laserTime += deltaTime;
            Vector2 origin = _laserStartPoint.position;
            Vector2 direction = transform.up;
            Vector2 endPosition = origin + direction * _config.MaxDistance;

            if (_raycastService.TryRaycast(origin, direction, _config.MaxDistance, out Vector2 hitPoint))
                endPosition = hitPoint;

            _lineRenderer.UpdateLine(origin, endPosition);

            if (_laserTime >= _config.LaserDuration)
            {
                _isShooting = false;
                _lineRenderer.Disable();
            }
        }
    }
}