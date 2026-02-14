using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Enemies;
using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public interface IScoreService
    {
        public IReadOnlyReactiveProperty<int> TotalScore { get; }

        public void ApplyConfig(ScoreConfig scoreConfig);
        
        public void AddScore(IEcsEntity instigatorEntity, IEnemy enemy);

        public void ResetScore();
    }
}