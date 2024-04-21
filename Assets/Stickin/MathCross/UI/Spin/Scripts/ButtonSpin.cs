using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class ButtonSpin : MonoBehaviour
    {
        [SerializeField] private Text _freeText;
        [SerializeField] private GameObject _rewardBg;
        [SerializeField] private Text _countText;
        [SerializeField] private Spin _spin;

        [InjectField] private AdsService _adsService;
        
        private Button _btn;

        private void Start()
        {
            SpinController.Instance.Refresh();
            
            InjectService.BindFields(this);
            
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);

            SpinController.Instance.OnRefresh += RefreshView;
            // ServiceAds.Instance.OnRefreshVideoReward += OnRefreshVideoReward;

            RefreshView();
        }
        
        private void OnDestroy()
        {
            SpinController.Instance.OnRefresh -= RefreshView;
            // ServiceAds.Instance.OnRefreshVideoReward -= OnRefreshVideoReward;
        }

        private void OnRefreshVideoReward(string id, bool isLoad)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            var isFree = SpinController.Instance.IsFreeSpin;
            
            _freeText.gameObject.SetActive(isFree);
            _rewardBg.SetActive(!isFree);
            _countText.text = SpinController.Instance.CountAvailableSpins + "/" +
                              SpinController.MAX_COUNT_SPINS;
            
            // SetInteractable(!_spin.IsRun);
        }

        private void OnClick()
        {
            if (SpinController.Instance.IsFreeSpin)
            {
                SpinRun();
            }
            else if (_adsService.IsRewardAvailable())
            {
                if (SpinController.Instance.CountAvailableSpins > 0)
                    _adsService.ShowReward(SpinRun);
                else
                    TextMessageMenu.ShowWithText("Not available spins today");
            }
            else
            {
                TextMessageMenu.ShowWithText(TextMessageMenu.AdsNotReady);
            }
        }

        private void SpinRun()
        {
            _spin.Run();
            
            SpinController.Instance.DecreaseCountAvailableSpins();
        }

        // public void SetInteractable(bool value)
        // {
        //     if (value)
        //     {
        //         // _btn.interactable = SpinController.Instance.IsFreeSpin ||
        //         //                     (SpinController.Instance.CountAvailableSpins > 0 &&
        //         //                      ServiceAds.Instance.IsAnyRewardAvailable());
        //     }
        //     else
        //     {
        //         _btn.interactable = false;
        //     }
        // }
    }
}