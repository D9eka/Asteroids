using System;
using System.Collections.Generic;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Score
{
    [Serializable]
    public class ScoreConfig
    {
        [field:SerializeField] public List<ScoreValue> Scores { get; private set; } = new List<ScoreValue>();
        
        private Dictionary<EnemyType, int> _cache;
        public IReadOnlyDictionary<EnemyType, int> ScoreByConfig
        {
            get
            {
                if (_cache == null) BuildCache();
                return _cache;
            }
        }

        public ScoreConfig(List<ScoreValue> scores)
        {
            Scores = scores;
        }

        public ScoreConfig()
        {
            Scores = new List<ScoreValue>
            {
                new ScoreValue(EnemyType.Asteroid, 40),
                new ScoreValue(EnemyType.Ufo, 80),
                new ScoreValue(EnemyType.AsteroidFragment, 20)
            };
        }

        private void BuildCache()
        {
            _cache = new Dictionary<EnemyType, int> { { EnemyType.None, 0 } };
            foreach (ScoreValue scoreValue in Scores)
            {
                _cache[scoreValue.EnemyType] = scoreValue.Score;
            }
        }
    }
}