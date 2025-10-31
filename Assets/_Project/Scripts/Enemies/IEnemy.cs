using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public interface IEnemy : ITransformProvider, IDamageable, IDamageSource, IPoolable, IPausable
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        EnemyType Type { get; }
        CollisionHandler CollisionHandler { get; }
        Movement.Core.Movement Movement { get; }

        public void SetType(EnemyType type);
    }
}