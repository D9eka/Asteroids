using System;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.SaveService;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Advertisement
{
    public class TestAdvertisementService : IAdvertisementService, IInitializable, IDisposable
    {
        private readonly Subject<bool> _rewardGranted = new Subject<bool>();
        
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly AdTracker _adTracker;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

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
            _gameplaySessionManager.GameStarted.Subscribe(_ => CanRevive = true)
                .AddTo(_disposables);
            _adTracker.IsAdFree.Subscribe(isAdFree => _skipAd = isAdFree)
                .AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
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