using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/Spawn/Enemy/EnemySpawnConfig", fileName = "EnemySpawnConfig")]
    public class EnemySpawnConfig : ScriptableObject
    {
        [field:SerializeField] public List<EnemyTypeSpawnConfig> Enemies { get; private set; }
    }
}