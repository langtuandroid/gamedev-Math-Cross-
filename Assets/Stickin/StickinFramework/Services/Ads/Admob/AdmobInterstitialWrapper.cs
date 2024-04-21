using System;
using UnityEngine;

#if ST_ADS
using GoogleMobileAds.Api;
#endif

namespace stickin
{
    public class AdmobInterstitialWrapper
    {
#if ST_ADS
        private readonly string _id;

        private InterstitialAd _interstitialView;

        public AdmobInterstitialWrapper(string id)
        {
            _id = id;

            // LoadInterstitialAd();
            Request();
        }

        public bool IsLoad()
        {
            return _interstitialView != null && _interstitialView.CanShowAd();
        }

        public bool Request()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialView != null)
            {
                _interstitialView.Destroy();
                _interstitialView = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            // send the request to load the ad.
            InterstitialAd.Load(_id, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError($"interstitial ad failed to load an ad with error : {error}");
                        return;
                    }

                    Debug.Log($"Interstitial ad loaded with response : {ad.GetResponseInfo()}");

                    _interstitialView = ad;

                    ad.OnAdFullScreenContentClosed += () => { Request(); };
                    // Raised when the ad failed to open full screen content.
                    ad.OnAdFullScreenContentFailed += (AdError error) => { Request(); };
                });

            return true;
        }

        public void Show(Action callbackComplete, Action callbackFail)
        {
            if (_interstitialView != null && _interstitialView.CanShowAd())
            {
                Debug.Log("AdmobInterstitialWrapper.Show: OK");

                _interstitialView.Show();
                // Request();
            }
            else
            {
                Debug.LogError("InterstitialAdHelper.Show: not loaded");
            }
        }

        public void Hide()
        {
        }

        private void Clear()
        {
            if (_interstitialView != null)
                _interstitialView = null;
        }

        #region Callbacks

        public void OnAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("AdmobInterstitialWrapper.OnAdLoaded");
        }

        public void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.LogError($"AdmobInterstitialWrapper.OnAdLoaded: {args.LoadAdError.GetMessage()}");
            Clear();
        }

        public void OnAdOpening(object sender, EventArgs args)
        {
            Debug.Log("AdmobInterstitialWrapper.OnAdOpening");
        }

        public void OnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("AdmobInterstitialWrapper.OnAdClosed");
        }

        public void Destroy()
        {
        }

        #endregion

#else
    public AdmobInterstitialWrapper(string id) { }
    public bool IsLoad() => false;
    public void Show(object o, object o1) { }
    public void Request() { }
#endif
    }
}