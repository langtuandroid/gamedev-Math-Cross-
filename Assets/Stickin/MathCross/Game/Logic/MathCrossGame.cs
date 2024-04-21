using System.Collections.Generic;
using stickin;

namespace stickin.mathcross
{
    public class MathCrossGame : Game
    {
        public const int SignAdd = -100;
        public const int SignSub = -99;
        public const int SignDiv = -98;
        public const int SignMul = -97;
        public const int SignEql = -96;

        private MathCrossGameConfig _config;
        private LevelDifficult _difficult;
        private Board _board;
        private Pocket _pocket;
        
        public LevelDifficult Difficult => _difficult;

        public MathCrossGame(
            MathCrossSavable savable, 
            MathCrossGameConfig gameConfig, 
            LevelDifficult difficult, 
            IGameView gameView)
        {
            _difficult = difficult;
            _savable = savable;
            _config = gameConfig;
            var seconds = 0;
            var rewardResource = 0;
            
            RegistrGameModule(savable);

            if (savable.IsExistSave())
            {
                var levelSaveModel = savable.Load();
                
                _board = new Board(levelSaveModel.BoardCells, levelSaveModel.GridIndexes, levelSaveModel.RewardsIndexes, levelSaveModel.BoardSize);
                _pocket = new Pocket(levelSaveModel.PocketCells, levelSaveModel.PocketSize);

                _difficult = levelSaveModel.Difficult;
                seconds = levelSaveModel.Seconds;
                rewardResource = levelSaveModel.RewardResource;
            }
            else
            {
                var difficultConfig = gameConfig.GetDifficultConfig(difficult);
                
                var levelGenerator = new LevelsGenerator();
                var levelModel = levelGenerator.Generate(difficultConfig);
                
                _board = new Board(levelModel.BoardCells, levelModel.GridIndexes, levelModel.RewardIndexes, levelModel.Size);
                _pocket = new Pocket(levelModel.PocketCells, levelModel.PocketCells.Count);
            }

            var timerModule = new GameTimer(seconds, TimerType.Increase, TimerUpdateType.Seconds, null);
            RegistrGameModule(timerModule);

            var rewardResourceModule = new RewardResourceModule("coin", rewardResource);
            RegistrGameModule(rewardResourceModule);

            savable.Init(_difficult, _board, _pocket, timerModule, rewardResourceModule);
            Save();
            
            _board.OnAddedCell += OnAddedCellBoard;
            _board.OnRemoveCell += OnRemoveCellBoard;
            _board.OnComplete += OnCompleteBoard;
            
            _pocket.OnAddedCell += OnAddedCellPocket;
            _pocket.OnRemoveCell += OnRemoveCellPocket;

            gameView.Init(_difficult, _board, _pocket, rewardResourceModule);

            InitHints(); // Reinit. After init board and pocket
            
            _board.CheckCrossState();
        }

        protected override void InitHints()
        {
            _hints = new List<Hint>()
            {
                new LampHint(_board, _pocket)
            };
        }

        public override void Destroy()
        {
            base.Destroy();

            if (_board != null)
            {
                _board.OnAddedCell -= OnAddedCellBoard;
                _board.OnRemoveCell -= OnRemoveCellBoard;
                _board.OnComplete -= OnCompleteBoard;
            }

            if (_pocket != null)
            {
                _pocket.OnAddedCell -= OnAddedCellPocket;
                _pocket.OnRemoveCell -= OnRemoveCellPocket;
            }
        }

        private void OnAddedCellBoard(Cell model)
        {
            _pocket.RemoveCell(model);
            Save();
        }

        private void OnRemoveCellBoard(Cell model)
        {
            _pocket.AddedCell(model);
            Save();
        }
        
        private void OnAddedCellPocket(Cell model)
        {
            _board.RemoveCell(model);
            Save();
        }
        
        private void OnRemoveCellPocket(Cell model) { }

        private void OnCompleteBoard()
        {
            EndGame(GameStateType.Win);
            Save();
        }

        public override void SimulateEndGame()
        {
            base.SimulateEndGame();
            EndGame(GameStateType.Win);
        }
    }
}