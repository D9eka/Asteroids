using System;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.SaveService;
using UniRx;
using Unity.Services.LevelPlay;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Advertisement.LevelPlayAd
{
    public class LPlayAdvertisementService : IAdvertisementService, IInitializable, IDisposable
    {
        private Subject<bool> _revivalRewardGranted = new();
        
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly AdTracker _adTracker;
        private readonly string _appKey;
        private readonly string _interstitialAdId;
        private readonly string _revivalAdId;
        
        private bool _isReady;
        private bool _skipAd;
        private LPlayInterstitialAdvertisement _interstitialAd;
        private LPlayRewardedAdvertisement _revivalAd;
        
        public IObservable<bool> RevivalRewardGranted => _revivalRewardGranted;
        public bool CanRevive => _revivalAd.CanGiveReward();

        public LPlayAdvertisementService(IGameplaySessionManager gameplaySessionManager, 
            AdTracker adTracker, string appKey, string interstitialAdId, string revivalAdId)
        {
            _gameplaySessionManager = gameplaySessionManager;
            _adTracker = adTracker;
            _appKey = appKey;
            _interstitialAdId = interstitialAdId;
            _revivalAdId = revivalAdId;
        }

        public void Initialize()
        {
            LevelPlay.Init(_appKey);
            LevelPlay.OnInitSuccess += LevelPlayOnInitSuccess; 
            LevelPlay.OnInitFailed += LevelPlayOnInitFailed;

            _gameplaySessionManager.GameStarted.Subscribe(_ => _revivalAd.Reset());
            _adTracker.IsAdFree.Subscribe(isAdFree => _skipAd = isAdFree);
        }

        public void Dispose()
        {
            LevelPlay.OnInitSuccess -= LevelPlayOnInitSuccess;
            LevelPlay.OnInitFailed -= LevelPlayOnInitFailed;
        }

        public void ShowInterstitialAd()
        {
            if (_skipAd) return;
            if (!_isReady || !_interstitialAd.IsLoaded) return;
            _interstitialAd.Show();
        }

        public void ShowRevivalAd(Action<bool> onComplete)
        {
            if (_skipAd)
            {
                onComplete?.Invoke(true);
                return;
            }
            if (!_isReady || !_revivalAd.IsLoaded) return;
            _revivalAd.Show(onComplete);
        }

        private void LevelPlayOnInitFailed(LevelPlayInitError obj)
        {
            Debug.LogError($"LevelPlayOnInitFailed: {obj}");
        }

        private void LevelPlayOnInitSuccess(LevelPlayConfiguration obj)
        {
            Debug.Log("LevelPlay Init Success!");
            _isReady = true;
            CreateAds();
        }

        private void CreateAds()
        {
            _interstitialAd = new LPlayInterstitialAdvertisement(_interstitialAdId);
            _interstitialAd.Initialize();
            
            _revivalAd = new LPlayRewardedAdvertisement(_revivalAdId);
            _revivalAd.Initialize();
            _revivalAd.RewardGranted.Subscribe(isRewarded => _revivalRewardGranted.OnNext(isRewarded));
        }
    }
}