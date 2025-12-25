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
    }
}