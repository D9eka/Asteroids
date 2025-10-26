using System;
using _Project.Scripts.Camera;
using _Project.Scripts.Enemies;
using _Project.Scripts.Movement.DirectionProviders;
using _Project.Scripts.Movement.DirectionProviders.Config;
using _Project.Scripts.Spawning.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Spawning.Movement
{
    public class EnemyMovementConfigurator
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

            DirectionProviderConfig movementConfig = config.DirectionProviderConfig ?? new LinearDirectionProviderConfig();
            IDirectionProvider directionProvider = CreateDirectionProvider(movementConfig, direction);
            float speed = Random.Range(movementConfig.MinSpeed, movementConfig.MaxSpeed);

            enemy.Movement.SetDirectionProvider(directionProvider);
            enemy.Movement.SetVelocity(speed);
        }

        private IDirectionProvider CreateDirectionProvider(DirectionProviderConfig parameters, Vector2 direction)
        {
            return parameters switch
            {
                LinearDirectionProviderConfig => new LinearDirectionProvider(direction),
                IntermittentTargetDirectionProviderConfig movementParameters => new IntermittentTargetDirectionProvider(
                    direction, _playerTransform, movementParameters.UpdateInterval, movementParameters.MoveToTargetChance),
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