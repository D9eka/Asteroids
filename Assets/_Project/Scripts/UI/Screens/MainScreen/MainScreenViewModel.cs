using System;
using _Project.Scripts.Core.InjectIds;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.SaveService;
using Zenject;
using UniRx;

namespace Asteroids.Scripts.UI.MainScreen
{
    public class MainScreenViewModel : IViewModel, IInitializable, IDisposable
    {
        private readonly ISaveService _saveService;
        private readonly IPauseSystem _pauseSystem;
        private readonly IUIController _uiController;
        private readonly IView _gameplayView;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public ReactiveProperty<int> PreviousScore { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> HighestScore { get; } = new ReactiveProperty<int>(0);

        
        public MainScreenViewModel(ISaveService saveService, IPauseSystem pauseSystem, IUIController uiController, 
            [Inject(Id = ScreenInjectId.GameplayScreenView)] IView gameplayView)
        {
            _saveService = saveService;
            _pauseSystem = pauseSystem;
            _uiController = uiController;
            _gameplayView = gameplayView;
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
            _pauseSystem.Resume();
            _uiController.OpenScreen(_gameplayView);
        }

        public void OnExitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}