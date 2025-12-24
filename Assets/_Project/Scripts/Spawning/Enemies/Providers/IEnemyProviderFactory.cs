using System.Threading.Tasks;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        public Task<IEnemyProvider> Create(EnemyTypeSpawnConfig spawnConfig);
    }
}