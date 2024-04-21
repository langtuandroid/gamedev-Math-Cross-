using System;
using System.Collections.Generic;
using UnityEngine;

namespace stickin.mathcross
{
    public class CellsContainer
    {
        public List<Cell> Cells { get; private set; }

        public event Action<Cell> OnAddedCell;
        public event Action<Cell> OnRemoveCell;

        public CellsContainer()
        {
            Cells = new List<Cell>();
        }

        public virtual void AddedCell(Cell model, Action midAction)
        {
            if (!Cells.Contains(model))
                Cells.Add(model);
            
            midAction?.Invoke();
            
            OnAddedCell?.Invoke(model);
        }

        public virtual void RemoveCell(Cell model)
        {
            if (model != null && Cells.Contains(model))
            {
                Cells.Remove(model);

                OnRemoveCell?.Invoke(model);
            }
        }
        
        protected Cell GetCell(Vector2Int index)
        {
            foreach (var cell in Cells)
            {
                if (cell.CurrentIndex == index)
                    return cell;
            }

            return null;
        }
    }
}