using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectilePool<T> : MonoMemoryPool<Vector3, Quaternion, ProjectileData, DamageInfo, ICollisionService, T>
        where T : Component, IProjectile
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, ProjectileData data, 
            DamageInfo damageInfo, ICollisionService collisionService, T item)
        {
            if (item == null || data == null || collisionService == null)
            {
                Debug.LogError("Invalid parameters in ProjectilePool.Reinitialize");
                return;
            }

            item.transform.position = position;
            item.transform.rotation = rotation;
            item.Initialize(data, damageInfo, collisionService);
        }

        protected override void OnDespawned(T item)
        {
            base.OnDespawned(item);
            item?.OnDespawned();
        }

        protected override void OnSpawned(T item)
        {
            base.OnSpawned(item);
            item?.OnSpawned();
        }
    }
}