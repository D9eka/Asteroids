using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Enemies.Config;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializerBase
    {
        public void Initialize(IEnemy enemy, EnemyTypeConfig config);
        public bool CanInitialize(IEnemy enemy);
    }
}