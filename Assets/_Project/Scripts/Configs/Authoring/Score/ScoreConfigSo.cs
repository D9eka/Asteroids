using System.Collections.Generic;
using Asteroids.Scripts.Configs.Authoring.Enemies;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Score
{
    [CreateAssetMenu(menuName = "Configs/Score/ScoreConfig", fileName = "ScoreConfig")]
    public class ScoreConfigSo : ScriptableObject
    {
        [SerializeField] private List<EnemyTypeConfigSo> _entries = new();

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