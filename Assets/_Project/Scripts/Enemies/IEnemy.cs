using System;
using _Project.Scripts.Collision;
using _Project.Scripts.Core;
using _Project.Scripts.Damage;
using _Project.Scripts.Pause;
using _Project.Scripts.Spawning.Common.Pooling;
using UnityEngine;

namespace _Project.Scripts.Enemies
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