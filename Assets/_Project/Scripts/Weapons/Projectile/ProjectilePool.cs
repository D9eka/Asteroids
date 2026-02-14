using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectilePool : MonoMemoryPool<ProjectilePoolData, Projectile>
    {
        private readonly EcsWorld _ecsWorld;
        private readonly EntityViewRegistry _entityViewRegistry;

        public ProjectilePool(EcsWorld ecsWorld, EntityViewRegistry entityViewRegistry)
        {
            _ecsWorld = ecsWorld;
            _entityViewRegistry = entityViewRegistry;
        }

        protected override void Reinitialize(ProjectilePoolData data, Projectile item)
        {
            if (item == null || data.Config == null || data.CollisionService == null)
            {
                Debug.LogError("Invalid parameters in ProjectilePool.Reinitialize");
                return;
            }

            item.transform.position = data.Position;
            item.transform.rotation = data.Rotation;
            _entityViewRegistry.Register(data.Id, item);
            item.Initialize(this, data.Id, data.Config, data.DamageInfo, data.CollisionService);
        }

        protected override void OnDespawned(Projectile item)
        {
            base.OnDespawned(item);
            _entityViewRegistry.Unregister(item.Id);
            _ecsWorld.DelEntity(item.Id);
            item.SetId(-1);
            item?.OnDespawned();
        }

        protected override void OnSpawned(Projectile item)
        {
            base.OnSpawned(item);
            item?.OnSpawned();
        }
    }
}