using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Enemies;
using UniRx;

namespace Asteroids.Scripts.Score
{
    public interface IScoreService
    {
        public IReadOnlyReactiveProperty<int> TotalScore { get; }

        public void ApplyConfig(ScoreConfig scoreConfig);

        public void AddScore(DamageInfo damageInfo, IEnemy enemy);

        public void ResetScore();
    }
}