using Asteroids.Scripts.UI.Buttons;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.EndGameScreen
{
    public class ReviveScreenView : BaseView
    {
        [SerializeField] private ButtonWithCountdown _reviveButton;
        [SerializeField] private Button _exitButton;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private ReviveScreenViewModel _screenViewModel;

        [Inject]
        public void Construct(ReviveScreenViewModel screenViewModel)
        {
            _screenViewModel = screenViewModel;
        }

        private void Awake()
        {
            _reviveButton.onClick.AsObservable()
                .Subscribe(_ => _screenViewModel.OnAdvertisementButtonClicked())
                .AddTo(_disposables);

            _reviveButton.OnEndDelay.Subscribe(_ => _exitButton.onClick.Invoke());
            
            _exitButton.onClick.AsObservable()
                .Subscribe(_ => _screenViewModel.OnCloseAdvertisementButtonClicked(this))
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}