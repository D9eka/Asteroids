using System;
using Asteroids.Scripts.Score;
using Zenject;

namespace Asteroids.Scripts.Configs.Runtime
{
    public class ScoreConfigRuntime : IInitializable, IDisposable
    {
        private readonly IScoreService _scoreService;
        private readonly IScoreConfigProvider _scoreConfigProvider;

        public ScoreConfigRuntime(IScoreService scoreService, IScoreConfigProvider scoreConfigProvider)
        {
            _scoreService = scoreService;
            _scoreConfigProvider = scoreConfigProvider;
        }

        public void Initialize()
        {
            _scoreService.ApplyConfig(_scoreConfigProvider.ScoreConfig);
            
            _scoreConfigProvider.OnConfigUpdated += UpdateConfig;
        }
        
        public void Dispose()
        {
            _scoreConfigProvider.OnConfigUpdated -= UpdateConfig;
        }
        
        private void UpdateConfig()
        {
            _scoreService.ApplyConfig(_scoreConfigProvider.ScoreConfig);
        }
    }
}