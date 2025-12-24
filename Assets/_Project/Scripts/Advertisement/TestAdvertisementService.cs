using System;
using Asteroids.Scripts.GameState.GameplaySession;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Advertisement
{
    public class TestAdvertisementService : IAdvertisementService
    {
        private readonly Subject<bool> _rewardGranted = new Subject<bool>();
        
        private readonly IGameplaySessionManager _gameplaySessionManager;
        
        public bool CanRevive { get; private set; } = true;
        public IObservable<bool> RevivalRewardGranted => _rewardGranted;
        
        public TestAdvertisementService(IGameplaySessionManager gameplaySessionManager)
        {
            _gameplaySessionManager = gameplaySessionManager;
        }
        
        public void Initialize()
        {
            _gameplaySessionManager.GameStarted.Subscribe(_ => CanRevive = true);
        }
        
        public void ShowInterstitialAd()
        {
            Debug.Log("Show Interstitial Ad");
        }

        public void ShowRevivalAd(Action<bool> onComplete)
        {
            Debug.Log("Reward granted");
            _rewardGranted.OnNext(true);
            onComplete?.Invoke(true);
            CanRevive = false;
        }
    }
}