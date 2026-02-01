using Asteroids.Scripts.Spawning.Common.Pooling;
using Fusion;
using UnityEngine;
namespace _Project.Scripts.Multiplayer.Pooling
{
    public sealed class ObjectPoolAdapter<T> : IPoolAdapter where T : Component, IPoolable
    {
        private readonly ObjectPool<T> _pool;

        public ObjectPoolAdapter(ObjectPool<T> pool)
        {
            _pool = pool;
        }

        public NetworkObject Spawn(Vector3 position, Quaternion rotation)
        {
            T instance = _pool.Spawn(position);
            if (instance == null)
                return null;

            Transform transform = instance.transform;
            transform.rotation = rotation;
            return instance.GetComponent<NetworkObject>();
        }

        public void Despawn(NetworkObject instance)
        {
            if (instance == null)
                return;

            T poolable = instance.GetComponent<T>();
            if (poolable != null)
                _pool.Despawn(poolable);
        }
    }
}