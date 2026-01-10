using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Enemies.SpawnConfig
{
    [Serializable]
    public class EnemySpawnConfig
    {
        [field:SerializeReference] public List<EnemyTypeSpawnConfig> Enemies { get; private set; }

        public EnemySpawnConfig(List<EnemyTypeSpawnConfig> enemies)
        {
            Enemies = enemies;
        }

        public EnemySpawnConfig()
        {
            Enemies = new List<EnemyTypeSpawnConfig>();
            Enemies.Add(new EnemyTypeSpawnConfig(new AsteroidTypeConfig(),
                0.9f,2f,3f,15));
            Enemies.Add(new EnemyTypeSpawnConfig(new UfoTypeConfig(),
                0.5f, 5f, 2f, 5));
        }
    }
}