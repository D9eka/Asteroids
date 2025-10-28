using _Project.Scripts.Collision;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapons.Projectile
{
    public class ProjectTilePool<T> : MonoMemoryPool<Vector3, Quaternion, ProjectileData, ICollisionService, T>
        where T : Component, IProjectile
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, ProjectileData data, ICollisionService collisionService, T item)
        {
            if (item == null || data == null || collisionService == null)
            {
                Debug.LogError("Invalid parameters in ProjectTilePool.Reinitialize");
                return;
            }

            item.transform.position = position;
            item.transform.rotation = rotation;
            item.Initialize(data, collisionService);
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