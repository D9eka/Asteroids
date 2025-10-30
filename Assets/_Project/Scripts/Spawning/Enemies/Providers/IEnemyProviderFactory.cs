using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Enemies.Config;

namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        EnemyType EnemyType { get; }
    
        public IEnemyProvider Create(EnemyTypeSpawnConfig spawnConfig);
    }
}