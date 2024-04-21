using System.Collections.Generic;
using stickin;
using UnityEngine;

namespace stickin.mathcross
{
    public class PocketView : CellsParentView
    {
        private const float _moveDuration = 0.2f;
        
        [SerializeField] private GridCellView _gridCellPrefab;
        private List<GridCellView> _gridCells;
        
        private int _maxCount;
        private Pocket _pocket;
        
        public void Init(Pocket pocket)
        {
            _pocket = pocket;
            
            var indexes = new List<Vector2Int>();
            var space = 20;
            var parentSize = (_cellsParent.parent.transform as RectTransform).rect.size;
            
            var koef = parentSize.x / parentSize.y;
            var columns = Mathf.CeilToInt(Mathf.Sqrt(pocket.MaxSize * koef));
            
            for(var i = 0; i < pocket.MaxSize; i++)
            {
                var x = i % columns;
                var y = i / columns;

                indexes.Add(new Vector2Int(x, -y));
            }

            _gridCells = RectBoardExtensions.AddedCells(indexes, _gridCellPrefab, _cellsParent, space);
            _cellsParent.ResizeForContent();
            _cellsParent.ResizeInParent();
        }

        public override void AddedCell(CellView view, float duration = _moveDuration)
        {
            base.AddedCell(view, duration);

            RefreshPositions(duration);
        }

        public override void RemoveCell(CellView view)
        {
            base.RemoveCell(view);

            RefreshPositions();
        }

        private void RefreshPositions(float duration = _moveDuration)
        {
            var index = 0;
            for (var i = 0; i < _pocket.Cells.Count; i++)
            {
                var model = _pocket.Cells[i];
                if (_cells.ContainsKey(model))
                {
                    var cellView = _cells[model];
                    if (cellView != null)
                        cellView.ToLocalPosition(_gridCells[index].transform.localPosition, duration);

                    index++;
                }
            }
        }
    }
}