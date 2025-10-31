using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Enemies.Providers;

namespace _Project.Scripts.Spawning.Enemies.Core
{
    public interface IEnemyFactory
    {
        IEnemy Spawn(IEnemyProvider provider);
    }
}