using _Project.Scripts.Enemies;

namespace _Project.Scripts.Spawning.Factory
{
    public interface IEnemyFactory
    {
        IEnemy Spawn();
    }
}