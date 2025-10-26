using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Spawning.Config
{
    [CreateAssetMenu(menuName = "Configs/EnemySpawnConfig", fileName = "EnemySpawnConfig")]
    public class EnemySpawnConfig : ScriptableObject
    {
        [field:SerializeField] public List<EnemyTypeConfig> Enemies { get; private set; }
    }
}