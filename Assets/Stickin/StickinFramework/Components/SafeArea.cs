using UnityEngine;

namespace stickin
{
    public class SafeArea : MonoBehaviour
    {

        #region Serialized Fields

        [SerializeField] private bool _needIgnoreX;
        [SerializeField] private bool _needIgnoreY;
        [SerializeField] private bool _ignoreTopIfIPhone;
        [SerializeField] private float _topBorderIfIgnoreTopForIOS = 50;

        #endregion

        [InjectField] private AdsService _adsService;

        #region Private Properties

        private RectTransform _rt;
        private Rect _lastSafeArea = Rect.zero;
        private static float _batteryBorder = 0;

        #endregion

        public static Rect Rect;

        #region Private Methods

        private void Start()
        {
            InjectService.BindFields(this);
            
            _rt = GetComponent<RectTransform>();
            Refresh();
            
            if (_adsService != null)
                _adsService.OnRefreshBanner += OnRefreshBanner;
        }

        private void OnDestroy()
        {
            if (_adsService != null)
                _adsService.OnRefreshBanner -= OnRefreshBanner;
        }

        private void Refresh()
        {
            Rect safeArea = Screen.safeArea;

            if (safeArea != _lastSafeArea)
                ApplySafeArea(safeArea);
        }

        private void ApplySafeArea(Rect r)
        {
            _lastSafeArea = r;
            // Debug.LogError($"ApplySafeArea r = {r}");
            if (_needIgnoreX)
                r.x = 0;

            if (_needIgnoreY)
                r.y = 0;

#if UNITY_IPHONE || UNITY_IOS || UNITY_EDITOR
            if (_ignoreTopIfIPhone && r.y > 50)
                r.height = Screen.height - r.y - _topBorderIfIgnoreTopForIOS - _batteryBorder;
#endif

            Debug.Log($"SafeArea.ApplySafeArea: ScreenSize w = {Screen.width}    h = {Screen.height}");
            
#if ST_ADS
            var bannerHeight = _adsService != null ? _adsService.GetBannerHeight() : 0;
            bannerHeight /= Screen.height / _rt.rect.height; // / _rt.lossyScale.x;
            r.height -= bannerHeight;
            r.position = new Vector2(r.position.x, r.position.y + (_adsService != null && _adsService.BannerIsTop ? 0 : bannerHeight));
#endif
            Rect = r;

            var screenSize = new Vector2(Screen.width, Screen.height);

            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= screenSize.x;
            anchorMin.y /= screenSize.y;
            anchorMax.x /= screenSize.x;
            anchorMax.y /= screenSize.y;

            _rt.anchorMin = anchorMin;
            _rt.anchorMax = anchorMax;
        }

        private void OnRefreshBanner()
        {
            ApplySafeArea(Screen.safeArea);
        }

        #endregion

        public static void SetShowBattery(bool isShow)
        {
            _batteryBorder = isShow ? 20 : 0;

            var safeAreas = GameObject.FindObjectsOfType<SafeArea>();
            foreach (var safeArea in safeAreas)
            {
                if (safeArea != null)
                    safeArea.Refresh();
            }
        }
    }
}