using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Enemies;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializerBase
    {
        void Initialize(IEnemy enemy, EnemyTypeSpawnConfig spawnConfig);
        bool CanInitialize(IEnemy enemy);
    }
}