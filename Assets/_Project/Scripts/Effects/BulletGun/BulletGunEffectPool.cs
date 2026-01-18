using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Effects
{
    public class BulletGunEffectPool : MemoryPool<Transform, BulletGunEffect>
    {
        protected override void Reinitialize(Transform parent, BulletGunEffect item)
        {
            if (item == null)
            {
                Debug.LogError("Invalid parameters in BulletGunEffectPool.Reinitialize");
                return;
            }

            item.transform.parent = parent;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.Initialize(this);
        }

        protected override void OnDespawned(BulletGunEffect item)
        {
            item.transform.parent = null;
            base.OnDespawned(item);
            item?.OnDespawned();
        }

        protected override void OnSpawned(BulletGunEffect item)
        {
            base.OnSpawned(item);
            item?.OnSpawned();
        }
    }
}