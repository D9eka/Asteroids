using System;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Score;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI
{
    public class GameUIViewModel : IInitializable, IDisposable
    {
        private readonly IScoreService _scoreService;
        private readonly IPlayerParamsService _paramsService;
        private readonly IGameStateController _gameStateController;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ReactiveProperty<int> CurrentScore { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<string> PlayerParams { get; } = new ReactiveProperty<string>("");
        public IReactiveCommand<bool> ShowRestartButtonCommand { get; } = new ReactiveCommand<bool>();

        public GameUIViewModel(
            IScoreService scoreService,
            IPlayerParamsService paramsService,
            IGameStateController gameStateController)
        {
            _scoreService = scoreService;
            _paramsService = paramsService;
            _gameStateController = gameStateController;
        }

        public void Initialize()
        {
            _scoreService.TotalScore
                .Subscribe(score => CurrentScore.Value = score)
                .AddTo(_disposables);
        
            _gameStateController.PlayerDeath
                .Subscribe(_ => ShowRestartButtonCommand.Execute(true))
                .AddTo(_disposables);
        
            _paramsService.Params
                .Subscribe(param => PlayerParams.Value = param)
                .AddTo(_disposables);
        }

        public void OnRestartClicked()
        {
            _gameStateController.HandleRestartRequest();
            ShowRestartButtonCommand.Execute(false);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}