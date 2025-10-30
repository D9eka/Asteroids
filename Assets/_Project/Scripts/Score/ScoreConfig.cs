using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Config;
using UnityEngine;

namespace _Project.Scripts.Score
{
    [CreateAssetMenu(menuName = "Configs/Score/ScoreConfig", fileName = "ScoreConfig")]
    public class ScoreConfig : ScriptableObject
    {
        [SerializeField] private List<EnemyTypeConfig> _entries = new();

        private Dictionary<EnemyType, int> _cache;

        public IReadOnlyDictionary<EnemyType, int> ScoreByConfig
        {
            get
            {
                if (_cache == null) BuildCache();
                return _cache;
            }
        }

        private void BuildCache()
        {
            _cache = new Dictionary<EnemyType, int> { { EnemyType.None, 0 } };
            foreach (var entry in _entries)
            {
                _cache[entry.Type] = entry.Score;
            }
        }
    }
}