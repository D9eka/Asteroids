using System;
using Asteroids.Scripts.Advertisement;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.SaveService;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.ReviveScreen
{
    public class ReviveScreenViewModel : IInitializable, IDisposable
    {
        private const string REVIVE_FOR_AD_TEXT = "Watch AD and revive!";
        private const string REVIVE_FOR_FREE = "You can revive for FREE!";
        
        private readonly IGameStateController _gameStateController;
        private readonly IUIController _uiController;
        private readonly AdTracker _adTracker;
        private readonly IAdvertisementService _advertisementService;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private bool _interstitialAdShowed;
        
        public ReactiveProperty<string> ScreenTitle { get; } = new ReactiveProperty<string>(REVIVE_FOR_FREE);

        public ReviveScreenViewModel(IGameStateController gameStateController, IUIController uiController, 
            AdTracker adTracker, IAdvertisementService advertisementService)
        {
            _gameStateController = gameStateController;
            _uiController = uiController;
            _adTracker = adTracker;
            _advertisementService = advertisementService;
        }
        
        public void Initialize()
        {
            _adTracker.IsAdFree.Subscribe(isAdFree => ScreenTitle.Value = isAdFree ? REVIVE_FOR_FREE : REVIVE_FOR_AD_TEXT);
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