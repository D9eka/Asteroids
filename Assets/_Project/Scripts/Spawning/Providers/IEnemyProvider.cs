using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Spawning.Providers
{
    public interface IEnemyProvider
    {
        float Probability { get; }
        float SpawnInterval { get; } 

        IEnemy Spawn(Vector2 position);
    }
}