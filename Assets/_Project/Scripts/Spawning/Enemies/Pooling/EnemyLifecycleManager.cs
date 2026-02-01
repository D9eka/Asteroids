using System;
using Asteroids.Scripts.Effects.Explosion;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Fusion;
using UnityEngine;
using Zenject;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public class EnemyLifecycleManager : IEnemyLifecycleManager, IInitializable, IDisposable
    {
        public event Action<GameObject, IEnemy> OnEnemyKilled; 
        
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _poolLifecycle;
        private readonly ExplosionEffectSpawner _explosionEffectSpawner;

        public EnemyLifecycleManager(IPoolableLifecycleManager<Pooling_IPoolable> poolLifecycle,
            ExplosionEffectSpawner explosionEffectSpawner)
        {
            _poolLifecycle = poolLifecycle;
            _explosionEffectSpawner = explosionEffectSpawner;
        }

        public void Initialize()
        {
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
            if (enemy is NetworkBehaviour)
            {
                _explosionEffectSpawner.AddEnemy(enemy);
                return;
            }

            _poolLifecycle.Register(enemy, pool);
            _explosionEffectSpawner.AddEnemy(enemy);
        }

        private void HandleEnemyKilled(GameObject killer, IEnemy enemy)
        {
            if (enemy is NetworkBehaviour netEnemy)
            {
                if (netEnemy.Object != null && netEnemy.Object.HasStateAuthority && netEnemy.Runner != null)
                    netEnemy.Runner.Despawn(netEnemy.Object);

                enemy.OnKilled -= HandleEnemyKilled;
            }
            else
            {
                _poolLifecycle.Despawn(enemy);
            }
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
