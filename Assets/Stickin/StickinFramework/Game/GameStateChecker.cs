using System.Collections;
using stickin;
using stickin.menus;
using UnityEngine;

namespace stickin
{
    public class GameStateChecker : MonoBehaviour
    {
        [SerializeField] private BaseMenu _winMenu;
        [SerializeField] private BaseMenu _loseMenu;
        [SerializeField] private bool _tryShowIntersitial = true;

        [InjectField] private AdsService _adsService;

        private GameView _gameView;

        private void Start()
        {
            InjectService.BindFields(this);
            
            _gameView = FindObjectOfType<GameView>();
            _gameView.OnChangeGameState += OnChangeGameState;
        }

        private void OnChangeGameState(GameStateType state, GameEndReasonType reason)
        {
            if (state == GameStateType.Win)
            {
                var hashtable = new Hashtable
                {
                    ["game"] = _gameView.Game
                };

                MenusService.Show(_winMenu, hashtable);
                TryShowInterstitial();
            }
            else if (state == GameStateType.Lose)
            {
                var data = new Hashtable
                {
                    ["reason"] = reason,
                    ["game"] = _gameView.Game
                };
                MenusService.Show(_loseMenu, data);
                TryShowInterstitial();
            }
        }

        private void TryShowInterstitial()
        {
            if (_tryShowIntersitial)
                StartCoroutine(TryShowInterstitialCoroutine());
        }

        private IEnumerator TryShowInterstitialCoroutine()
        {
            yield return new WaitForSeconds(1f);
            
            _adsService.TryShowInterstitial();
        }
    }
}