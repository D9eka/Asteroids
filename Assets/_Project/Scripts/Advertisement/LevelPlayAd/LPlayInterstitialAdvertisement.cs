using System;
using Unity.Services.LevelPlay;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Advertisement
{
    public class LPlayInterstitialAdvertisement : IInitializable, IDisposable
    {
        protected readonly string Id;

        private LevelPlayInterstitialAd _ad;
        
        public bool IsLoaded { get; private set; }
        
        public LPlayInterstitialAdvertisement(string id)
        {
            Id = id;
        }

        public void Initialize()
        {
            _ad = new LevelPlayInterstitialAd(Id);
            _ad.OnAdLoaded += AdOnAdLoaded;
            _ad.OnAdLoadFailed += AdOnAdLoadFailed;
            _ad.OnAdClosed += AdOnAdClosed;

            _ad.LoadAd();
        }

        public void Show()
        {
            if (!IsLoaded) return;
            IsLoaded = false;
            _ad.ShowAd();
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
            IsLoaded = false;
            Debug.LogError(obj.ErrorMessage);
        }

        private void AdOnAdClosed(LevelPlayAdInfo obj)
        {
            _ad.LoadAd();
        }
    }
}