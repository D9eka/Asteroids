using System;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Score;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public class EnemyLifecycleManager : IEnemyLifecycleManager, IDisposable
    {
        private readonly IPoolableLifecycleManager<IPoolable> _poolLifecycle;
        private readonly IScoreService _scoreService;

        public EnemyLifecycleManager(IPoolableLifecycleManager<IPoolable> poolLifecycle, IScoreService scoreService)
        {
            _poolLifecycle = poolLifecycle;
            _poolLifecycle.OnDespawned += OnPoolableDespawned;
            
            _scoreService = scoreService;
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
            _scoreService.AddScore(killer, enemy);
            _poolLifecycle.Despawn(enemy);
        }

        private void OnPoolableDespawned(IPoolable poolable)
        {
            if (poolable is IEnemy enemy)
            {
                enemy.OnKilled -= HandleEnemyKilled;
            }
        }

        public void ClearAll() => _poolLifecycle.ClearAll();
    }
}