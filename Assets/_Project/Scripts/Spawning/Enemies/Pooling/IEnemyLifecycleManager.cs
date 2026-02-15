using System;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Enemies;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public interface IEnemyLifecycleManager
    {
        public event Action<DamageInfo, IEnemy> OnEnemyKilled;
        
        void Register(IEnemy enemy, IMemoryPool pool);
    }
}