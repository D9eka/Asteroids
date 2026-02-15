using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Ecs.Weapons.Services;
using Asteroids.Scripts.Weapons.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Weapons.Systems
{
    public class LaserGunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly WeaponViewRegistry _weaponViewRegistry;
        
        private EcsFilter _laserGunFilter;
        private EcsPool<LaserGunComponent> _laserGunsPool;
        private EcsPool<WantsToShootTag> _wantsToShootTagsPool;
        
        public LaserGunSystem(WeaponViewRegistry weaponViewRegistry)
        {
            _weaponViewRegistry = weaponViewRegistry;
        }
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _laserGunFilter = world.Filter<LaserGunComponent>().End();
            _laserGunsPool = world.GetPool<LaserGunComponent>();
            _wantsToShootTagsPool = world.GetPool<WantsToShootTag>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _laserGunFilter)
            {
                ref LaserGunComponent laserGunComponent = ref _laserGunsPool.Get(entity);
                Recharge(ref laserGunComponent);
                if (CanShoot(entity, laserGunComponent) &&
                    _weaponViewRegistry.TryGet(entity, out IWeapon weapon))
                {
                    weapon.Shoot();
                    laserGunComponent.IsActive = true;
                    laserGunComponent.CurrentCharges--;
                    laserGunComponent.ActiveTime = laserGunComponent.Duration;
                }
            }
        }

        private void Recharge(ref LaserGunComponent laserGunComponent)
        {
            if (laserGunComponent.IsActive)
            {
                UpdateLaser(ref laserGunComponent);
            }
            else
            {
                if (laserGunComponent.ShootCooldown > 0)
                {
                    laserGunComponent.ShootCooldown -= Time.fixedDeltaTime;
                }
                if (laserGunComponent.CurrentCharges < laserGunComponent.MaxCharges)
                {
                    UpdateChanges(ref laserGunComponent);
                }
            }
        }

        private void UpdateLaser(ref LaserGunComponent laserGunComponent)
        {
            laserGunComponent.ActiveTime -= Time.fixedDeltaTime;
            if (laserGunComponent.ActiveTime <= 0)
            {
                laserGunComponent.IsActive = false;
                laserGunComponent.ShootCooldown = laserGunComponent.FireRate;
            }
        }

        private void UpdateChanges(ref LaserGunComponent laserGunComponent)
        {
            if (laserGunComponent.ChargesCooldown > 0)
            {
                laserGunComponent.ChargesCooldown -= Time.fixedDeltaTime;
            }
            if (laserGunComponent.ChargesCooldown <= 0)
            {
                laserGunComponent.CurrentCharges++;
                laserGunComponent.ChargesCooldown = laserGunComponent.RechargeRate;
            }
        }

        private bool CanShoot(int entity, LaserGunComponent laserGunComponent)
        {
            return _wantsToShootTagsPool.Has(entity) && !laserGunComponent.IsActive && 
                laserGunComponent.ShootCooldown <= 0 && laserGunComponent.CurrentCharges > 0;
        }
    }
}