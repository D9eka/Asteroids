using _Project.Scripts.Spawning.Config;

namespace _Project.Scripts.Spawning.Providers
{
    public interface IPooledEnemyProvider : IEnemyProvider
    {
        EnemyTypeConfig Config { get; }
    }
}