using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class AdmobIdsConfig
    {
        public string AppId;
        public string BannerId;
        public string InterstitialId;
        public string RewardId;
    }

    public enum BannerPosition
    {
        Bottom,
        Top
    }
    
    [CreateAssetMenu(fileName = "AdmobAdsConfig", menuName = "Stickin/Ads/Admob config")]
    public class AdmobAdsConfig : ScriptableObject
    {
        [Header("Ids")]
        [SerializeField] private AdmobIdsConfig _ios;
        [SerializeField] private AdmobIdsConfig _android;

        [Header("Params")] 
        [Header("Banner")] 
        public bool ShowBannerOnStart;
        public BannerPosition BannerPosition;
        
        [Header("Interstitial")] 
        public int StartDelaySeconds;
        public int DelayBetweenAdsSeconds;
        
        [Header("Reward")] 
        public bool RewardAdsIsRestartDelay;

        [Header("For test")]
        public bool IsTest;
        public List<string> TestDevices;

        // for editor
        public AdmobIdsConfig IOS => _ios;
        public AdmobIdsConfig Android => _android;

#if UNITY_ANDROID
        public AdmobIdsConfig Ids => _android;
#else
        public AdmobIdsConfig Ids => _ios;
#endif
    }
}