using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Authoring.Enemies;
using Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Authoring.Movement.Direction;
using Asteroids.Scripts.Configs.Authoring.Movement.Rotation;
using Asteroids.Scripts.Configs.Authoring.Player;
using Asteroids.Scripts.Configs.Authoring.Score;
using Asteroids.Scripts.Configs.Authoring.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Authoring.Weapons.LaserGun;
using Asteroids.Scripts.Configs.Snapshot;
using Asteroids.Scripts.Configs.Snapshot.Enemies;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Snapshot.Movement.Direction;
using Asteroids.Scripts.Configs.Snapshot.Movement.Rotation;
using Asteroids.Scripts.Configs.Snapshot.Player;
using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Configs.Snapshot.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.LaserGun;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Enemies;

namespace Asteroids.Scripts.Configs.Mapping
{
    public class ConfigDataMapper
    {
        public ConfigData Map(
            PlayerMovementDataSo movementDataSo,
            BulletGunConfigSo bulletGunConfigSo,
            LaserGunConfigSo laserGunConfigSo,
            EnemySpawnConfigSo enemySpawnConfigSo,
            ScoreConfigSo scoreConfigSo)
        {
            return new ConfigData(
                CreatePlayerDataConfig(movementDataSo, bulletGunConfigSo, laserGunConfigSo),
                CreateEnemySpawnConfig(enemySpawnConfigSo),
                CreateScoreConfig(scoreConfigSo)
            );
        }

        private PlayerConfig CreatePlayerDataConfig(PlayerMovementDataSo movementDataSo, BulletGunConfigSo bulletGunConfigSo, 
            LaserGunConfigSo laserGunConfigSo)
        {
            
            return new PlayerConfig
            (
                new PlayerMovementConfig(movementDataSo.ThrustForce, movementDataSo.RotationSpeed),
                CreateBulletGunConfig(bulletGunConfigSo),
                CreateLaserGunConfig(laserGunConfigSo)
            );
        }
        
        private BulletGunConfig CreateBulletGunConfig(BulletGunConfigSo bulletGunConfigSo)
        {
            return new BulletGunConfig(bulletGunConfigSo.DamageType, bulletGunConfigSo.FireRate,
                new ProjectileConfig(
                    bulletGunConfigSo.ProjectileConfig.Speed,
                    bulletGunConfigSo.ProjectileConfig.LifeTime));
        }

        private LaserGunConfig CreateLaserGunConfig(LaserGunConfigSo laserGunConfigSo)
        {
            return new LaserGunConfig(laserGunConfigSo.DamageType, laserGunConfigSo.FireRate,
                laserGunConfigSo.LaserDuration, laserGunConfigSo.RechargeRate,
                laserGunConfigSo.MaxCharges, laserGunConfigSo.MaxCharges);
        }

        private EnemySpawnConfig CreateEnemySpawnConfig(EnemySpawnConfigSo enemySpawnConfigSo)
        {
            List<EnemyTypeSpawnConfig> enemies = new();
            foreach (EnemyTypeSpawnConfigSo enemyTypeSpawnConfigSo in enemySpawnConfigSo.Enemies)
            {
                enemies.Add(CreateEnemyTypeSpawnConfig(enemyTypeSpawnConfigSo));
            }
            return new EnemySpawnConfig(enemies);
        }

        private EnemyTypeSpawnConfig CreateEnemyTypeSpawnConfig(EnemyTypeSpawnConfigSo enemyTypeSpawnConfigSo)
        {
            EnemyTypeConfigSo enemyTypeConfigSo = enemyTypeSpawnConfigSo.ConfigSo;
            DirectionProviderConfig directionProviderConfig =
                CreateDirectionProviderConfig(enemyTypeConfigSo.DirectionProviderConfigSo);
            RotationProviderConfig rotationProviderConfig = 
                CreateRotationProviderConfig(enemyTypeConfigSo.RotationProviderConfigSo);
            EnemyTypeConfig enemyTypeConfig =
                CreateEnemyTypeConfig(enemyTypeConfigSo, directionProviderConfig, rotationProviderConfig);

            if (enemyTypeSpawnConfigSo is AsteroidFragmentTypeSpawnConfigSo asteroidFragmentTypeSpawnConfigSo)
            {
                return new AsteroidFragmentTypeSpawnConfig(
                    enemyTypeConfig, 
                    asteroidFragmentTypeSpawnConfigSo.SpawnProbability, 
                    asteroidFragmentTypeSpawnConfigSo.SpawnInterval,
                    asteroidFragmentTypeSpawnConfigSo.SpawnDistanceOutsideBounds, 
                    asteroidFragmentTypeSpawnConfigSo.PoolSize, 
                    asteroidFragmentTypeSpawnConfigSo.MinFragments, 
                    asteroidFragmentTypeSpawnConfigSo.MaxFragments, 
                    asteroidFragmentTypeSpawnConfigSo.FragmentPositionOffsetModefier, 
                    asteroidFragmentTypeSpawnConfigSo.FragmentSpeedMultiplier);
            }
            
            return new EnemyTypeSpawnConfig(
                    enemyTypeConfig,
                    enemyTypeSpawnConfigSo.SpawnProbability, enemyTypeSpawnConfigSo.SpawnInterval,
                    enemyTypeSpawnConfigSo.SpawnDistanceOutsideBounds, enemyTypeSpawnConfigSo.PoolSize);
        }

        private DirectionProviderConfig CreateDirectionProviderConfig(DirectionProviderConfigSo directionProviderConfigSo)
        {
            if (directionProviderConfigSo is LinearDirectionProviderConfigSo linearDirectionProviderConfigSo)
            {
                return new LinearDirectionProviderConfig(
                    linearDirectionProviderConfigSo.MinSpeed, linearDirectionProviderConfigSo.MaxSpeed);
            }
            if (directionProviderConfigSo is TargetDirectionProviderConfigSo targetDirectionProviderConfigSo)
            {
                return new TargetDirectionProviderConfig(
                    targetDirectionProviderConfigSo.UpdateInterval, 
                    targetDirectionProviderConfigSo.MinSpeed, targetDirectionProviderConfigSo.MaxSpeed);
            }
            throw new NotSupportedException($"Unsupported movement parameters type: {directionProviderConfigSo.GetType().Name}");
        }

        private RotationProviderConfig CreateRotationProviderConfig(RotationProviderConfigSo rotationProviderConfigSo)
        {
            if (rotationProviderConfigSo is MovementBasedRotationProviderConfigSo)
            {
                return new MovementBasedRotationProviderConfig();
            }
            if (rotationProviderConfigSo is TargetBasedRotationProviderConfigSo targetBasedRotationProviderConfigSo)
            {
                return new TargetBasedRotationProviderConfig(targetBasedRotationProviderConfigSo.RotationSpeed);
            }
            throw new NotSupportedException($"Unsupported rotation parameters type: {rotationProviderConfigSo.GetType().Name}");
        }

        private EnemyTypeConfig CreateEnemyTypeConfig(EnemyTypeConfigSo enemyTypeConfigSo, 
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig)
        {
            if (enemyTypeConfigSo is AsteroidTypeConfigSo asteroidTypeConfigSo)
            {
                return CreateAsteroidTypeConfig(asteroidTypeConfigSo, directionProviderConfig, rotationProviderConfig);
            }
            if (enemyTypeConfigSo is UfoTypeConfigSo ufoTypeConfigSo)
            {
                return CreateUfoTypeConfig(ufoTypeConfigSo, directionProviderConfig, rotationProviderConfig);
            }

            return new EnemyTypeConfig(enemyTypeConfigSo.PrefabId, enemyTypeConfigSo.Type, enemyTypeConfigSo.Score,
                directionProviderConfig, rotationProviderConfig);
        }

        private AsteroidTypeConfig CreateAsteroidTypeConfig(AsteroidTypeConfigSo asteroidTypeConfigSo, 
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig)
        {
            AsteroidFragmentTypeSpawnConfig asteroidFragmentTypeSpawnConfig =
                CreateEnemyTypeSpawnConfig(asteroidTypeConfigSo.AsteroidFragmentSpawnConfigSo) as AsteroidFragmentTypeSpawnConfig;
            
            return new AsteroidTypeConfig(asteroidTypeConfigSo.PrefabId, asteroidTypeConfigSo.Type,
                asteroidTypeConfigSo.Score, directionProviderConfig, rotationProviderConfig, asteroidFragmentTypeSpawnConfig);
        }

        private UfoTypeConfig CreateUfoTypeConfig(UfoTypeConfigSo ufoTypeConfigSo, 
            DirectionProviderConfig directionProviderConfig, RotationProviderConfig rotationProviderConfig)
        {
            return new UfoTypeConfig(ufoTypeConfigSo.PrefabId, ufoTypeConfigSo.Type, ufoTypeConfigSo.Score,
                directionProviderConfig, rotationProviderConfig,
                CreateBulletGunConfig(ufoTypeConfigSo.BulletGunConfigSo));
        }

        private ScoreConfig CreateScoreConfig(ScoreConfigSo scoreConfigSo)
        {
            List<ScoreValue> scoreValues = new List<ScoreValue>();
            foreach (KeyValuePair<EnemyType, int> scoreValue in scoreConfigSo.ScoreByConfig)
            {
                scoreValues.Add(new ScoreValue(scoreValue.Key, scoreValue.Value));
            }
            return new ScoreConfig(scoreValues);
        }
    }
}