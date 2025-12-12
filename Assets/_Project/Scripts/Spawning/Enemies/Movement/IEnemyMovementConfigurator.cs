using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Movement.DirectionProviders;
using Asteroids.Scripts.Movement.DirectionProviders.Config;
using Asteroids.Scripts.Movement.RotationProviders;
using Asteroids.Scripts.Movement.RotationProviders.Config;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Movement
{
    public interface IEnemyMovementConfigurator
    {
        public void Initialize(Transform playerTransform);
        public void Configure(IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config);
        public IDirectionProvider CreateDirectionProvider(DirectionProviderConfig parameters, Vector2 direction);
        public IRotationProvider CreateRotationProvider(RotationProviderConfig parameters, Transform self);
    }
}