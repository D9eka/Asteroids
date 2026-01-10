using System;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public class EnemyLifecycleManager : IEnemyLifecycleManager, IDisposable
    {
        public event Action<GameObject, IEnemy> OnEnemyKilled; 
        
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _poolLifecycle;

        public EnemyLifecycleManager(IPoolableLifecycleManager<Pooling_IPoolable> poolLifecycle)
        {
            _poolLifecycle = poolLifecycle;
            _poolLifecycle.OnDespawned += OnPoolableDespawned;
        }
        
        public void Dispose()
        {
            ClearAll();
            _poolLifecycle.OnDespawned -= OnPoolableDespawned;
        }

        public void Register(IEnemy enemy, IMemoryPool pool)
        {
            enemy.OnKilled += HandleEnemyKilled;
            _poolLifecycle.Register(enemy, pool);
        }

        private void HandleEnemyKilled(GameObject killer, IEnemy enemy)
        {
            _poolLifecycle.Despawn(enemy);
            OnEnemyKilled?.Invoke(killer, enemy);
        }

        private void OnPoolableDespawned(Pooling_IPoolable poolable)
        {
            if (poolable is IEnemy enemy)
            {
                enemy.OnKilled -= HandleEnemyKilled;
            }
        }

        public void ClearAll() => _poolLifecycle.ClearAll();
    }
}