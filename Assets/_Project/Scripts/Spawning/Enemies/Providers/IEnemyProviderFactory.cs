using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Cysharp.Threading.Tasks;

namespace Asteroids.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        public UniTask<IEnemyProvider> Create(EnemyTypeSpawnConfig spawnConfig);
    }
}