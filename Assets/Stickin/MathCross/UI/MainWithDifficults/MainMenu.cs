using System;
using System.Collections;
using stickin.menus;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class MainMenu : BaseMenu
    {
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Transform _selectDifficultMenuPosition;

        [InjectField] private LevelsProgressService _levelsProgressService;

        private int _level;

        private void Start()
        {
            InjectService.BindFields(this);
            SpinController.Instance.Refresh();

            _level = 1; // levels generate random
            
            var isExistProgress = _levelsProgressService.IsExistProgress(_level, LevelProgressType.Started);
            _continueBtn.gameObject.SetActive(isExistProgress);
            
            _continueBtn.onClick.AddListener(OnClickContinue);
            _playBtn.onClick.AddListener(OnClickPlay);
        }

        private void OnClickContinue()
        {
            Play(LevelDifficult.Unknown, false);
        }
        
        private void OnClickPlay()
        {
            MenusService.Show<SelectDifficultMenu>(new Hashtable
            {
                ["position"] = _selectDifficultMenuPosition.position,
                ["callback"] = (Action<LevelDifficult>)PlayNew
            });
        }

        public void PlayNew(LevelDifficult difficult)
        {
            Play(difficult, true);
        }
        
        public void Play(LevelDifficult difficult, bool withReset)
        {
            if (withReset)
                _levelsProgressService.ResetProgress(_level);
            
            var data = new Hashtable {["difficult"] = difficult};
            GameLauncher.PlayLevel(_level, OrderAssetType.Levels, data);
        }
    }
}