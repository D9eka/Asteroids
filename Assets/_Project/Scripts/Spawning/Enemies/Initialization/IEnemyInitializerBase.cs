using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Config;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializerBase
    {
        void Initialize(IEnemy enemy, EnemyTypeSpawnConfig spawnConfig);
        bool CanInitialize(IEnemy enemy);
    }
}