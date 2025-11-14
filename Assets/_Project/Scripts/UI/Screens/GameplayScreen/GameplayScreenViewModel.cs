using System;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Score;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI.GameplayScreen
{
    public class GameplayScreenViewModel : IViewModel, IInitializable, IDisposable
    {
        private readonly IScoreService _scoreService;
        private readonly IPlayerParamsService _paramsService;
        private readonly IGameStateController _gameStateController;
        private readonly IUIController _uiController;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ReactiveProperty<int> CurrentScore { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<string> PlayerParams { get; } = new ReactiveProperty<string>("");
        public IReactiveCommand<bool> ShowRestartButtonCommand { get; } = new ReactiveCommand<bool>();

        public GameplayScreenViewModel(
            IScoreService scoreService,
            IPlayerParamsService paramsService,
            IGameStateController gameStateController,
            IUIController uiController)
        {
            _scoreService = scoreService;
            _paramsService = paramsService;
            _gameStateController = gameStateController;
            _uiController = uiController;
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

        public void OnExitClicked(IView view)
        {
            _gameStateController.HandleExitRequest();
            ShowRestartButtonCommand.Execute(false);
            _uiController.CloseScreen(view);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}