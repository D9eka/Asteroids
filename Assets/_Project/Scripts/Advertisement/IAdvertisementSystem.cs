using System;

namespace _Project.Scripts.Advertisement
{
    public interface IAdvertisementSystem
    {
        public IObservable<bool> RevivalRewardGranted { get; }
        
        public bool CanRevive { get; }
        
        public void ShowInterstitialAd();
        public void ShowRevivalAd();
    }
}