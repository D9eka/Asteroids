using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Enemies;
using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public interface IScoreService
    {
        public IReadOnlyReactiveProperty<int> TotalScore { get; }

        public void ApplyConfig(ScoreConfig scoreConfig);
        
        public void AddScore(GameObject killer, IEnemy enemy);

        public void ResetScore();
    }
}