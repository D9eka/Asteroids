using System;
using Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Authoring.Player;
using Asteroids.Scripts.Configs.Authoring.Score;
using Asteroids.Scripts.Configs.Authoring.Weapons.BulletGun;
using Asteroids.Scripts.Configs.Authoring.Weapons.LaserGun;
using Asteroids.Scripts.Configs.Mapping;
using Asteroids.Scripts.Configs.Snapshot;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Snapshot.Player;
using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.RemoteConfigs;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Configs.Runtime
{
    public class GameConfigProvider : IInitializable, IDisposable, IPlayerConfigProvider, IEnemyConfigProvider, IScoreConfigProvider
    {
        public event Action OnPlayerConfigUpdated;
        public event Action OnEnemyConfigUpdated;
        public event Action OnScoreConfigUpdated;
        
        private readonly IRemoteConfigService _remoteConfigService;

        private ConfigData _configData;
        
        public PlayerConfig PlayerConfig => _configData.PlayerConfig;
        public EnemySpawnConfig EnemySpawnConfig => _configData.EnemySpawnConfig;
        public ScoreConfig ScoreConfig => _configData.ScoreConfig;
        
        public GameConfigProvider(ConfigDataMapper configDataMapper, PlayerMovementDataSo movementDataSo, 
            BulletGunConfigSo bulletGunConfigSo, LaserGunConfigSo laserGunConfigSo,
            EnemySpawnConfigSo enemySpawnConfigSo, ScoreConfigSo scoreConfigSo, IRemoteConfigService remoteConfigService)
        {
            _configData = configDataMapper.Map(movementDataSo, bulletGunConfigSo, laserGunConfigSo, enemySpawnConfigSo, scoreConfigSo);
            _remoteConfigService = remoteConfigService;
            //var str = JsonUtility.ToJson(_configData);
            //Debug.Log(str);
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
                UpdateConfig(configData);
            }
        }

        private void RemoteConfigServiceOnConfigUpdated()
        {
            if (_remoteConfigService.TryGetConfig(out ConfigData configData))
            {
                UpdateConfig(configData);
            }
        }

        private void UpdateConfig(ConfigData configData)
        {
            _configData = configData;
            OnPlayerConfigUpdated?.Invoke();
            OnEnemyConfigUpdated?.Invoke();
            OnScoreConfigUpdated?.Invoke();
        }
    }
}

