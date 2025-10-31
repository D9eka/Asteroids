using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializer<in TEnemy, in TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        public void Initialize(TEnemy enemy, TConfig config);
    }
}