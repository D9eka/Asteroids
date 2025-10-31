using System;
using Zenject;

namespace Asteroids.Scripts.Spawning.Common.Pooling
{
    public interface IPoolableLifecycleManager<T> where T : class, IPoolable
    {
        public event Action<T> OnDespawned;
        
        void Register(T instance, IMemoryPool pool);
        void Despawn(T instance);
        void ClearAll();
    }
}