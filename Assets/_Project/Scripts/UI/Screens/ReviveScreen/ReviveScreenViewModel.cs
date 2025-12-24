using System;
using Asteroids.Scripts.Advertisement;
using Asteroids.Scripts.GameState;
using UniRx;

namespace Asteroids.Scripts.UI.Screens.ReviveScreen
{
    public class ReviveScreenViewModel : IDisposable
    {
        private readonly IGameStateController _gameStateController;
        private readonly IUIController _uiController;
        private readonly IAdvertisementService _advertisementService;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private bool _interstitialAdShowed;

        public ReviveScreenViewModel(IGameStateController gameStateController, IUIController uiController, 
            IAdvertisementService advertisementService)
        {
            _gameStateController = gameStateController;
            _uiController = uiController;
            _advertisementService = advertisementService;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void OnAdvertisementButtonClicked()
        {
            _advertisementService.ShowRevivalAd(AdvertisementSystemOnRevivalAdRewardGranted);
        }

        public void OnCloseAdvertisementButtonClicked(IView view)
        {
            if (!_interstitialAdShowed)
            {
                _interstitialAdShowed = true;
                _advertisementService.ShowInterstitialAd();
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