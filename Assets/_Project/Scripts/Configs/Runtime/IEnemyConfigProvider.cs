using System;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;

namespace Asteroids.Scripts.Configs.Runtime
{
    public interface IEnemyConfigProvider
    {
        public event Action OnConfigUpdated;
        
        public EnemySpawnConfig EnemySpawnConfig { get; }
    }
}