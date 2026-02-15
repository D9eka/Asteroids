using System;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Effects.Explosion;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Leopotam.EcsLite;
using Zenject;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public class EnemyLifecycleManager : IEnemyLifecycleManager, IInitializable, IDisposable
    {
        public event Action<DamageInfo, IEnemy> OnEnemyKilled;
        
        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _poolLifecycle;
        private readonly EcsWorld _ecsWorld;
        private readonly EntityViewRegistry _entityViewRegistry;
        private readonly ExplosionEffectSpawner _explosionEffectSpawner;

        public EnemyLifecycleManager(IPoolableLifecycleManager<Pooling_IPoolable> poolLifecycle,
            EntityViewRegistry entityViewRegistry, EcsWorld ecsWorld,
            ExplosionEffectSpawner explosionEffectSpawner)
        {
            _poolLifecycle = poolLifecycle;
            _entityViewRegistry = entityViewRegistry;
            _ecsWorld = ecsWorld;
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
            _poolLifecycle.Register(enemy, pool);
            _explosionEffectSpawner.AddEnemy(enemy);
        }

        private void HandleEnemyKilled(DamageInfo damageInfo, IEnemy enemy)
        {
            _poolLifecycle.Despawn(enemy);
            OnEnemyKilled?.Invoke(damageInfo, enemy);
        }

        private void OnPoolableDespawned(Pooling_IPoolable poolable)
        {
            if (poolable is IEnemy enemy && enemy.Id >= 0)
            {
                enemy.OnKilled -= HandleEnemyKilled;
                _explosionEffectSpawner.RemoveEnemy(enemy);
                _entityViewRegistry.Unregister(enemy.Id);
                _ecsWorld.DelEntity(enemy.Id);
                enemy.SetId(-1);
            }
        }

        public void ClearAll() => _poolLifecycle.ClearAll();
    }
}