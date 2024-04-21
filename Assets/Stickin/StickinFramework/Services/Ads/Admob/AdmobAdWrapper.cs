using System;
using UnityEngine;

namespace stickin
{
    public class AdmobAdWrapper : AdWrapper
    {
        [SerializeField] private AdmobAdsConfig _config;

        private AdmobBannerWrapper _bannerWrapper;
        private AdmobInterstitialWrapper _interstitialWrapper;
        private AdmobRewardWrapper _rewardWrapper;
        
        private bool _isInterstitialAvailable;

        public override void Init()
        {
            AdmobConfiguration.Init(_config.IsTest, _config.TestDevices);

            _bannerWrapper = new AdmobBannerWrapper(
                _config.Ids.BannerId, 
                _config.BannerPosition == BannerPosition.Top,
                _config.ShowBannerOnStart, 
                OnInitComplete);

            _interstitialWrapper = new AdmobInterstitialWrapper(_config.Ids.InterstitialId);
            _rewardWrapper = new AdmobRewardWrapper(_config.Ids.RewardId);
            
            InterstitialAvailableDelay(_config.StartDelaySeconds);
        }
        
        private void OnInitComplete()
        {
            
        }

        public override bool IsInterstitialAvailable()
        {
            return _isInterstitialAvailable && _interstitialWrapper.IsLoad();
        }

        public override void ShowInterstitial()
        {
            _interstitialWrapper.Show(null, null);
            InterstitialAvailableDelay(_config.DelayBetweenAdsSeconds);
        }

        public override bool IsRewardAvailable()
        {
            return _rewardWrapper.IsLoad();
        }

        public override void ShowReward(Action callbackComplete, Action callbackFail)
        {
            _rewardWrapper.Show(callbackComplete, callbackFail);
            
            if (_config.RewardAdsIsRestartDelay)
                InterstitialAvailableDelay(_config.DelayBetweenAdsSeconds);
        }

        public override bool IsBannerAvailable()
        {
            return _bannerWrapper.IsLoad();
        }

        public override void ShowBanner()
        {
            _bannerWrapper.Show(null, null);
        }

        public override void HideBanner()
        {
            _bannerWrapper.Hide();
        }

        public override float GetBannerHeight()
        {
            return _bannerWrapper.GetBannerHeight();
        }

        public override void CheckAvailableAd()
        {
            if (!_bannerWrapper.IsLoad())
                _bannerWrapper.Request();
        
            if (!_interstitialWrapper.IsLoad())
                _interstitialWrapper.Request();
        
            if (!_rewardWrapper.IsLoad())
                _rewardWrapper.Request();
        }
        
        private void InterstitialAvailableDelay(float delay)
        {
            _isInterstitialAvailable = false;
            Debug.Log($"InterstitialAvailableDelay {_isInterstitialAvailable}     with delay = {delay}");

            if (delay > 0)
            {
                Updater.Instance.RemoveDelayedCall(InterstitialAvailable);
                Updater.Instance.AddDelayedCall(delay, InterstitialAvailable);
            }
            else
            {
                InterstitialAvailable();
            }
        }
        
        private void InterstitialAvailable()
        {
            _isInterstitialAvailable = true;
        }
    }
}