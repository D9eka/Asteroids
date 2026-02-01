using System.Collections.Generic;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Weapons.Projectile;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.Pooling
{
    public class NetworkObjectPoolRegistry
    {
        private readonly Dictionary<NetworkObject, IPoolAdapter> _pools = new();

        public void Register<T>(NetworkObject prefab, ObjectPool<T> pool) where T : Component, IPoolable
        {
            if (prefab == null || pool == null)
                return;

            _pools[prefab] = new ObjectPoolAdapter<T>(pool);
        }

        public void Register(NetworkObject prefab, ProjectilePool pool)
        {
            if (prefab == null || pool == null)
                return;

            _pools[prefab] = new ProjectilePoolAdapter(pool);
        }

        public bool TryGet(NetworkObject prefab, out IPoolAdapter adapter)
        {
            return _pools.TryGetValue(prefab, out adapter);
        }

        public void Unregister(NetworkObject prefab)
        {
            if (prefab == null)
                return;

            _pools.Remove(prefab);
        }
    }
}
