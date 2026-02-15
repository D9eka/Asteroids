using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.Ecs.Weapons.Services;
using Asteroids.Scripts.Weapons.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Weapons.Systems
{
    public class BulletGunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly WeaponViewRegistry _weaponViewRegistry;
        
        private EcsFilter _bulletGunFilter;
        private EcsPool<BulletGunComponent> _bulletGunsPool;
        private EcsPool<WantsToShootTag> _wantsToShootTagsPool;

        public BulletGunSystem(WeaponViewRegistry weaponViewRegistry)
        {
            _weaponViewRegistry = weaponViewRegistry;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _bulletGunFilter = world.Filter<BulletGunComponent>().End();
            _bulletGunsPool = world.GetPool<BulletGunComponent>();
            _wantsToShootTagsPool = world.GetPool<WantsToShootTag>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _bulletGunFilter)
            {
                ref BulletGunComponent bulletGunComponent = ref _bulletGunsPool.Get(entity);
                Recharge(ref bulletGunComponent);
                if (CanShoot(entity, bulletGunComponent) &&
                    _weaponViewRegistry.TryGet(entity, out IWeapon weapon))
                {
                    weapon.Shoot();
                    bulletGunComponent.Cooldown = bulletGunComponent.FireRate;
                }
            }
        }

        private void Recharge(ref BulletGunComponent bulletGunComponent)
        {
            if (bulletGunComponent.Cooldown > 0)
            {
                bulletGunComponent.Cooldown -= Time.fixedDeltaTime;
            }
        }

        private bool CanShoot(int entity, BulletGunComponent bulletGunComponent)
        {
            return _wantsToShootTagsPool.Has(entity) && bulletGunComponent.Cooldown <= 0;
        }
    }
}