using System;
using Asteroids.Scripts.Core.GameExit;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.SaveService;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.MainScreen
{
    public class MainScreenViewModel : IViewModel, IInitializable, IDisposable
    {
        private readonly ISaveService _saveService;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly IGameExitService _gameExitService;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public ReactiveProperty<int> PreviousScore { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> HighestScore { get; } = new ReactiveProperty<int>(0);

        
        public MainScreenViewModel(ISaveService saveService, IGameplaySessionManager gameplaySessionManager, 
            IGameExitService gameExitService)
        {
            _saveService = saveService;
            _gameplaySessionManager = gameplaySessionManager;
            _gameExitService = gameExitService;
        }

        public void Initialize()
        {
            _saveService.Load();
            _saveService.Data.PreviousScore
                .Subscribe(score => PreviousScore.Value = score)
                .AddTo(_disposables);
            
            _saveService.Data.HighestScore
                .Subscribe(score => HighestScore.Value = score)
                .AddTo(_disposables);
        }

        public void OnStartClicked()
        {
            _gameplaySessionManager.Start();
        }

        public void OnExitClicked()
        {
            _gameExitService.Exit();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}