using Asteroids.Scripts.Spawning.Enemies.Config;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        public IEnemyProvider Create(EnemyTypeSpawnConfig spawnConfig);
    }
}