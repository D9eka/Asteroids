using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProvider
    {
        public float Probability { get; }
        public float SpawnInterval { get; } 

        public IEnemy Spawn(Vector2 position);
    }
}