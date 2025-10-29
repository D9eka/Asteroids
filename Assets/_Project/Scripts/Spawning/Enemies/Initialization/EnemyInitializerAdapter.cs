using _Project.Scripts.Enemies;
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

        public void Initialize(IEnemy enemy, EnemyTypeConfig config)
        {
            if (enemy is TEnemy e && config is TConfig c)
                _inner.Initialize(e, c);
        }

        public bool CanInitialize(IEnemy enemy) => enemy is TEnemy;
    }
}