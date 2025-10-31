using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IPooledEnemyProvider<out TEnemy, out TConfig> : IEnemyProvider
        where TEnemy : class, IEnemy
        where TConfig : EnemyTypeSpawnConfig
    {
        public TConfig Config { get; }
        public TEnemy Spawn(Vector2 position);
    }
}