using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig
{
    [CreateAssetMenu(menuName = "Configs/Spawn/Enemy/EnemySpawnConfig", fileName = "EnemySpawnConfig")]
    public class EnemySpawnConfigSo : ScriptableObject
    {
        [field:SerializeField] public List<EnemyTypeSpawnConfigSo> Enemies { get; private set; }
    }
}