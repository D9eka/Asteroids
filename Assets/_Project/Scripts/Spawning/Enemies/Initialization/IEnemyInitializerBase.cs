using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Enemies.Config;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializerBase
    {
        void Initialize(IEnemy enemy, EnemyTypeSpawnConfig spawnConfig);
        bool CanInitialize(IEnemy enemy);
    }
}