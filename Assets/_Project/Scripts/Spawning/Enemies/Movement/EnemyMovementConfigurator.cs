using System;
using Asteroids.Scripts.Ecs.Components;
using Asteroids.Scripts.Camera;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Enemies;
using Leopotam.EcsLite;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Scripts.Spawning.Enemies.Movement
{
    public class EnemyMovementConfigurator : IEnemyMovementConfigurator
    {
        private readonly EcsWorld _ecsWorld;
        private readonly ICameraBoundsUpdater _cameraBoundsUpdater;
        
        private int _playerId;

        public EnemyMovementConfigurator(EcsWorld ecsWorld, ICameraBoundsUpdater cameraBoundsUpdater)
        {
            _ecsWorld = ecsWorld;
            _cameraBoundsUpdater = cameraBoundsUpdater;
        }

        public void Initialize(int playerId)
        {
            _playerId = playerId;
        }

        public void Configure(int enemyEntity, IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config)
        {
            Vector2 min = _cameraBoundsUpdater.MinBounds;
            Vector2 max = _cameraBoundsUpdater.MaxBounds;
            Vector2 direction = GetRandomInwardDirection(spawnPos, min, max);

            EcsPool<PositionComponent> positionPool = _ecsWorld.GetPool<PositionComponent>();
            ref PositionComponent positionComponent = ref positionPool.Add(enemyEntity);
            positionComponent.Position = enemy.Transform.position;
            EcsPool<VelocityResultComponent>  velocityResultPool = _ecsWorld.GetPool<VelocityResultComponent>();
            velocityResultPool.Add(enemyEntity);
            EcsPool<RotationResultComponent> rotationResultPool = _ecsWorld.GetPool<RotationResultComponent>();
            ref RotationResultComponent rotationResultComponent = ref rotationResultPool.Add(enemyEntity);
            rotationResultComponent.Angle = enemy.Transform.eulerAngles.z;
            
            DirectionProviderConfig directionProviderConfig = config.DirectionProviderConfig;
            AddDirectionProvider(enemyEntity, directionProviderConfig, direction);
            
            RotationProviderConfig rotationConfig = config.RotationProviderConfig;
            AddRotationProvider(enemyEntity, rotationConfig);

            SetVelocity(enemyEntity, Random.Range(directionProviderConfig.MinSpeed, directionProviderConfig.MaxSpeed));
        }

        private void AddDirectionProvider(int enemyEntity, DirectionProviderConfig config, Vector2 direction)
        {
            EcsPool<LinearDirectionComponent> linearComponentPool = _ecsWorld.GetPool<LinearDirectionComponent>();
            ref LinearDirectionComponent linearDirectionComponent = ref linearComponentPool.Add(enemyEntity);
            linearDirectionComponent.Direction = direction;
            switch (config)
            {
                case LinearDirectionProviderConfig:
                    break;
                case TargetDirectionProviderConfig targetDirectionProviderConfig:
                    EcsPool<TargetFollowComponent> targetFollowComponentPool = _ecsWorld.GetPool<TargetFollowComponent>();
                    ref TargetFollowComponent targetFollowComponent = ref targetFollowComponentPool.Add(enemyEntity);
                    targetFollowComponent.TargetEntity = _playerId;
                    targetFollowComponent.UpdateInterval = targetDirectionProviderConfig.UpdateInterval;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported movement parameters type: {config.GetType().Name}");
            }
        }

        private void AddRotationProvider(int enemyEntity, RotationProviderConfig config)
        {
            switch (config)
            {
                case MovementBasedRotationProviderConfig:
                    EcsPool<VelocityRotationTag> velocityRotationPool = _ecsWorld.GetPool<VelocityRotationTag>();
                    velocityRotationPool.Add(enemyEntity);
                    break;
                case TargetBasedRotationProviderConfig rotationParameters:
                    EcsPool<TargetRotationComponent> targetRotationPool = _ecsWorld.GetPool<TargetRotationComponent>();
                    ref TargetRotationComponent targetRotationComponent = ref targetRotationPool.Add(enemyEntity);
                    targetRotationComponent.TargetEntity = _playerId;
                    targetRotationComponent.RotationSpeed = rotationParameters.RotationSpeed;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported rotation parameters type: {config.GetType().Name}");
            }
        }

        private void SetVelocity(int enemyEntity, float velocity)
        {
            EcsPool<VelocityMovementComponent> velocityMovementPool = _ecsWorld.GetPool<VelocityMovementComponent>();
            ref VelocityMovementComponent velocityMovementComponent = ref velocityMovementPool.Add(enemyEntity);
            velocityMovementComponent.Velocity = velocity;
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