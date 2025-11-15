using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.MainScreen
{
    public class MainScreenView : BaseView
    {
        private const string PREVIOUS_SCORE_START_TEXT = "Previous: ";
        private const string HIGHEST_SCORE_START_TEXT = "Highest: ";
        
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitGameButton;
        
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _previousScoreText;
        [SerializeField] private TextMeshProUGUI _highestScoreText;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private MainScreenViewModel _screenViewModel;

        [Inject]
        public void Construct(MainScreenViewModel screenViewModel)
        {
            _screenViewModel = screenViewModel;
        }
        
        private void Awake()
        {
            _startButton.onClick.AsObservable()
                .Subscribe(_ => _screenViewModel.OnStartClicked())
                .AddTo(_disposables);
            
            _exitGameButton.onClick.AsObservable()
                .Subscribe(_ => _screenViewModel.OnExitClicked())
                .AddTo(_disposables);
            
            _screenViewModel.PreviousScore
                .Subscribe(UpdatePreviousScoreText)
                .AddTo(_disposables);
            
            _screenViewModel.HighestScore
                .Subscribe(UpdateHighestScoreText)
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void UpdatePreviousScoreText(int score)
        {
            _previousScoreText.text = PREVIOUS_SCORE_START_TEXT + score.ToString();
        }

        private void UpdateHighestScoreText(int score)
        {
            _highestScoreText.text = HIGHEST_SCORE_START_TEXT + score.ToString();
        }
    }
}