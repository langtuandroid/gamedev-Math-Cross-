using System;
using System.Collections.Generic;

namespace stickin.mathcross
{
    public class Pocket : CellsContainer
    {
        public int MaxSize { get; }

        public Pocket(List<Cell> cells, int maxSize) : base()
        {
            foreach (var cell in cells)
            {
                cell.SetIsCorrect(true);
                Cells.Add(cell);
            }

            MaxSize = maxSize;
            SortCells();
        }
        
        public override void AddedCell(Cell model, Action midAction = null)
        {
            model.SetIsCorrect(true);
            base.AddedCell(model, () => SortCells());
        }

        private void SortCells()
        {
            Cells.Sort(delegate(Cell c1, Cell c2)
            {
                if (c1.Value == c2.Value)
                    return c1.GetHashCode().CompareTo(c2.GetHashCode());
                
                return c1.Value.CompareTo(c2.Value);
            });
        }
    }
}