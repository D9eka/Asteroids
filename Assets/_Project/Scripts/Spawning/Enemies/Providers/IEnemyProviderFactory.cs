using System.Threading.Tasks;
using Asteroids.Scripts.Spawning.Enemies.Config;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        public Task<IEnemyProvider> Create(EnemyTypeSpawnConfig spawnConfig);
    }
}