using System.Collections.Generic;
using stickin;
using stickin.menus;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class MathCrossGameView : GameView, IGameView
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private PocketView _pocketView;
        [SerializeField] private CellView _cellViewPrefab;
        [SerializeField] private TextTimer _timerText;
        [SerializeField] private Text _difficultText;
        [SerializeField] private TouchableController _touchableController;

        [InjectField] private LevelsProgressService _levelsProgressService;
        [InjectField] private ResourcesService _resourcesService;

        private List<CellView> _allCellsViews = new List<CellView>();
        private Pocket _pocket;
        private Board _board;
        private CellView _currentCellTouch;

        public override void InitWithLevelNumber(GameParams gameParams)
        {
            base.InitWithLevelNumber(gameParams);

            var difficult = LevelDifficult.Unknown;
            var gameSavable = new MathCrossSavable(gameParams.LevelNumber, _levelsProgressService);

            if (gameParams.CustomData != null && gameParams.CustomData.ContainsKey("difficult"))
                difficult = (LevelDifficult) gameParams.CustomData["difficult"];

            _game = new MathCrossGame(gameSavable, _gameConfig as MathCrossGameConfig, difficult, this);

            var timer = _game.GetGameModule<GameTimer>();
            _timerText.Init(timer);
            
            // for levels generator preview. Check scene 'LevelsGenerator'
            CheckAddedAllCellsToBoard(gameParams);
        }
        
        public void Init(LevelDifficult difficult, Board board, Pocket pocket, RewardResourceModule rewardResourceModule)
        {
            _difficultText.text = difficult.ToString();
            
            _board = board;
            _pocket = pocket;

            _board.OnAddedCell += OnAddedCellBoard;
            _board.OnRemoveCell += OnRemoveCellBoard;
            
            _pocket.OnAddedCell += OnAddedCellPocket;
            _pocket.OnRemoveCell += OnRemoveCellPocket;

            rewardResourceModule.SetResourcesService(_resourcesService);
            
            _boardView.Init(board, rewardResourceModule);
            _pocketView.Init(pocket);
            _touchableController.Init(_boardView.CellsParentLocalScale);

            CreateCellsView(board.Cells, _boardView);
            CreateCellsView(pocket.Cells, _pocketView);
        }

        private void CreateCellsView(List<Cell> cells, CellsParentView parent)
        {
            foreach (var cellModel in cells)
            {
                var cellView = Instantiate(_cellViewPrefab, transform);
                cellView.Init(cellModel);
                parent.AddedCell(cellView, 0f);
                
                _allCellsViews.Add(cellView);
                
                if (cellView.TryGetComponent(out Touchable touchable))
                    _touchableController.RegistrTouchable(touchable);
            }
        }

        // for levels generator preview
        private void CheckAddedAllCellsToBoard(GameParams gameParams)
        {
            if (gameParams.CustomData != null && 
                gameParams.CustomData.ContainsKey("allCellsToBoard"))
            {
                var allCellsToBoard = (bool) gameParams.CustomData["allCellsToBoard"];
                if (allCellsToBoard)
                {
                    var steps = 1000;
                    while (steps > 0 && _pocket.Cells.Count > 0)
                    {
                        var cellModel = _pocket.Cells[0];
                        _board.TryAddedCell(cellModel, cellModel.CurrentIndex);
                        
                        steps--;
                    }
                }
                
                _board.CheckCrossState();
            }
        }

        private void OnAddedCellBoard(Cell model) => _boardView.AddedCell(GetCell(model));
        private void OnRemoveCellBoard(Cell model) => _boardView.RemoveCell(GetCell(model));
        private void OnAddedCellPocket(Cell model) => _pocketView.AddedCell(GetCell(model));
        private void OnRemoveCellPocket(Cell model) => _pocketView.RemoveCell(GetCell(model));

        protected override void Awake()
        {
            base.Awake();

            if (_touchableController != null)
            {
                _touchableController.OnStartTouch += OnStartTouch;
                _touchableController.OnEndTouch += OnEndTouch;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_touchableController != null)
            {
                _touchableController.OnStartTouch -= OnStartTouch;
                _touchableController.OnEndTouch -= OnEndTouch;
            }

            if (_board != null)
            {
                _board.OnAddedCell -= OnAddedCellBoard;
                _board.OnRemoveCell -= OnRemoveCellBoard;
            }

            if (_pocket != null)
            {
                _pocket.OnAddedCell -= OnAddedCellPocket;
                _pocket.OnRemoveCell -= OnRemoveCellPocket;
            }
        }

        private void OnStartTouch(Touchable touchable, Vector2 screenPos)
        {
            if (_currentCellTouch != null)
                _currentCellTouch = null;
            
            if (touchable.TryGetComponent(out CellView cell))
                _currentCellTouch = cell;
        }

        private void OnEndTouch(Touchable touchable)
        {
            if (touchable.TryGetComponent(out CellView cell))
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, cell.transform.position);
                var index = _boardView.ConvertScreenPositionToIndex(screenPos);

                var neighbors = RectBoardExtensions.MidAndNeighbors8;
                var isFind = false;

                for (var i = 0; i < neighbors.Count; i++)
                {
                    var checkIndex = Vector2Int.RoundToInt(index + new Vector2(neighbors[i].x / 2f, neighbors[i].y / 2f));
                    
                    if (_board.TryAddedCell(cell.Model, checkIndex))
                    {
                        isFind = true;
                        break;
                    }
                }

                if (!isFind)
                    _pocket.AddedCell(cell.Model); // return to pocket and refresh positions
            }
        }

        private CellView GetCell(Cell model)
        {
            foreach (var cell in _allCellsViews)
            {
                if (cell.Model == model)
                    return cell;
            }

            return null;
        }
    }
}
