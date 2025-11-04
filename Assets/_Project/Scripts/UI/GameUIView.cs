using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Scripts.UI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _paramsText;
        [SerializeField] private Button _restartButton;

        private GameUIViewModel _viewModel;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public void Construct(GameUIViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Awake()
        {
            _viewModel.CurrentScore
                .Subscribe(UpdateScoreText)
                .AddTo(_disposables);
            
            _viewModel.PlayerParams
                .Subscribe(UpdateParamsText)
                .AddTo(_disposables);
            
            _viewModel.ShowRestartButtonCommand
                .Subscribe(SetRestartButtonActive)
                .AddTo(_disposables);
            
            _restartButton.onClick.AsObservable()
                .Subscribe(_ => _viewModel.OnRestartClicked())
                .AddTo(_disposables);
            
            HideRestartButton();
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

        private void SetRestartButtonActive(bool isActive)
        {
            _restartButton.gameObject.SetActive(isActive);
        }

        private void HideRestartButton()
        {
            _restartButton.gameObject.SetActive(false);
        }
    }
}