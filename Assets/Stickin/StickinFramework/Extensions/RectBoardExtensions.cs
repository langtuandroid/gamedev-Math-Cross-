using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public static class RectBoardExtensions
    {
        public static readonly List<Vector2Int> MidAndNeighbors4 = new()
        {
            Vector2Int.zero,
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.up
        };
        
        public static readonly List<Vector2Int> Neighbors4 = new()
        {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.up
        };
        
        public static readonly List<Vector2Int> MidAndNeighbors8 = new()
        {
            Vector2Int.zero,
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right,
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        public static readonly List<Vector2Int> Neighbors8 = new()
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right,
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        public static Vector2 GetCellPosition(RectTransform rt, Vector2Int index, float spaceBetweenCells = 0f, float xPosMultuplier = 1f, float yPosMultuplier = 1f)
        {
            // var x = index.x * (rt.sizeDelta.x + spaceBetweenCells) * xPosMultuplier + rt.sizeDelta.x / 2f;
            // var y = index.y * (rt.sizeDelta.y + spaceBetweenCells) * yPosMultuplier + rt.sizeDelta.y / 2f;
            
            var x = index.x * (rt.sizeDelta.x + spaceBetweenCells) * xPosMultuplier + rt.sizeDelta.x * rt.pivot.x;
            var y = index.y * (rt.sizeDelta.y + spaceBetweenCells) * yPosMultuplier + rt.sizeDelta.y * rt.pivot.y;

            return new Vector2(x, y);
        }

        public static Vector2Int GetCellIndexInt(RectTransform rt, Vector2 position, float spaceBetweenCells = 0f)
        {
            var index = GetCellIndex(rt, position, spaceBetweenCells);
            return new Vector2Int(
                Mathf.RoundToInt(index.x),
                Mathf.RoundToInt(index.y)); // @TODO need check CeilToInt
        }
        
        public static Vector2 GetCellIndex(RectTransform rt, Vector2 position, float spaceBetweenCells = 0f)
        {
            position -= rt.sizeDelta / 2f;
            position.x = position.x / (rt.sizeDelta.x + spaceBetweenCells);
            position.y = position.y / (rt.sizeDelta.x + spaceBetweenCells);
            
            return new Vector2(
                position.x,
                position.y);
        }
        
        public static T AddedCell<T>(Vector2Int index, T prefab, RectTransform parent, float spaceBetweenCells = 0f) where T : MonoBehaviour
        {
            var cell = GameObject.Instantiate(prefab, parent);
            var cellRt = cell.RectTransform();
            
            cellRt.anchoredPosition = GetCellPosition(cellRt, index, spaceBetweenCells);

            return cell;
        }
        
        public static List<T> AddedCells<T>(
            List<Vector2Int> indexes, 
            T prefab, 
            RectTransform parent, 
            float spaceBetweenCells = 0f, 
            bool changeSize = true, 
            float xPosMultuplier = 1f, 
            float yPosMultuplier = 1f) where T : MonoBehaviour
        {
            var result = new List<T>();
            
            var maxX = 0f;
            var maxY = 0f;
            var cellRt = prefab.RectTransform();
            
            foreach (var index in indexes)
            {
                var cell = GameObject.Instantiate(prefab, parent);
                result.Add(cell);
                
                var rt = cell.RectTransform();
                var pos = GetCellPosition(cellRt, index, spaceBetweenCells, xPosMultuplier, yPosMultuplier);
                
                rt.anchoredPosition = pos;
                
                maxX = Mathf.Max(pos.x, maxX);
                maxY = Mathf.Max(pos.y, maxY);
            }

            if (changeSize)
            {
                parent.sizeDelta = new Vector2(
                    maxX + cellRt.sizeDelta.x / 2f,
                    maxY + cellRt.sizeDelta.y / 2f);
            }

            return result;
        }

        public static int GetIndex(int i, int j, int height) => i * height + j;
    }
}