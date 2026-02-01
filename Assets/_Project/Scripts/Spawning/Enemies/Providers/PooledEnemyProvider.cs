using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;
using Fusion;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public class PooledEnemyProvider<TEnemy, TConfig> : IPooledEnemyProvider<TEnemy, TConfig>
        where TEnemy : MonoBehaviour, IEnemy
        where TConfig : EnemyTypeSpawnConfig
    {
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;
        private readonly ObjectPool<TEnemy> _pool;
        private readonly IEnemyConfigProvider _enemyConfigProvider;

        public TConfig Config { get; private set; }
        public NetworkObject Prefab { get; }
        public EnemyType EnemyType => Config.Config.Type;
        public float Probability => Config.SpawnProbability;
        public float SpawnInterval => Config.SpawnInterval;

        public PooledEnemyProvider(IEnemyLifecycleManager enemyLifecycleManager, ObjectPool<TEnemy> pool,
            TConfig config, NetworkObject prefab)
        {
            _enemyLifecycleManager = enemyLifecycleManager;
            _pool = pool;
            Config = config;
            Prefab = prefab;
        }
        
        public void AppendConfig(EnemyTypeSpawnConfig config)
        {
            Config = config as TConfig;
        }

        TEnemy IPooledEnemyProvider<TEnemy, TConfig>.Spawn(Vector2 position)
        {
            TEnemy enemy = _pool.Spawn(position);
            _enemyLifecycleManager.Register(enemy, _pool);
            return enemy;
        }
    }
}
