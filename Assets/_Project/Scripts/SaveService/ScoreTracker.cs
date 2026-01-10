using System;
using Asteroids.Scripts.Score;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.SaveService
{
    public class ScoreTracker : IScoreTracker, IInitializable, IDisposable
    {
        private readonly ISaveService _saveService;
        private readonly IScoreService _scoreService;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private ReactiveProperty<int> _previousScore = new();
        private ReactiveProperty<int> _highestScore = new();

        public IReadOnlyReactiveProperty<int> PreviousScore => _previousScore;
        public IReadOnlyReactiveProperty<int> HighestScore => _highestScore;

        public ScoreTracker(ISaveService saveService, IScoreService scoreService)
        {
            _saveService = saveService;
            _scoreService = scoreService;
        }

        public async void Initialize()
        {
            _scoreService.TotalScore
                .Subscribe(UpdateProgress)
                .AddTo(_disposables);

            SaveData data = await _saveService.Load();
            _previousScore.Value = data.PreviousScore;
            _highestScore.Value = data.HighestScore;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public async void SaveCurrentScore()
        {
            SaveData saveData = await _saveService.Load();
            saveData.PreviousScore = _previousScore.Value;
            saveData.HighestScore = _highestScore.Value;
            
            _saveService.Save(saveData);
        }

        private void UpdateProgress(int score)
        {
            _previousScore.Value = score;
            _highestScore.Value = Mathf.Max(_highestScore.Value, score);
        }
    }
}