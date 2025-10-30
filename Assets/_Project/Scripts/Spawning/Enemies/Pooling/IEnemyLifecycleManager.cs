using _Project.Scripts.Enemies;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Pooling
{
    public interface IEnemyLifecycleManager
    {
        void Register(IEnemy enemy, IMemoryPool pool);
        void ClearAll();
    }
}