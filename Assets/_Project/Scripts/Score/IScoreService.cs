using Asteroids.Scripts.Enemies;
using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public interface IScoreService
    {
        public IReadOnlyReactiveProperty<int> TotalScore { get; }
        
        public void AddScore(GameObject killer, IEnemy enemy);

        public void ResetScore();
    }
}