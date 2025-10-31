using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Providers;

namespace Asteroids.Scripts.Spawning.Enemies.Core
{
    public interface IEnemyFactory
    {
        IEnemy Spawn(IEnemyProvider provider);
    }
}