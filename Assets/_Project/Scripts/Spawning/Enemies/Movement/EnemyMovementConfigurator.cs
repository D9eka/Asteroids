using System;
using _Project.Scripts.Camera;
using _Project.Scripts.Enemies;
using _Project.Scripts.Movement.DirectionProviders;
using _Project.Scripts.Movement.DirectionProviders.Config;
using _Project.Scripts.Movement.RotationProviders;
using _Project.Scripts.Movement.RotationProviders.Config;
using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Spawning.Enemies.Movement
{
    public class EnemyMovementConfigurator : IEnemyMovementConfigurator
    {
        private readonly ICameraBoundsUpdater _cameraBoundsUpdater;
        private readonly Transform _playerTransform;

        public EnemyMovementConfigurator(
            ICameraBoundsUpdater cameraBoundsUpdater,
            Transform playerTransform)
        {
            _cameraBoundsUpdater = cameraBoundsUpdater;
            _playerTransform = playerTransform;
        }

        public void Configure(IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config)
        {
            Vector2 min = _cameraBoundsUpdater.MinBounds;
            Vector2 max = _cameraBoundsUpdater.MaxBounds;
            Vector2 direction = GetRandomInwardDirection(spawnPos, min, max);

            DirectionProviderConfig directionProviderConfig = config.DirectionProviderConfig ??
                ScriptableObject.CreateInstance<LinearDirectionProviderConfig>();
            IDirectionProvider directionProvider = CreateDirectionProvider(directionProviderConfig, direction);
            float speed = Random.Range(directionProviderConfig.MinSpeed, directionProviderConfig.MaxSpeed);
            
            RotationProviderConfig rotationConfig = config.RotationProviderConfig ??
                ScriptableObject.CreateInstance<MovementBasedRotationProviderConfig>();
            IRotationProvider rotationProvider = CreateRotationProvider(rotationConfig, enemy.Transform);

            enemy.Movement.SetDirectionProvider(directionProvider);
            enemy.Movement.SetVelocity(speed);
            enemy.Movement.SetRotationProvider(rotationProvider);
        }

        public IDirectionProvider CreateDirectionProvider(DirectionProviderConfig parameters, Vector2 direction)
        {
            return parameters switch
            {
                LinearDirectionProviderConfig => new LinearDirectionProvider(direction),
                IntermittentTargetDirectionProviderConfig movementParameters => new IntermittentTargetDirectionProvider(
                    direction, _playerTransform, movementParameters.UpdateInterval, movementParameters.MoveToTargetChance),
                _ => throw new NotSupportedException($"Unsupported movement parameters type: {parameters.GetType().Name}")
            };
        }

        public IRotationProvider CreateRotationProvider(RotationProviderConfig parameters, Transform self)
        {
            return parameters switch
            {
                MovementBasedRotationProviderConfig => new MovementDirectionRotationProvider(
                    self.gameObject.GetComponent<Rigidbody2D>()),
                TargetBasedRotationProviderConfig rotationParameters => new TargetDirectionRotationProvider(self,
                    _playerTransform, rotationParameters.RotationSpeed),
                _ => throw new NotSupportedException($"Unsupported movement parameters type: {parameters.GetType().Name}")
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