using System;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Score
{
    [Serializable]
    public class ScoreValue
    {
        [field:SerializeField] public EnemyType EnemyType  { get; private set; }
        [field:SerializeField] public int Score  { get; private set; }

        public ScoreValue(EnemyType enemyType, int score)
        {
            EnemyType = enemyType;
            Score = score;
        }
    }
}