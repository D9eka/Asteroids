using _Project.Scripts.Enemies;

namespace _Project.Scripts.Spawning.Enemies.Core
{
    public interface IEnemyFactory
    {
        IEnemy Spawn();
    }
}