using System;
using Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Authoring.Player;
using Asteroids.Scripts.Configs.Authoring.Score;
using Asteroids.Scripts.Configs.Authoring.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Authoring.Weapons.LaserGun;
using Asteroids.Scripts.Configs.Mapping;
using Asteroids.Scripts.Configs.Snapshot;
using Asteroids.Scripts.RemoteConfigs;
using Zenject;

namespace Asteroids.Scripts.ConfigsProvider
{
    public class GameConfigProvider : IInitializable, IDisposable
    {
        private readonly IRemoteConfigService _remoteConfigService;
        
        public ConfigData ConfigData { get; private set; }
        
        public GameConfigProvider(ConfigDataMapper configDataMapper, PlayerMovementDataSo movementDataSo, 
            BulletGunConfigSo bulletGunConfigSo, LaserGunConfigSo laserGunConfigSo,
            EnemySpawnConfigSo enemySpawnConfigSo, ScoreConfigSo scoreConfigSo, IRemoteConfigService remoteConfigService)
        {
            ConfigData = configDataMapper.Map(movementDataSo, bulletGunConfigSo, laserGunConfigSo, enemySpawnConfigSo, scoreConfigSo);
            _remoteConfigService = remoteConfigService;
        }

        public void Initialize()
        {
            _remoteConfigService.OnConfigLoaded += RemoteConfigServiceOnConfigLoaded;
            _remoteConfigService.OnConfigUpdated += RemoteConfigServiceOnConfigUpdated;
        }

        public void Dispose()
        {
            _remoteConfigService.OnConfigLoaded -= RemoteConfigServiceOnConfigLoaded;
            _remoteConfigService.OnConfigUpdated -= RemoteConfigServiceOnConfigUpdated;
        }

        private void RemoteConfigServiceOnConfigLoaded()
        {
            if (_remoteConfigService.TryGetConfig(out ConfigData configData))
            {
                ConfigData = configData;
            }
        }

        private void RemoteConfigServiceOnConfigUpdated()
        {
            if (_remoteConfigService.TryGetConfig(out ConfigData configData))
            {
                ConfigData = configData;
            }
        }
    }
}

