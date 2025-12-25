using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProvider
    {
        public EnemyType EnemyType { get; }
        public float Probability { get; }
        public float SpawnInterval { get; }

        public void AppendConfig(EnemyTypeSpawnConfig config);
    }
}