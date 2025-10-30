using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Config;
using _Project.Scripts.Spawning.Enemies.Fragments;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class AsteroidInitializer : IEnemyInitializer<Asteroid, AsteroidTypeConfig>
    {
        private readonly IAsteroidFragmentFactory _fragmentsFactory;

        [Inject]
        public AsteroidInitializer(IAsteroidFragmentFactory fragmentsFactory)
        {
            _fragmentsFactory = fragmentsFactory;
        }
        
        public void Initialize(Asteroid enemy, AsteroidTypeConfig config)
        {
            enemy.Initialize(_fragmentsFactory, config);
        }
    }
}