using UnityEngine;
using Zenject;

namespace _Project.Scripts.Effects.Explosion
{
    public class ExplosionEffectPool : MemoryPool<Vector3, AudioClip, ExplosionEffect>
    {
        protected override void Reinitialize(Vector3 position, AudioClip clip, ExplosionEffect item)
        {
            if (item == null)
            {
                Debug.LogError("Invalid parameters in ExplosionEffectPool.Reinitialize");
                return;
            }

            item.transform.position = position;
            item.Initialize(this, clip);
        }

        protected override void OnDespawned(ExplosionEffect item)
        {
            base.OnDespawned(item);
            item?.OnDespawned();
        }

        protected override void OnSpawned(ExplosionEffect item)
        {
            base.OnSpawned(item);
            item?.OnSpawned();
        }
    }
}