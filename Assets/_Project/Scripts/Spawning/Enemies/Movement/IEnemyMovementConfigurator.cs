using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Movement
{
    public interface IEnemyMovementConfigurator
    {
        public void Initialize(int playerId);
        public void Configure(int enemyEntity, IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config);
    }
}