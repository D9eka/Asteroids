using System;
using Asteroids.Scripts.Enemies;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Spawning.Enemies.Pooling
{
    public interface IEnemyLifecycleManager
    {
        public event Action<GameObject, IEnemy> OnEnemyKilled; 
        
        void Register(IEnemy enemy, IMemoryPool pool);
    }
}