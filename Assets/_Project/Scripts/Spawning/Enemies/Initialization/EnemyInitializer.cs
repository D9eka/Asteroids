using _Project.Scripts.Collision;
using _Project.Scripts.Enemies;
using _Project.Scripts.Spawning.Enemies.Config;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Initialization
{
    public class EnemyInitializer : IEnemyInitializer<IEnemy, EnemyTypeConfig>
    {
        private readonly ICollisionService _collisionService;

        [Inject]
        public EnemyInitializer(ICollisionService collisionService)
        {
            _collisionService = collisionService;
        }
        
        public void Initialize(IEnemy enemy, EnemyTypeConfig config)
        {
            enemy.CollisionHandler.Initialize(_collisionService);
        }
    }
}