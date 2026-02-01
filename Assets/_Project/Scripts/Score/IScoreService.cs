using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Enemies;
using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public interface IScoreService
    {
        public void ApplyConfig(ScoreConfig scoreConfig);
    }
}