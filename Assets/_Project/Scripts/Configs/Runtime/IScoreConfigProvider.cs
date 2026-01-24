using System;
using Asteroids.Scripts.Configs.Snapshot.Score;

namespace Asteroids.Scripts.Configs.Runtime
{
    public interface IScoreConfigProvider
    {
        public event Action OnConfigUpdated;
        
        public ScoreConfig ScoreConfig { get; }
    }
}