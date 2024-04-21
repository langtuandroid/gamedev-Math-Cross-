using System;
using UnityEngine;
// using UnityEngine.Advertisements;

namespace stickin
{
    public class UnityAdWrapper : AdWrapper
    {
        [Header("Unity settgins")]
        [SerializeField] private string _iosGameId;
        [SerializeField] private string _androidGameId;
        [SerializeField] private bool _testMode;

#if UNITY_ANDROID
        private string _gameId => _androidGameId;
#else
        private string _gameId => _iosGameId;
#endif
        
        public override void Init()
        {
            // Advertisement.Initialize (_gameId, _testMode);
        }

        public override bool IsInterstitialAvailable()
        {
            throw new NotImplementedException();
        }

        public override void ShowInterstitial()
        {
            throw new NotImplementedException();
        }

        public override bool IsRewardAvailable()
        {
            throw new NotImplementedException();
        }

        public override void ShowReward(Action callbackComplete, Action callbackFail)
        {
            throw new NotImplementedException();
        }

        public override bool IsBannerAvailable()
        {
            throw new NotImplementedException();
        }

        public override void ShowBanner()
        {
            throw new NotImplementedException();
        }

        public override void HideBanner()
        {
            throw new NotImplementedException();
        }

        public override float GetBannerHeight()
        {
            throw new NotImplementedException();
        }

        public override void CheckAvailableAd()
        {
            throw new NotImplementedException();
        }
    }
}