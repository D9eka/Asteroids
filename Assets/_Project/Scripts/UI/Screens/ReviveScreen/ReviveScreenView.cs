using Asteroids.Scripts.UI.Buttons;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.ReviveScreen
{
    public class ReviveScreenView : BaseView
    {
        [SerializeField] private TMP_Text _header;
        [SerializeField] private ButtonWithCountdown _reviveButton;
        [SerializeField] private Button _exitButton;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private ReviveScreenViewModel _screenViewModel;

        [Inject]
        public void Construct(ReviveScreenViewModel screenViewModel)
        {
            _screenViewModel = screenViewModel;
        }

        protected override void Awake()
        {
            base.Awake();
            
            _screenViewModel.ScreenTitle
                .Subscribe(title => _header.text = title)
                .AddTo(_disposables);
            
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