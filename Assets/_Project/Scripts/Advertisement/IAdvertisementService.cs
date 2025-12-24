using System;

namespace Asteroids.Scripts.Advertisement
{
    public interface IAdvertisementService
    {
        public IObservable<bool> RevivalRewardGranted { get; }
        
        public bool CanRevive { get; }
        
        public void ShowInterstitialAd();
        public void ShowRevivalAd(Action<bool> onComplete);
    }
}