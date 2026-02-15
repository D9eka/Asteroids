using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Spawning.Common.Pooling;

namespace Asteroids.Scripts.Enemies
{
    public interface IEnemy : IDamageable, IPoolable
    {
        public event Action<IEcsEntity, IEnemy> OnKilled;
        
        EnemyType Type { get; }
        CollisionHandler CollisionHandler { get; }

        public void SetType(EnemyType type);
    }
}