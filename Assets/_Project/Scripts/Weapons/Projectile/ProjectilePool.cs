using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectilePool : MonoMemoryPool<Vector3, Quaternion, ProjectileConfig, DamageInfo, ICollisionService, Projectile>
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, ProjectileConfig config, 
            DamageInfo damageInfo, ICollisionService collisionService, Projectile item)
        {
            if (item == null)
                return;

            item.transform.position = position;
            item.transform.rotation = rotation;

            if (config == null || collisionService == null)
            {
                item.OnSpawned();
                return;
            }

            item.Initialize(config, damageInfo, collisionService);
        }

        protected override void OnDespawned(Projectile item)
        {
            base.OnDespawned(item);
            item?.OnDespawned();
        }

        protected override void OnSpawned(Projectile item)
        {
            base.OnSpawned(item);
            item?.OnSpawned();
        }
    }
}
