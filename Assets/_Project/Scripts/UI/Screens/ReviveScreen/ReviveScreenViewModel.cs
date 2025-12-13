using System;
using _Project.Scripts.Advertisement;
using Asteroids.Scripts.GameState;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.EndGameScreen
{
    public class ReviveScreenViewModel : IInitializable, IDisposable
    {
        private readonly IGameStateController _gameStateController;
        private readonly IUIController _uiController;
        private readonly IAdvertisementSystem _advertisementSystem;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private bool _interstitialAdShowed;

        public ReviveScreenViewModel(IGameStateController gameStateController, IUIController uiController, 
            IAdvertisementSystem advertisementSystem)
        {
            _gameStateController = gameStateController;
            _uiController = uiController;
            _advertisementSystem = advertisementSystem;
        }

        public void Initialize()
        {
            _advertisementSystem.RevivalRewardGranted.Subscribe(AdvertisementSystemOnRevivalAdRewardGranted);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void OnAdvertisementButtonClicked()
        {
            _advertisementSystem.ShowRevivalAd();
        }

        public void OnCloseAdvertisementButtonClicked(IView view)
        {
            if (!_interstitialAdShowed)
            {
                _interstitialAdShowed = true;
                _advertisementSystem.ShowInterstitialAd();
            }
            _uiController.CloseScreen(view);
        }

        private void AdvertisementSystemOnRevivalAdRewardGranted(bool isGranted)
        {
            if (isGranted)
            {
                _gameStateController.HandleRevivalRequest();
            }
        }
    }
}