using System.Collections.Generic;
using stickin;
using UnityEngine;

namespace stickin.mathcross
{
    public class LevelModel
    {
        public Vector2Int Size;
        public List<Vector2Int> GridIndexes;
        
        public List<Cell> BoardCells;
        public List<Cell> PocketCells;
        public List<Vector2Int> RewardIndexes;

        private List<List<Cell>> _lines;

        public LevelModel(Cell[,] m, Vector2Int size, List<List<Cell>> lines)
        {
            _lines = lines;
            
            Size = size;
            BoardCells = new List<Cell>();
            PocketCells = new List<Cell>();
            GridIndexes = new List<Vector2Int>();
            RewardIndexes = new List<Vector2Int>();

            for (var i = 0; i < m.GetLength(0); i++)
            {
                for (var j = 0; j < m.GetLength(1); j++)
                {
                    var cell = m[i, j];

                    if (cell != null)
                    {
                        GridIndexes.Add(new Vector2Int(i, j));
                        
                        if (cell.IsLocked)
                            PocketCells.Add(cell);
                        else
                            BoardCells.Add(cell);
                    }
                }
            }
        }

        public void CellToPocket(int percentageToPocket, int percentageRewards)
        {
            var i = 0;
            var steps = 1000;
            var countNumbers = 0;

            foreach (var cell in BoardCells)
            {
                cell.IsLocked = true;
                if (cell.IsNumber)
                    countNumbers++;
            }
            
            var countToPocket = Mathf.Max(countNumbers * percentageToPocket / 100, _lines.Count);

            while (steps > 0 && PocketCells.Count < countToPocket)
            {
                var line = _lines.GetElement(i);
                
                var boardCells = new List<Cell>();
                foreach (var cell in line)
                {
                    if (cell.IsNumber && !PocketCells.Contains(cell))
                        boardCells.Add(cell);
                }

                if (boardCells.Count > 1)
                {
                    var cell = boardCells.GetRandom();
                    cell.IsLocked = false;
                    
                    PocketCells.Add(cell);
                    BoardCells.Remove(cell);
                    
                    if (Random.Range(0, 100) < percentageRewards)
                        RewardIndexes.Add(cell.CurrentIndex);
                }

                steps--;
                i++;
                if (i == _lines.Count - 1)
                    _lines.Shuffle();
            } 
            
            _lines.Clear();
        }
    }
}