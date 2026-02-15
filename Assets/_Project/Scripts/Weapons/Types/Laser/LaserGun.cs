using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Services.Raycast;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.Laser
{
    public class LaserGun : MonoBehaviour, IWeapon
    {
        public event Action<IWeapon> OnShoot;
        
        [SerializeField] private Transform _laserStartPoint;
        
        private IEcsEntity _instigatorEntity;
        private LaserGunConfig _config;
        private ILineRenderer _lineRenderer;
        private IRaycastService _raycastService;
        private ICollisionService _collisionService;
        private EcsPool<LaserGunComponent> _laserGunComponentsPool;
        
        public int Id { get; private set; }

        public Transform Transform => transform;
        
        public void Initialize(int id, EcsWorld ecsWorld, IEcsEntity instigatorEntity, 
            LaserGunConfig config, ILineRenderer lineRenderer, 
            IRaycastService raycastService, ICollisionService collisionService)
        {
            Id = id;
            _laserGunComponentsPool = ecsWorld.GetPool<LaserGunComponent>();
            _instigatorEntity = instigatorEntity;
            _config = config;
            _lineRenderer = lineRenderer;
            _raycastService = raycastService;
            _collisionService = collisionService;
            
            _lineRenderer.Disable();
        }

        private void FixedUpdate()
        {
            if (_laserGunComponentsPool == null) return;
            LaserGunComponent laserGunComponent = _laserGunComponentsPool.Get(Id);
            if (laserGunComponent.IsActive)
            {
                UpdateLaser();
            }
            else
            {
                _lineRenderer.Disable();
            }
        }

        public void Shoot()
        {
            _lineRenderer.Enable();
            OnShoot?.Invoke(this);
        }

        public void ApplyConfig(LaserGunConfig config)
        {
            _config = config;
        }

        private void UpdateLaser()
        {
            Vector2 origin = _laserStartPoint.position;
            Vector2 direction = transform.up;
            Vector2 endPosition = origin + direction * _config.MaxDistance;

            if (_raycastService.TryRaycast(origin, direction, _config.MaxDistance, out RaycastHit2D hit))
            {
                endPosition = hit.transform.position;
                Debug.DrawRay(origin, direction * hit.distance, Color.red);
                _collisionService.OnHit(_instigatorEntity.Transform.gameObject, hit.collider.gameObject);
            }

            _lineRenderer.UpdateLine(origin, endPosition);
        }

    }
}