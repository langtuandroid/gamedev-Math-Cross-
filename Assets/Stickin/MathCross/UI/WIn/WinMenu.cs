using System.Collections;
using stickin.menus;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class WinMenu : BaseMenu
    {
        [SerializeField] private Text _difficultText;
        [SerializeField] private TextTimer _timerText;
        [SerializeField] private TextWithIcon _coinsText;
        [SerializeField] private TextWithIcon _claimText;
        [SerializeField] private Button _claimX3Btn;
        [SerializeField] private Button _claimBtn;

        [SerializeField] private int _claimX = 5;

        [InjectField] private ResourcesService _resourcesService;
        [InjectField] private AdsService _adsService;

        private RewardResourceModule _rewardResource;

        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            if (data != null && data.ContainsKey("game"))
            {
                var game = (Game) data["game"];
                _timerText.Init(game.GetGameModule<GameTimer>());

                _rewardResource = game.GetGameModule<RewardResourceModule>();
                _coinsText.SetText(_rewardResource.Value.ToString());
                _difficultText.text = string.Empty;// game.Difficult.ToString();
            }
        }

        private void Start()
        {
            InjectService.BindFields(this);
            
            _claimX3Btn.onClick.AddListener(OnClickClaimX3);
            _claimBtn.onClick.AddListener(OnClickClaim);
            
            _claimText.SetText($"Claim x{_claimX}");
        }

        private void OnClickClaim()
        {
            SceneLoader.LoadScene(2);
        }
        
        private void OnClickClaimX3()
        {
            if (_adsService.IsRewardAvailable())
                _adsService.ShowReward(OnRewardComplete);
            else
                TextMessageMenu.ShowWithText(TextMessageMenu.AdsNotReady);
        }

        private void OnRewardComplete()
        {
            _resourcesService.ChangeResource(_rewardResource.Id, _rewardResource.Value * (_claimX - 1));
            SceneLoader.LoadScene(2);
        }
    }
}