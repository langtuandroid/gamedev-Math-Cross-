using System;
using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class LevelsProgressService : BaseService
    {
        [SerializeField] private string _saveFilename = "LevelsProgressData.json";
        private LevelsProgressData _progress;

        public event Action OnSaveBegin; 

        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            base.Init(appData, callbackComplete);
            
            Load();
            InjectService.Bind<LevelsProgressService>(this);
            
            InitComplete(true);
        }
        
        public void Save()
        {
            OnSaveBegin?.Invoke();
            
            if (_progress != null)
                SaveHelper.SaveJson(_progress, _saveFilename, true);
        }

        private void Load()
        {
            _progress = SaveHelper.LoadJson<LevelsProgressData>(_saveFilename);

            if (_progress == null)
            {
                _progress = new LevelsProgressData();
                _progress.Levels = new List<LevelProgressData>();
            }
        }

        public bool IsExistProgress(int levelNumber, LevelProgressType progressType)
        {
            foreach (var levelProgressData in _progress.Levels)
            {
                if (levelProgressData.LevelNumber == levelNumber &&
                    levelProgressData.ProgressType == progressType)
                    return true;
            }

            return false;
        }
        
        public LevelProgressData GetProgress(int levelNumber, LevelProgressData defaultValue = null, bool withSave = true)
        {
            foreach (var levelProgressData in _progress.Levels)
            {
                if (levelProgressData.LevelNumber == levelNumber)
                    return levelProgressData;
            }

            if (defaultValue != null)
            {
                _progress.Levels.Add(defaultValue);
                
                if (withSave)
                    Save();
                
                return defaultValue;
            }

            var newData = new LevelProgressData(levelNumber);
            _progress.Levels.Add(newData);
            
            if (withSave)
                Save();

            return newData;
        }
        
        public void SetProgress(int levelNumber, string customStr, LevelProgressType type)
        {
            var progress = GetProgress(levelNumber);
            progress.SetCustomStr(customStr);
            progress.SetProgressType(type);

            // Save();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // Debug.LogError("OnApplicationFocus = " + hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // Debug.LogError("OnApplicationPause = " + pauseStatus);
            
            if (pauseStatus)
                Save();
        }

        private void OnApplicationQuit()
        {
            // Debug.LogError("OnApplicationQuit");
            Save();
        }

        public void ResetProgress(int levelNumber)
        {
            var progress = GetProgress(levelNumber);
            progress.SetProgressType(LevelProgressType.None);
            progress.SetCustomStr(String.Empty);

            Save();
        }
    }
}