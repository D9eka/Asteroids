using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Config;
using _Project.Scripts.Spawning.Pooling;
using UnityEngine;

namespace _Project.Scripts.Spawning.Providers
{
    public class PooledEnemyProvider<T> : IPooledEnemyProvider where T : MonoBehaviour, IEnemy
    {
        private readonly GenericPool<T> _pool;

        public float Probability { get; }
        public float SpawnInterval { get; }
        public EnemyTypeConfig Config { get; }

        public PooledEnemyProvider(GenericPool<T> pool, EnemyTypeConfig config)
        {
            _pool = pool;
            Config = config;
            Probability = config.SpawnProbability;
            SpawnInterval = config.SpawnInterval;
        }

        public IEnemy Spawn(Vector2 position)
        {
            return _pool.Spawn(position);
        }
    }
}