using System;
using System.Collections.Generic;
using stickin;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace stickin.mathcross
{
    public class BoardView : CellsParentView
    {
        [SerializeField] private GridCellView _gridCellPrefab;
        [SerializeField] private RewardResourceView _rewardResourcePrefab;

        private Board _board;
        private RewardResourceModule _rewardResourceModule;
        private Vector2Int _size;
        private List<GridCellView> _gridCells;
        private List<RewardResourceView> _rewards;

        public Vector3 CellsParentLocalScale => _cellsParent.transform.localScale;

        public void Init(Board board, RewardResourceModule rewardResourceModule)
        {
            _board = board;
            _rewardResourceModule = rewardResourceModule;
            _size = board.Size;

            _board.OnCollectReward += OnCollectReward;
            
            Clear();

            AddedGrid();
            AddedRewards();
        }

        private void OnDestroy()
        {
            if (_board != null)
                _board.OnCollectReward -= OnCollectReward;
        }

        private void OnCollectReward(Vector2Int index)
        {
            foreach (var rewardView in _rewards)
            {
                if (rewardView.Index == index)
                {
                    rewardView.Collect();
                    _rewards.Remove(rewardView);
                    break;
                }
            }
        }

        private void AddedGrid()
        {
            var indexes = new List<Vector2Int>();
            foreach (var boardIndex in _board.GridIndexes)
            {
                var index = Vector2Int.FloorToInt(FixedIndex(boardIndex));
                indexes.Add(index);
            }
            
            _gridCells = RectBoardExtensions.AddedCells(indexes, _gridCellPrefab, _cellsParent);
            
            _cellsParent.ResizeInParent();
        }

        private void AddedRewards()
        {
            _rewards = new List<RewardResourceView>();
            
            foreach (var index in _board.RewardsIndexes)
            {
                var fixedIndex = Vector2Int.FloorToInt(FixedIndex(index));
                var rewardView = RectBoardExtensions.AddedCell(fixedIndex, _rewardResourcePrefab, _cellsParent);
                rewardView.Init(index, _rewardResourceModule);
                
                _rewards.Add(rewardView);
            }
        }
        
        private void Clear()
        {
            if (_gridCells != null)
            {
                foreach (var cell in _gridCells)
                    Destroy(cell.gameObject);

                _gridCells.Clear();
            }
        }
        
        public override void AddedCell(CellView view, float duration = 0.3f)
        {
            base.AddedCell(view, duration);

            var index = Vector2Int.FloorToInt(FixedIndex(view.Model.CurrentIndex));
            
            var position = RectBoardExtensions.GetCellPosition(view.RectTransform(), index);
            view.ToAnchorPosition(position, duration);
        }

        public Vector2 ConvertScreenPositionToIndex(Vector2 screenPos)
        {
            var pos = ConvertPosToTransform(screenPos, _cellsParent) + _cellsParent.rect.size / 2;
            
            var index = RectBoardExtensions.GetCellIndex(_gridCells[0].RectTransform(), pos);
            index = FixedIndex(index);
            return index;
        }

        private Vector2 FixedIndex(Vector2 index)
        {
            index.y = _size.y - index.y - 1;
            return index;
        }
    }
}