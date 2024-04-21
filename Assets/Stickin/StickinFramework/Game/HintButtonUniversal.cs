using stickin;
using stickin.menus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Button))]
    public class HintButtonUniversal : MonoBehaviour
    {
        [System.Serializable]
        public struct HintEventData
        {
            public HintSO HintSO;
            public FailReason FailReason;

            public HintEventData(HintSO so, FailReason reason)
            {
                HintSO = so;
                FailReason = reason;
            }
        }
        
        public enum FailReason
        {
            Unknown,
            NoAds,
            NoCoins
        }
        
        [SerializeField] private HintSO _hintSo;
        [Header("Parents")]
        [SerializeField] private GameObject _rewardParent;
        [SerializeField] private GameObject _priceParent;
        [SerializeField] private GameObject _countParent;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Header("Texts")]
        [SerializeField] private TextResourceValue _countText;
        [SerializeField] private Text _priceText;
        
        [Header("Icon")]
        [SerializeField] private Image _iconImage;

        [SerializeField] private float _fadeDisabled = 0.5f;

        [InjectField] private ResourcesService _resourcesService;
        [InjectField] private AdsService _adsService;

        private Button _btn;
        private Game _game;
        private int _countAvailable;
        private GameView _gameView;
        private int _count => _resourcesService != null ? _resourcesService.GetResourceValueInt(_hintSo.ResourceId) : 0;

        [SerializeField] private UnityEvent<HintEventData> OnUseHintFail;
        // public event Action<HintSO, FailReason> OnUseHintFail;

        public void Init(HintSO hintSo, Game game)
        {
            _countAvailable = hintSo.CountInOneGame;
            
            _game = game;
            _game.OnLockedTouch += OnLockedTouch;
            
            _hintSo = hintSo;
            
            if (hintSo.Icon != null && _iconImage != null)
                _iconImage.sprite = hintSo.Icon;

            RefreshCount();
            RefreshTypeAd();
            RefreshTypeCoin();
            
            if (_countText != null)
                _countText.Init(_hintSo.ResourceId);
        }

        private void OnLockedTouch(bool locked)
        {
            RefreshEnabled();
        }

        private void OnDestroy()
        {
            if (_game != null)
                _game.OnLockedTouch -= OnLockedTouch;
            
            if (_resourcesService != null)
                _resourcesService.OnUserUpdate -= OnUserUpdate;
        }

        private void Awake()
        {
            InjectService.BindFields(this);
            
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);

            if (_hintSo != null)
            {
                _gameView = FindAnyObjectByType<GameView>();
                if (_gameView.IsInitComplete)
                    OnInitGameComplete();
                else
                    _gameView.OnInitComplete += OnInitGameComplete;
            }

            if (_resourcesService != null)
                _resourcesService.OnUserUpdate += OnUserUpdate;
        }

        private void OnUserUpdate(string id, double _, Transform __)
        {
            if (_hintSo.ResourceId == id)
            {
                RefreshCount();
                RefreshTypeAd();
                RefreshTypeCoin();
            }
        }

        private void OnInitGameComplete()
        {
            Init(_hintSo, _gameView.Game);
        }

        private void OnClick()
        {
            if (_countAvailable > 0 &&
                _game.CanUseHint(_hintSo.LogicClass))
            {
                if (_hintSo.PriceType == HintPriceType.Ad)
                {
                    if (_adsService.IsRewardAvailable())
                        _adsService.ShowReward(UseHint);
                    else
                        FailHint(FailReason.NoAds);
                }
                else if (_hintSo.PriceType == HintPriceType.Coins)
                {
                    if (_count > 0)
                    {
                        _resourcesService.ChangeResource(_hintSo.ResourceId, -1);
                        _game.UseHint(_hintSo.LogicClass);
                    }
                    else
                    {
                        var coins = _resourcesService.GetResourceValueInt("coin");
                        if (coins > _hintSo.Price)
                        {
                            _resourcesService.ChangeResource("coin", -_hintSo.Price);
                            _game.UseHint(_hintSo.LogicClass);
                        }
                        else
                        {
                            FailHint(FailReason.NoCoins);
                        }
                    }
                }
                else
                {
                    Debug.LogError("HintButtonUniversal: Unsupported HintPriceType");
                }

                RefreshEnabled();
            }
        }

        private void UseHint()
        {
            if (_game.UseHint(_hintSo.LogicClass))
            {
                _countAvailable--;
                RefreshEnabled();
            }
        }

        private void RefreshCount()
        {
            var count = _resourcesService.GetResourceValue(_hintSo.ResourceId);

            if (_countParent != null)
            {
                _countParent.SetActive(count > 0);
                // _countText.text = count.ToString();
            }
        }

        private void RefreshTypeAd()
        {
            if (_rewardParent != null)
                _rewardParent.SetActive(_countParent != null && 
                                        !_countParent.activeSelf && 
                                        _hintSo.PriceType == HintPriceType.Ad);
        }
        
        private void RefreshTypeCoin()
        {
            if (_priceParent != null)
            {
                _priceParent.SetActive(_countParent != null && 
                                       !_countParent.activeSelf && 
                                       _hintSo.PriceType == HintPriceType.Coins);
                
                if (_priceText != null && _priceParent.activeSelf)
                    _priceText.text = _hintSo.Price.ToString();
            }
        }
        
        private void SetEnabled(bool enabled)
        {
            _btn.interactable = enabled;
            _canvasGroup.alpha = enabled ? 1f : _fadeDisabled;
        }

        private void RefreshEnabled()
        {
            SetEnabled(_countAvailable > 0 && 
                       _game.CanUseHint(_hintSo.LogicClass));
        }

        private void FailHint(FailReason reason)
        {
            Debug.LogError($"Fail use hint = {reason}");
            OnUseHintFail?.Invoke(new HintEventData(_hintSo, reason));
        }
    }
}
