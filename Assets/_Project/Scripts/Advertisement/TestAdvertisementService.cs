using System;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.SaveService;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Advertisement
{
    public class TestAdvertisementService : IAdvertisementService, IInitializable
    {
        private readonly Subject<bool> _rewardGranted = new Subject<bool>();
        
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly AdTracker _adTracker;

        private bool _skipAd;
        
        public bool CanRevive { get; private set; } = true;
        public IObservable<bool> RevivalRewardGranted => _rewardGranted;
        
        public TestAdvertisementService(IGameplaySessionManager gameplaySessionManager, AdTracker adTracker)
        {
            _gameplaySessionManager = gameplaySessionManager;
            _adTracker = adTracker;
        }
        
        public void Initialize()
        {
            _gameplaySessionManager.GameStarted.Subscribe(_ => CanRevive = true);
            _adTracker.IsAdFree.Subscribe(isAdFree => _skipAd = isAdFree);
        }
        
        public void ShowInterstitialAd()
        {
            Debug.Log("Show Interstitial Ad");
        }

        public void ShowRevivalAd(Action<bool> onComplete)
        {
            if (_skipAd)
            {
                Debug.Log("Ad skipped, Reward granted");
            }
            else
            {
                Debug.Log("Reward granted");
            }
            _rewardGranted.OnNext(true);
            onComplete?.Invoke(true);
            CanRevive = false;
        }
    }
}