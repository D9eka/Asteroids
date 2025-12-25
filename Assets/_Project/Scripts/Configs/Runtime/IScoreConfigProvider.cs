using System;
using Asteroids.Scripts.Configs.Snapshot.Score;

namespace Asteroids.Scripts.Configs.Runtime
{
    public interface IScoreConfigProvider
    {
        public event Action OnScoreConfigUpdated;
        
        public ScoreConfig ScoreConfig { get; }
    }
}