using System;
using UniRx;
using Unity.Services.LevelPlay;
using UnityEngine;

namespace _Project.Scripts.Advertisement
{
    public class LPlayRewardedAdvertisement : IDisposable
    {
        protected readonly string Id;
        private readonly Subject<bool> _rewardGranted = new Subject<bool>();

        private LevelPlayRewardedAd _ad;
        private bool _isRewardGiven;
        
        public bool IsLoaded { get; private set; }
        public IObservable<bool> RewardGranted => _rewardGranted;
        
        public LPlayRewardedAdvertisement(string id)
        {
            Id = id;
        }

        public void Initialize()
        {
            _ad = new LevelPlayRewardedAd(Id);
            _ad.OnAdLoaded += AdOnAdLoaded;
            _ad.OnAdLoadFailed += AdOnAdLoadFailed;
            _ad.OnAdClosed += AdOnAdClosed;
            _ad.OnAdRewarded += AdOnAdRewarded;
            _ad.OnAdDisplayFailed += AdOnAdDisplayFailed;

            _ad.LoadAd();
        }

        public bool CanGiveReward()
        {
            return IsLoaded && !_isRewardGiven;
        }

        public void Show()
        {
            if (!CanGiveReward()) return;
            IsLoaded = false;
            _ad.ShowAd();
        }

        public void Reset()
        {
            _isRewardGiven = false;
        }

        public void Dispose()
        {
            _ad.OnAdLoaded -= AdOnAdLoaded;
            _ad.OnAdLoadFailed -= AdOnAdLoadFailed;
            _ad.OnAdClosed -= AdOnAdClosed;
        }

        private void AdOnAdLoaded(LevelPlayAdInfo obj)
        {
            IsLoaded = true;
        }

        private void AdOnAdLoadFailed(LevelPlayAdError obj)
        {
            Debug.LogError(obj.ErrorMessage);
        }

        private void AdOnAdClosed(LevelPlayAdInfo obj)
        {
            _ad.LoadAd();
        }

        private void AdOnAdRewarded(LevelPlayAdInfo arg1, LevelPlayReward arg2)
        {
            _rewardGranted.OnNext(true);
            _isRewardGiven = true;
            _ad.LoadAd();
        }

        private void AdOnAdDisplayFailed(LevelPlayAdInfo arg1, LevelPlayAdError arg2)
        {
            _rewardGranted.OnNext(false);
            _ad.LoadAd();
        }
    }
}