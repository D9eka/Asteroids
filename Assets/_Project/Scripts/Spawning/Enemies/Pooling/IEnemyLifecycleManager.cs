using Asteroids.Scripts.Enemies;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public interface IEnemyLifecycleManager
    {
        void Register(IEnemy enemy, IMemoryPool pool);
        void ClearAll();
    }
}