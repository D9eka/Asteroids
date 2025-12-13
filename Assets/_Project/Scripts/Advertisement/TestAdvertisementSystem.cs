using System;
using Asteroids.Scripts.GameState.GameplaySession;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Advertisement
{
    public class TestAdvertisementSystem : IAdvertisementSystem
    {
        private readonly Subject<bool> _rewardGranted = new Subject<bool>();
        
        private readonly IGameplaySessionManager _gameplaySessionManager;
        
        public bool CanRevive { get; private set; } = true;
        public IObservable<bool> RevivalRewardGranted => _rewardGranted;
        
        public TestAdvertisementSystem(IGameplaySessionManager gameplaySessionManager)
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

        public void ShowRevivalAd()
        {
            Debug.Log("Reward granted");
            _rewardGranted.OnNext(true);
            CanRevive = false;
        }
    }
}