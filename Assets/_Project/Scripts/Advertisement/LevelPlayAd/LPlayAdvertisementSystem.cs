using System;
using Asteroids.Scripts.GameState.GameplaySession;
using UniRx;
using Unity.Services.LevelPlay;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Advertisement
{
    public class LPlayAdvertisementSystem : IAdvertisementSystem, IInitializable, IDisposable
    {
        private Subject<bool> _revivalRewardGranted = new();
        
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly string _appKey;
        private readonly string _interstitialAdId;
        private readonly string _revivalAdId;
        
        private bool _isReady;
        private LPlayInterstitialAdvertisement _interstitialAd;
        private LPlayRewardedAdvertisement _revivalAd;
        
        public IObservable<bool> RevivalRewardGranted => _revivalRewardGranted;
        public bool CanRevive => _revivalAd.CanGiveReward();

        public LPlayAdvertisementSystem(IGameplaySessionManager gameplaySessionManager,
            string appKey, string interstitialAdId, string revivalAdId)
        {
            _gameplaySessionManager = gameplaySessionManager;
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
        }

        public void Dispose()
        {
            LevelPlay.OnInitSuccess -= LevelPlayOnInitSuccess;
            LevelPlay.OnInitFailed -= LevelPlayOnInitFailed;
        }

        public void ShowInterstitialAd()
        {
            if (!_isReady || !_interstitialAd.IsLoaded) return;
            _interstitialAd.Show();
        }

        public void ShowRevivalAd()
        {
            if (!_isReady || !_revivalAd.IsLoaded) return;
            _revivalAd.Show();
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