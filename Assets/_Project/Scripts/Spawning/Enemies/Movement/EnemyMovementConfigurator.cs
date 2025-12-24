using System;
using Asteroids.Scripts.Camera;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Movement.DirectionProviders;
using Asteroids.Scripts.Movement.RotationProviders;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Scripts.Spawning.Enemies.Movement
{
    public class EnemyMovementConfigurator : IEnemyMovementConfigurator
    {
        private readonly ICameraBoundsUpdater _cameraBoundsUpdater;
        
        private Transform _playerTransform;

        public EnemyMovementConfigurator(ICameraBoundsUpdater cameraBoundsUpdater)
        {
            _cameraBoundsUpdater = cameraBoundsUpdater;
        }

        public void Initialize(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void Configure(IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config)
        {
            Vector2 min = _cameraBoundsUpdater.MinBounds;
            Vector2 max = _cameraBoundsUpdater.MaxBounds;
            Vector2 direction = GetRandomInwardDirection(spawnPos, min, max);

            DirectionProviderConfig directionProviderConfig = config.DirectionProviderConfig;
            IDirectionProvider directionProvider = CreateDirectionProvider(directionProviderConfig, direction);
            float speed = Random.Range(directionProviderConfig.MinSpeed, directionProviderConfig.MaxSpeed);
            
            RotationProviderConfig rotationConfig = config.RotationProviderConfig;
            IRotationProvider rotationProvider = CreateRotationProvider(rotationConfig, enemy.Transform);

            enemy.Movement.SetDirectionProvider(directionProvider);
            enemy.Movement.SetVelocity(speed);
            enemy.Movement.SetRotationProvider(rotationProvider);
        }

        public IDirectionProvider CreateDirectionProvider(DirectionProviderConfig config, Vector2 direction)
        {
            return config switch
            {
                LinearDirectionProviderConfig => new LinearDirectionProvider(direction),
                TargetDirectionProviderConfig targetDirectionProviderConfig => new TargetDirectionProvider(direction,
                    _playerTransform, targetDirectionProviderConfig.UpdateInterval),
                _ => throw new NotSupportedException($"Unsupported movement parameters type: {config.GetType().Name}")
            };
        }

        public IRotationProvider CreateRotationProvider(RotationProviderConfig config, Transform self)
        {
            return config switch
            {
                MovementBasedRotationProviderConfig => new MovementDirectionRotationProvider(
                    self.gameObject.GetComponent<Rigidbody2D>()),
                TargetBasedRotationProviderConfig rotationParameters => new TargetDirectionRotationProvider(self,
                    _playerTransform, rotationParameters.RotationSpeed),
                _ => throw new NotSupportedException($"Unsupported rotation parameters type: {config.GetType().Name}")
            };
        }

        private Vector2 GetRandomInwardDirection(Vector2 spawnPos, Vector2 minBounds, Vector2 maxBounds)
        {
            Vector2 center = (minBounds + maxBounds) / 2f;
            Vector2 baseDir = (center - spawnPos).normalized;
            float angleOffset = Random.Range(-25f, 25f);
            return Quaternion.Euler(0, 0, angleOffset) * baseDir;
        }
    }
}