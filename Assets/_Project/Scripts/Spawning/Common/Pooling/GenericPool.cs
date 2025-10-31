using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Common.Pooling
{
    public class GenericPool<T> : MonoMemoryPool<Vector3, T> where T : Component, IPoolable
    {
        protected override void Reinitialize(Vector3 position, T item)
        {
            item.transform.position = position;
            item.OnSpawned();
        }

        protected override void OnDespawned(T item)
        {
            base.OnDespawned(item);
            item.OnDespawned();
        }
    }
}