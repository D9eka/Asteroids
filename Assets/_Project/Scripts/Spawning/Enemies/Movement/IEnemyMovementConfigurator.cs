using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Movement.DirectionProviders;
using Asteroids.Scripts.Movement.RotationProviders;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Movement
{
    public interface IEnemyMovementConfigurator
    {
        public void Initialize(Transform playerTransform);
        public void Configure(IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config);
        public IDirectionProvider CreateDirectionProvider(DirectionProviderConfig config, Vector2 direction);
        public IRotationProvider CreateRotationProvider(RotationProviderConfig config, Transform self);
    }
}