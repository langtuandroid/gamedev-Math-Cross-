using System;
using System.Collections.Generic;
using UnityEngine;

namespace stickin.mathcross
{
    public class Board : CellsContainer
    {
        public List<Vector2Int> GridIndexes { get; }
        public Vector2Int Size { get; set; }
        public List<Vector2Int> RewardsIndexes { get; }

        public event Action<Vector2Int> OnCollectReward;
        public event Action OnComplete;

        public Board(List<Cell> cells, List<Vector2Int> gridGridIndexes, List<Vector2Int> rewardsIndexes, Vector2Int size) : base()
        {
            GridIndexes = gridGridIndexes;
            RewardsIndexes = rewardsIndexes;
            Size = size;

            foreach (var cell in cells)
                Cells.Add(cell);
        }

        public override void AddedCell(Cell model, Action midAction = null)
        {
            base.AddedCell(model, midAction);
            
            if (RewardsIndexes.Contains(model.CurrentIndex))
            {
                RewardsIndexes.Remove(model.CurrentIndex);
                OnCollectReward?.Invoke(model.CurrentIndex);
            }

            CheckCrossState();
        }

        public override void RemoveCell(Cell model)
        {
            base.RemoveCell(model);
            
            CheckCrossState();
        }

        public bool TryAddedCell(Cell model, Vector2Int index)
        {
            if (GridIndexes.Contains(index))
            {
                var oldCell = GetCell(index);
                if (oldCell != null && oldCell.IsLocked)
                    return false;

                var currentCellInBoard = Cells.Contains(model);
                var modelIndex = model.CurrentIndex;
                
                model.SetCurrentIndex(index);
                AddedCell(model);

                if (oldCell != null)
                {
                    if (currentCellInBoard)
                    {
                        oldCell.SetCurrentIndex(modelIndex);
                        AddedCell(oldCell);
                    }
                    else
                        RemoveCell(oldCell);
                }

                if (IsComplete())
                    OnComplete?.Invoke();

                return true;
            }

            return false;
        }
        
        public void CheckCrossState()
        {
            foreach (var cell in Cells)
                cell.SetIsCorrect(!cell.IsLocked);

            var correctCells = new List<Cell>();
            var uncorrectCells = new List<Cell>();
            
            foreach (var cell in Cells)
            {
                if (cell.IsEqual)
                {
                    CheckLine(cell, Vector2Int.right, correctCells, uncorrectCells);
                    CheckLine(cell, Vector2Int.up, correctCells, uncorrectCells);
                }
            }

            foreach (var cell in correctCells)
                cell.SetIsCorrect(true);
            
            foreach (var cell in uncorrectCells)
                cell.SetIsCorrect(false);
        }

        private void CheckLine(Cell cell, Vector2Int direction, List<Cell> correctCells, List<Cell> uncorrectCells)
        {
            var (leftResult, isFullLeft) = CalculateResult(cell.CurrentIndex, -direction);
            var (rightResult, isFullRight) = CalculateResult(cell.CurrentIndex, direction);

            if (isFullLeft && isFullRight)
            {
                if (leftResult == rightResult)
                {
                    SetLineCellsCorrect(cell.CurrentIndex, direction, correctCells);
                    SetLineCellsCorrect(cell.CurrentIndex, -direction, correctCells);
                }
                else
                {
                    SetLineCellsCorrect(cell.CurrentIndex, direction, uncorrectCells);
                    SetLineCellsCorrect(cell.CurrentIndex, -direction, uncorrectCells);
                }
            }
        }

        private (int, bool) CalculateResult(Vector2Int index, Vector2Int direction)
        {
            var result = 0;
            var prevSign = MathCrossGame.SignAdd;
            var cells = new List<Cell>();
            
            index += direction;

            // get all cells in line
            do
            {
                Cell cell = GetCell(index);

                if (cell != null)
                    cells.Insert(0, cell);
                else
                    return (0, false);
                
                index += direction;
            } 
            while (GridIndexes.Contains(index));

            if (cells.Count <= 0)
                return (0, false);

            // check correct values
            foreach (var cell in cells)
            {
                if (cell.IsNumber)
                {
                    var valueInt = cell.Value;
                    
                    if (prevSign == MathCrossGame.SignAdd)
                        result += valueInt;
                    else if(prevSign == MathCrossGame.SignSub)
                        result -= valueInt;
                    else if(prevSign == MathCrossGame.SignDiv)
                        result /= (valueInt != 0 ? valueInt : 1);
                    else if(prevSign == MathCrossGame.SignMul)
                        result *= valueInt;
                }
                else if (cell.IsSign)
                    prevSign = cell.Value;
            }

            return (result, true);
        }

        private void SetLineCellsCorrect(Vector2Int index, Vector2Int direction, List<Cell> list)
        {
            while (GridIndexes.Contains(index))
            {
                var cell = GetCell(index);
                if (cell != null)
                    list.Add(cell);

                index += direction;
            }
        }
        
        public bool IsComplete()
        {
            foreach (var index in GridIndexes)
            {
                var cell = GetCell(index);
                if (cell == null || !cell.IsCorrect)
                    return false;
            }

            return true;
        }
    }
}