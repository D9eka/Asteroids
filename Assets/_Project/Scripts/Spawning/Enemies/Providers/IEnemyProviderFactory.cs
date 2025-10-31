using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Config;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        EnemyType EnemyType { get; }
    
        public IEnemyProvider Create(EnemyTypeSpawnConfig spawnConfig);
    }
}