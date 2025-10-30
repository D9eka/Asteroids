using _Project.Scripts.Enemies;
using _Project.Scripts.Score;
using _Project.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = _Project.Scripts.Spawning.Common.Pooling.IPoolable;

namespace _Project.Scripts.Spawning.Enemies.Pooling
{
    public class EnemyLifecycleManager : IEnemyLifecycleManager
    {
        private readonly IPoolableLifecycleManager<IPoolable> _poolLifecycle;
        private readonly IScoreService _scoreService;

        public EnemyLifecycleManager(IPoolableLifecycleManager<IPoolable> poolLifecycle, IScoreService scoreService)
        {
            _poolLifecycle = poolLifecycle;
            _poolLifecycle.OnDespawned += OnPoolableDespawned;
            
            _scoreService = scoreService;
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