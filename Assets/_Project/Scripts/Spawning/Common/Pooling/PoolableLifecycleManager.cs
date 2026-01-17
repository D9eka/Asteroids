using System;
using System.Collections.Generic;
using Zenject;

namespace Asteroids.Scripts.Spawning.Common.Pooling
{
    public class PoolableLifecycleManager<T> : IPoolableLifecycleManager<T> where T : class, IPoolable
    {
        public event Action<T> OnDespawned;
        
        private readonly Dictionary<T, IMemoryPool> _activeObjects = new();

        public void Register(T instance, IMemoryPool pool)
        {
            if (_activeObjects.ContainsKey(instance))
                return;

            _activeObjects.Add(instance, pool);
        }

        public void Despawn(T instance)
        {
            if (_activeObjects.TryGetValue(instance, out var pool))
            {
                pool.Despawn(instance);
                _activeObjects.Remove(instance);
            }
            else
            {
                instance.OnDespawned();
            }

            OnDespawned?.Invoke(instance);
        }

        public void ClearAll()
        {
            foreach (var kvp in _activeObjects)
            {
                if (kvp.Key.Enabled)
                {
                    kvp.Value.Despawn(kvp.Key);
                }
            }

            _activeObjects.Clear();
        }
    }
}