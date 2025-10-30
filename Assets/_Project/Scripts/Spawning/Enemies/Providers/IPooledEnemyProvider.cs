using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public interface IPooledEnemyProvider<out TEnemy, out TConfig> : IEnemyProvider
        where TEnemy : class, IEnemy
        where TConfig : EnemyTypeSpawnConfig
    {
        public TConfig Config { get; }
        public TEnemy Spawn(Vector2 position);
    }
}