using System;
using UnityEngine;

#if ST_ADS
using GoogleMobileAds.Api;
#endif

namespace stickin
{
    public class AdmobBannerWrapper
    {
#if ST_ADS
        private readonly string _id;

        private BannerView _bannerView;
        private bool _bannerIsLoaded;
        private bool _isBannerShow;
        private Action _onInitComplete;

        public BannerPositionType Position { get; set; }
        public event Action OnRefresh;

        public AdmobBannerWrapper(string id, bool isTop, bool showInStart, Action onInitComplete)
        {
            _id = id;
            Position = isTop ? BannerPositionType.TOP : BannerPositionType.BOTTOM;
            _onInitComplete = onInitComplete;

            Request();
        }

        public bool IsLoad()
        {
            return _bannerIsLoaded;
        }

        public bool Request()
        {
            Clear();

            _bannerView = new BannerView(_id, AdSize.SmartBanner,
                Position == BannerPositionType.TOP ? AdPosition.Top : AdPosition.Bottom);
            PositionTo(Position);

            _bannerView.OnBannerAdLoaded += HandlerOnAdLoaded;
            _bannerView.OnBannerAdLoadFailed += HandlerOnAdFailedToLoad;
            _bannerView.OnAdFullScreenContentOpened += OnAdOpening;
            _bannerView.OnAdFullScreenContentClosed += OnAdClosed;

            _isBannerShow = true;
            _bannerView.LoadAd(new AdRequest());
            // _bannerView.Hide();

            return true;
        }

        private void Clear()
        {
            if (_bannerView != null)
            {
                _bannerView.OnBannerAdLoaded -= HandlerOnAdLoaded;
                _bannerView.OnBannerAdLoadFailed -= HandlerOnAdFailedToLoad;
                _bannerView.OnAdFullScreenContentOpened -= OnAdOpening;
                _bannerView.OnAdFullScreenContentClosed -= OnAdClosed;

                _bannerView.Destroy();
                _bannerView = null;
            }
        }

        public void Show(Action callbackComplete, Action callbackFail)
        {
            if (_bannerView != null && _bannerIsLoaded && !_isBannerShow)
            {
                _bannerView.Show();
                _isBannerShow = true;

                callbackComplete?.Invoke();
            }
            else
                callbackFail?.Invoke();

            CallRefreshCallback();
        }

        public void Hide()
        {
            if (_bannerView != null)
                _bannerView.Hide();

            _isBannerShow = false;

            CallRefreshCallback();
        }

        public void PositionTo(BannerPositionType position)
        {
            Position = position;

            if (_bannerView != null)
                _bannerView.SetPosition(position == BannerPositionType.TOP ? AdPosition.Top : AdPosition.Bottom);
        }

        public bool IsShow()
        {
            return _isBannerShow;
        }

        public float GetHeight()
        {
            return _bannerView != null && _bannerIsLoaded && _isBannerShow ? _bannerView.GetHeightInPixels() : 0;
        }

        protected void CallRefreshCallback()
        {
            OnRefresh?.Invoke();
        }

        public float GetBannerHeight()
        {
            if (_bannerView != null)
                _bannerView.GetHeightInPixels();

            return 0;
        }

        #region Callbacks

        private void HandlerOnAdLoaded()
        {
            Debug.Log("BannerAdHelper.HandlerOnAdLoaded: banner is loaded");
            _bannerIsLoaded = true;

            _onInitComplete?.Invoke();
        }

        private void HandlerOnAdFailedToLoad(LoadAdError error)
        {
            Debug.LogError($"BannerAdHelper.HandlerOnAdFailedToLoad eror = {error.GetMessage()}");
            _bannerView = null;
            CallRefreshCallback();
        }

        private void OnAdOpening()
        {
            Debug.Log("BannerAdHelper.OnAdOpening");
        }

        private void OnAdClosed()
        {
            Debug.Log("BannerAdHelper.OnAdClosed");
        }

        public void Destroy()
        {
            Hide();
            _bannerView.Destroy();
        }

        #endregion

#else
    public AdmobBannerWrapper(string id, bool isTop, bool showInStart, Action onInitComplete) => PrintErrorMessage();
    public bool IsLoad() => false;
    public void Show(object o, object o1) { }
    public void Hide() { }
    public float GetBannerHeight() => 0;
    public void Request() { }

    private void PrintErrorMessage()
    {
        Debug.LogError("Added Scripting Define Symbol 'ST_ADS' for enabled Admob.");
    }
#endif
    }
}