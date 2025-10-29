using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Common.Pooling;
using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public class PooledEnemyProvider<TEnemy, TConfig> : IPooledEnemyProvider<TEnemy, TConfig>
        where TEnemy : MonoBehaviour, IEnemy
        where TConfig : EnemyTypeConfig
    {
        private readonly GenericPool<TEnemy> _pool;

        public TConfig Config { get; private set; }
        public float Probability => Config.SpawnProbability;
        public float SpawnInterval => Config.SpawnInterval;

        public PooledEnemyProvider(GenericPool<TEnemy> pool, TConfig config)
        {
            _pool = pool;
            Config = config;
        }

        TEnemy IPooledEnemyProvider<TEnemy, TConfig>.Spawn(Vector2 position)
        {
            return _pool.Spawn(position);
        }
    }
}