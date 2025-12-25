using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Spawning.Enemies.Providers;

namespace Asteroids.Scripts.Configs.Runtime
{
    public class EnemySpawnConfigRuntime : IDisposable
    {
        private readonly IEnemyConfigProvider _enemyConfigProvider;
        private List<IEnemyProvider> _enemyProviders;

        public EnemySpawnConfigRuntime(IEnemyConfigProvider enemyConfigProvider)
        {
            _enemyConfigProvider = enemyConfigProvider;
        }
        
        public void Initialize(List<IEnemyProvider> enemyProviders)
        {
            _enemyProviders = enemyProviders;
            UpdateConfigs();
            _enemyConfigProvider.OnEnemyConfigUpdated += UpdateConfigs;
        }
        
        private void UpdateConfigs()
        {
            foreach (IEnemyProvider enemyProvider in _enemyProviders)
            {
                foreach (EnemyTypeSpawnConfig enemyTypeSpawnConfig in _enemyConfigProvider.EnemySpawnConfig.Enemies)
                {
                    if (enemyProvider.EnemyType == enemyTypeSpawnConfig.Config.Type)
                    {
                        enemyProvider.AppendConfig(enemyTypeSpawnConfig);
                    }
                }
            }
        }
        public void Dispose()
        {
            _enemyConfigProvider.OnEnemyConfigUpdated -= UpdateConfigs;
        }
    }
}