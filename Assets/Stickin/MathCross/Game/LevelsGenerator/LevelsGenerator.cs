using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace stickin.mathcross
{
    public class LevelsGenerator
    {
        #region Properties
        private static int SignAdd => MathCrossGame.SignAdd;
        private static int SignSub => MathCrossGame.SignSub;
        private static int SignDiv => MathCrossGame.SignDiv;
        private static int SignMul => MathCrossGame.SignMul;
        private static int SignEql => MathCrossGame.SignEql;

        public class FreeIndexModel
        {
            public Vector2Int Index;
            public int Length;
            public bool IsHorizontal;
            public bool IsIncrease;
        }

        private List<int> _signs1 = new List<int> {SignAdd, SignSub};
        private List<int> _signs2 = new List<int> {SignDiv, SignMul};
        #endregion
        
        #region Generate Board

        public LevelModel Generate(DifficultConfig config)
        {
            var levelModel = GenerateBoard(config.Size.x, config.Size.y, config.MaxNumber, 20);
            levelModel.CellToPocket(config.PercentageNumbersToPocket, config.PercentageRewards);

            return levelModel;
        }

        // private void MoveCellsToPocket(LevelModel levelModel)
        // {
        //     var newBoardCells = new List<Cell>();
        //     var newPocketCells = new List<Cell>();
        //
        //     foreach (var boardCell in levelModel.BoardCells)
        //     {
        //         if (boardCell.IsNumber && Random.Range(0, 100) < 40)
        //         {
        //             newPocketCells.Add(boardCell);
        //             boardCell.IsLocked = false;
        //         }
        //         else
        //         {
        //             newBoardCells.Add(boardCell);
        //             boardCell.IsLocked = true;
        //         }
        //     }
        //
        //     levelModel.BoardCells = newBoardCells;
        //     levelModel.PocketCells = newPocketCells;
        // }

        private LevelModel GenerateBoard(int w, int h, int maxNumber, int countTries)
        {
            LevelModel result = null;

            while (countTries > 0)
            {
                var levelModel = GenerateBoard(w, h, maxNumber);
                if (result == null || result.GridIndexes.Count < levelModel.GridIndexes.Count)
                    result = levelModel;

                countTries--;
            }

            return result;
        }

        private LevelModel GenerateBoard(int w, int h, int maxNumber)
        {
            var m = new Cell[w, h];
            var lines = new List<List<Cell>>();

            for (var i = 0; i < w; i++)
            for (var j = 0; j < h; j++)
                m[i, j] = null;

            var steps = 1000;
            while (steps > 0)
            {
                var length = Random.Range(0, 100) > 90 ? 7 : 5; // line lenght

                var freeIndexes = GetFreeIndexesForLength(length, m);
                if (freeIndexes.Count > 0)
                {
                    var randomIndex = freeIndexes.GetRandom();
                    var cells = GetNewEqualityCells(randomIndex, m);
                    if (FillCellsNumbers(cells, maxNumber, randomIndex.IsIncrease))
                    {
                        foreach (var cell in cells)
                        {
                            if (m[cell.CurrentIndex.x, cell.CurrentIndex.y] == null)
                                m[cell.CurrentIndex.x, cell.CurrentIndex.y] = cell;
                        }
                        
                        lines.Add(cells);
                    }
                }
                else
                    break;

                steps--;
            }

            return new LevelModel(m, new Vector2Int(w, h), lines);
        }

        private List<Cell> GetNewEqualityCells(FreeIndexModel model, Cell[,] m)
        {
            var result = new List<Cell>();

            var i = model.Index.x;
            var j = model.Index.y;
            var step = model.IsIncrease ? 1 : -1;

            var signs = model.Length == 5 && Random.Range(0, 100) > 80 ? _signs2 : _signs1;

            for (var n = 0; n < model.Length; n++)
            {
                if (m[i, j] == null)
                {
                    var cell = new Cell(0, new Vector2Int(i, j));

                    if (model.IsIncrease)
                        cell.SetValue(n % 2 == 0 ? 0 : (n == model.Length - 2 ? SignEql : signs.GetRandom()));
                    else
                        cell.SetValue(n % 2 == 0 ? 0 : (n == 1 ? SignEql : signs.GetRandom()));

                    result.Add(cell);
                }
                else
                {
                    result.Add(m[i, j]);
                }

                if (model.IsHorizontal)
                    i += step;
                else
                    j += step;
            }

            return result;
        }

        private List<FreeIndexModel> GetFreeIndexesForLength(int length, Cell[,] m)
        {
            var result = new List<FreeIndexModel>();

            for (var i = 0; i < m.GetLength(0); i++)
            {
                for (var j = 0; j < m.GetLength(1); j++)
                {
                    if ((i == 0 && j == 0 && m[i, j] == null) ||
                        (m[i, j] != null && m[i, j].IsNumber))
                    {
                        var directions = new List<Vector2Int>
                        {
                            Vector2Int.right,
                            Vector2Int.up,
                            Vector2Int.down,
                            Vector2Int.left
                        };

                        foreach (var direction in directions)
                        {
                            var isIncrease = direction == Vector2Int.right || direction == Vector2Int.up;

                            if (IsAvailableInsert(m, i, j, length, direction, isIncrease))
                            {
                                FreeIndexModel model = new FreeIndexModel();
                                model.Length = length;
                                model.Index = new Vector2Int(i, j);

                                model.IsHorizontal = direction == Vector2Int.right || direction == Vector2Int.left;
                                model.IsIncrease = isIncrease;
                                result.Add(model);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool IsAvailableInsert(Cell[,] m, int i, int j, int length, Vector2Int direction, bool isIncrease)
        {
            if (IsCheck(m, i - direction.x, j - direction.y) &&
                m[i - direction.x, j - direction.y] != null)
                return false;

            while (IsCheck(m, i, j) && (m[i, j] == null || m[i, j].IsNumber))
            {
                length--;
                i += direction.x;
                j += direction.y;
            }

            if (IsCheck(m, i, j) && m[i, j] != null)
                length = 1;

            return length <= 0;
        }

        private bool IsCheck(Cell[,] m, int i, int j)
        {
            return i >= 0 && i < m.GetLength(0) &&
                   j >= 0 && j < m.GetLength(1);
        }

        #endregion

        #region Generate Numbers
        private void GetIndexesForMask(List<int> mask, int maxNumber, int numbersCount, int iteration, List<int> result, int number = 0, int maxCount = 1000)
        {
            if (result.Count >= maxCount)
                return;
            
            if (iteration >= 1)
            {
                var numberIndex = number * maxNumber;
                var maskValue = mask[numbersCount - iteration];
                
                if (maskValue == 0)
                {
                    for (int i = 0; i < maxNumber; i++)
                        GetIndexesForMask(mask, maxNumber, numbersCount, iteration - 1, result, numberIndex + i);
                }
                else
                {
                    GetIndexesForMask(mask, maxNumber, numbersCount, iteration - 1, result, numberIndex + maskValue - 1);
                }
            }
            else
                result.Add(number);
        }
        
        private bool FillCellsNumbers(List<Cell> cells, int maxNumber, bool IsIncrease)
        {
            if (!IsIncrease)
                cells.Reverse();

            if (cells.Count > 2 && cells[1].IsEqual)
            {
                cells.Add(cells[1]);
                cells.Add(cells[0]);
                cells.RemoveRange(0, 2);
            }
            
            var result = GenerateNumbersForCells(cells, maxNumber);
            // PrintCellsList(cells, $"IsIncrease = {IsIncrease}     success = {result}");

            return result;
        }
        
        private bool GenerateNumbersForCells(List<Cell> cells, int maxNumber)
        {
            if (cells.Count > 0)
            {
                List<int> mask = new List<int>();
                var allIsZero = true;
                var cellsSum = cells[cells.Count - 1].Value;
                
                foreach (var cell in cells)
                {
                    if (cell.IsEqual)
                        break;

                    if (cell.IsNumber)
                    {
                        mask.Add(cell.Value);
                        if (cell.Value != 0)
                            allIsZero = false;
                    }
                }

                if (allIsZero)
                    mask[0] = Random.Range(1, maxNumber);

                var numbersCount = mask.Count;
                var allIndexes = new List<int>(1000);

                GetIndexesForMask(mask, maxNumber, numbersCount, numbersCount, allIndexes);

                allIndexes.Shuffle();

                foreach (var i in allIndexes)
                {
                    var sum = 0;
                    var isCorrect = false;
                    var sign = SignAdd;

                    for (var j = 0; j < numbersCount; j++)
                    {
                        var number = i / (int) Mathf.Pow(maxNumber, numbersCount - j - 1) % maxNumber + 1;
                        if (sign == SignAdd)
                            sum += number;
                        else if (sign == SignSub)
                            sum -= number;
                        else if (sign == SignMul)
                            sum *= number;
                        else if (sign == SignDiv)
                        {
                            if(sum % number == 0)
                                sum /= number;
                            else
                            {
                                sum = 0;
                                break;
                            }
                        }

                        var signIndex = 2 * j + 1;
                        if (signIndex >= 0 && signIndex < cells.Count)
                            sign = cells[signIndex].Value;
                    }
                    
                    if (sum >= 1 && sum <= maxNumber && (cellsSum == 0 || cellsSum == sum))
                    {
                        var n = 0;
                        foreach (var cell in cells)
                        {
                            if (cell.IsNumber)
                            {
                                int value;
                                if (n < numbersCount)
                                    value = i / (int) Mathf.Pow(maxNumber, numbersCount - n - 1) % maxNumber + 1;
                                else
                                    value = sum;
                                
                                cell.SetValue(value);
                                n++;
                            }
                        }
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
        
        private void PrintCellsList(List<Cell> cells, string postfix)
        {
            var str = "";
            foreach (var cell in cells)
                str += cell.ValueString();

            Debug.LogError($"PrintCellsList = {str}    {postfix}");
        }
    }
}