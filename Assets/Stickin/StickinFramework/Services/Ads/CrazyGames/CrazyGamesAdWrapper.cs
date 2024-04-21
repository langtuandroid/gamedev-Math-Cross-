using System;
using UnityEngine;

#if ST_CRAZY_ADS
using CrazyGames;
#endif

namespace stickin
{
    public class CrazyGamesAdWrapper : AdWrapper
    {
#if ST_CRAZY_ADS
        [SerializeField] private CrazyBanner _banner;
        
        public override void Init()
        {
            CrazySDK.Instance.IsQaTool(isQaTool => { Debug.Log("Is QA Tool: " + isQaTool); });
        }

        public override bool IsInterstitialAvailable()
        {
            return true;
        }

        public override void ShowInterstitial()
        {
            CrazyAds.Instance.beginAdBreak(); // or not reward
        }

        public override bool IsRewardAvailable()
        {
            return true;
        }

        public override void ShowReward(Action callbackComplete, Action callbackFail)
        {
            CrazyAds.AdBreakCompletedCallback completedCallback = () => callbackComplete?.Invoke();
            CrazyAds.AdErrorCallback failCallback = () => callbackFail?.Invoke();
            
            CrazyAds.Instance.beginAdBreakRewarded(completedCallback, failCallback);
        }

        public override bool IsBannerAvailable()
        {
            return true;
        }

        public override void ShowBanner()
        {
            _banner.gameObject.SetActive(true);
            _banner.MarkVisible(true);
            CrazyAds.Instance.updateBannersDisplay();
        }

        public override void HideBanner()
        {
            _banner.MarkVisible(false);
        }

        public override float GetBannerHeight()
        {
            return 0;
        }

        public override void CheckAvailableAd()
        {
            
        }
#else
        public override void Init() => Debug.LogError("Need added define ST_CRAZY_ADS");
        public override bool IsInterstitialAvailable() => false;
        public override void ShowInterstitial() { }
        public override bool IsRewardAvailable() => false;
        public override void ShowReward(Action callbackComplete, Action callbackFail) { }
        public override bool IsBannerAvailable() => false;
        public override void ShowBanner() { }
        public override void HideBanner() { }
        public override float GetBannerHeight() => 0;
        public override void CheckAvailableAd() { }
#endif
    }
}