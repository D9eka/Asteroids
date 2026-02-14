using System;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Enemies;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public interface IEnemyLifecycleManager
    {
        public event Action<IEcsEntity, IEnemy> OnEnemyKilled; 
        
        void Register(IEnemy enemy, IMemoryPool pool);
    }
}