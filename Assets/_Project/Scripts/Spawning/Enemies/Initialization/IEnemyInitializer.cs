using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Enemies;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializer<in TEnemy, in TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        public void Initialize(TEnemy enemy, TConfig config);
    }
}