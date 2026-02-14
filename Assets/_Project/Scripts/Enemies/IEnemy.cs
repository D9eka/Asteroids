using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public interface IEnemy : ITransformProvider, IDamageable, IDamageSource, IPoolable
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        EnemyType Type { get; }
        int Id { get; }
        CollisionHandler CollisionHandler { get; }

        public void SetType(EnemyType type);
        public void SetId(int id);
    }
}