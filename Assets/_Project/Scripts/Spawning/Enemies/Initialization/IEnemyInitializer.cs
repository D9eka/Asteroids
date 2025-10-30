using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public interface IEnemyInitializer<in TEnemy, in TConfig>
        where TEnemy : IEnemy
        where TConfig : EnemyTypeConfig
    {
        public void Initialize(TEnemy enemy, TConfig config);
    }
}