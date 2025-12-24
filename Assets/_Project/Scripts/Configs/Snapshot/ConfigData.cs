using System;
using Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig;
using Asteroids.Scripts.Configs.Snapshot.Player;
using Asteroids.Scripts.Configs.Snapshot.Score;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot
{
    [Serializable]
    public class ConfigData
    {
        [field:SerializeField] public PlayerConfig PlayerConfig { get; private set; }
        [field:SerializeField] public EnemySpawnConfig EnemySpawnConfig { get; private set; }
        [field:SerializeField] public ScoreConfig ScoreConfig { get; private set; }

        public ConfigData(PlayerConfig playerConfig, EnemySpawnConfig enemySpawnConfig, ScoreConfig scoreConfig)
        {
            PlayerConfig = playerConfig;
            EnemySpawnConfig = enemySpawnConfig;
            ScoreConfig = scoreConfig;
        }
    }
}