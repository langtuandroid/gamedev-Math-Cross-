using System.Collections.Generic;
using stickin;
using UnityEngine;

namespace stickin.mathcross
{
    public class CellsParentView : TouchMonoBehaviour
    {
        [SerializeField] protected RectTransform _cellsParent;

        protected Dictionary<Cell, CellView> _cells = new Dictionary<Cell, CellView>();
        
        public virtual void AddedCell(CellView view, float duration)
        {
            view.transform.SetParent(_cellsParent);

            if (!_cells.ContainsKey(view.Model))
                _cells[view.Model] = view;

            view.ToScale(Vector3.one, 0.2f);
        }
        
        public virtual void RemoveCell(CellView view)
        {
            _cells.Remove(view.Model);
        }
    }
}