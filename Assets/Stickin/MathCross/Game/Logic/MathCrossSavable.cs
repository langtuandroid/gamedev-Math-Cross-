using System.Collections.Generic;
using stickin;
using UnityEngine;

namespace stickin.mathcross
{
    [System.Serializable]
    public class LevelSaveModel
    {
        public Vector2Int BoardSize;
        public int PocketSize;
        public int Seconds;
        public LevelDifficult Difficult;
        public int RewardResource;
        
        public List<Vector2Int> GridIndexes;
        public List<Vector2Int> RewardsIndexes;
        public List<Cell> BoardCells;
        public List<Cell> PocketCells;
    }
    
    public class MathCrossSavable : ISavable, IGameModule
    {
        private LevelsProgressService _levelsProgressService;
        
        private int _levelNumber;
        private LevelDifficult _difficult;
        private Board _board;
        private Pocket _pocket;
        private GameTimer _timer;
        private RewardResourceModule _rewardResourceModule;

        public MathCrossSavable(int levelNumber, LevelsProgressService levelsProgressService)
        {
            _levelNumber = levelNumber;
            _levelsProgressService = levelsProgressService;

            if (_levelsProgressService != null)
                _levelsProgressService.OnSaveBegin += Save;
        }

        public void Init(LevelDifficult difficult, Board board, Pocket pocket, GameTimer timer, RewardResourceModule rewardResourceModule)
        {
            _difficult = difficult;
            _board = board;
            _pocket = pocket;
            _timer = timer;
            _rewardResourceModule = rewardResourceModule;
        }

        public void Save()
        {
            if (_levelsProgressService != null)
            {
                var data = new LevelSaveModel();
                data.Difficult = _difficult;
                data.GridIndexes = _board.GridIndexes;
                data.RewardsIndexes = _board.RewardsIndexes;
                data.BoardCells = _board.Cells;
                data.Seconds = _timer.Seconds;
                data.BoardSize = _board.Size;
                data.PocketCells = _pocket.Cells;
                data.PocketSize = _pocket.MaxSize;
                data.RewardResource = _rewardResourceModule.Value;

                var json = JsonUtility.ToJson(data);

                _levelsProgressService.SetProgress(_levelNumber, json, _board.IsComplete() ? LevelProgressType.Done : LevelProgressType.Started);
            }
        }

        public LevelSaveModel Load()
        {
            LevelSaveModel result = null;
            
            var data = _levelsProgressService.GetProgress(_levelNumber, withSave: false);
            if (data != null && data.CustomData != null)
            {
                result = JsonUtility.FromJson<LevelSaveModel>(data.CustomData);
            }
            
            return result;
        }

        public bool IsExistSave()
        {
            return _levelsProgressService != null && _levelsProgressService.IsExistProgress(_levelNumber, LevelProgressType.Started);
        }

        public void Stop() {}
        public void Pause() { }
        public void Resume() { }

        public void Destroy()
        {
            if (_levelsProgressService != null)
                _levelsProgressService.OnSaveBegin -= Save;
        }
    }
}