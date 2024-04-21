using System;
using UnityEngine;

namespace stickin
{
    public abstract class AdWrapper : MonoBehaviour
    {
        public abstract void Init();
        public abstract bool IsInterstitialAvailable();
        public abstract void ShowInterstitial();
        public abstract bool IsRewardAvailable();
        public abstract void ShowReward(Action callbackComplete, Action callbackFail);
        public abstract bool IsBannerAvailable();
        public abstract void ShowBanner();
        public abstract void HideBanner();
        public abstract float GetBannerHeight();
        public abstract void CheckAvailableAd();
    }
}