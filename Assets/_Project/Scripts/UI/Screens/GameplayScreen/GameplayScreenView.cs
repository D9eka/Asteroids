using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.GameplayScreen
{
    public class GameplayScreenView : BaseView
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _paramsText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private GameplayScreenViewModel _screenViewModel;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public void Construct(GameplayScreenViewModel screenViewModel)
        {
            _screenViewModel = screenViewModel;
        }

        protected override void Awake()
        {
            base.Awake();
            
            _screenViewModel.CurrentScore
                .Subscribe(UpdateScoreText)
                .AddTo(_disposables);
            
            _screenViewModel.PlayerParams
                .Subscribe(UpdateParamsText)
                .AddTo(_disposables);
            
            _screenViewModel.ShowRestartButtonCommand
                .Subscribe(SetRestartButtonsActive)
                .AddTo(_disposables);
            
            _restartButton.onClick.AsObservable()
                .Subscribe(_ => _screenViewModel.OnRestartClicked())
                .AddTo(_disposables);
            
            _exitButton.onClick.AsObservable()
                .Subscribe(_ => _screenViewModel.OnExitClicked(this))
                .AddTo(_disposables);
            
            SetRestartButtonsActive(false);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            _restartButton.onClick.RemoveAllListeners();
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void UpdateParamsText(string parameters)
        {
            _paramsText.text = parameters;
        }

        private void SetRestartButtonsActive(bool isActive)
        {
            if (isActive)
            {
                ShowButton(_restartButton);
                ShowButton(_exitButton);
            }
            else
            {
                HideButton(_restartButton);
                HideButton(_exitButton);
            }
        }

        private void ShowButton(Button button)
        {
            button.gameObject.SetActive(true);
            Transform buttonTransform = button.gameObject.transform;
            buttonTransform.DOScale(Vector3.one, 0.3f);
        }

        private void HideButton(Button button)
        {
            Transform buttonTransform = button.gameObject.transform;
            buttonTransform.DOScale(Vector3.zero, 0.3f)
                .OnComplete(() => button.gameObject.SetActive(false));
        }
    }
}