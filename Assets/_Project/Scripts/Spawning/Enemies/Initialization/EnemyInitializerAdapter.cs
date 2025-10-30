using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Config;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class EnemyInitializerAdapter<TEnemy, TConfig> : IEnemyInitializerBase
        where TEnemy : class, IEnemy
        where TConfig : EnemyTypeConfig
    {
        private readonly IEnemyInitializer<TEnemy, TConfig> _inner;

        [Inject]
        public EnemyInitializerAdapter(IEnemyInitializer<TEnemy, TConfig> inner)
        {
            _inner = inner;
        }

        public void Initialize(IEnemy enemy, EnemyTypeSpawnConfig spawnConfig)
        {
            if (enemy is TEnemy e && spawnConfig.Config is TConfig c)
            {
                _inner.Initialize(e, c);
            }
            else
            {
                UnityEngine.Debug.LogWarning(
                    $"Cannot initialize {enemy.GetType().Name} with config {spawnConfig.GetType().Name}");
            }
        }

        public bool CanInitialize(IEnemy enemy) => enemy is TEnemy;
    }
}