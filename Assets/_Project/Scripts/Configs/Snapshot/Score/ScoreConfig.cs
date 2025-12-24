using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Score
{
    [Serializable]
    public class ScoreConfig
    {
        [field:SerializeField] public List<ScoreValue> Scores { get; private set; } = new List<ScoreValue>();

        public ScoreConfig(List<ScoreValue> scores)
        {
            Scores = scores;
        }
    }
}