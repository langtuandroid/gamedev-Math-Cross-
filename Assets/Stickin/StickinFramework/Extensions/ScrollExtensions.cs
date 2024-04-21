using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    public static class ScrollExtensions
    {
        public static List<T> AddedVerticalCells<T>(this ScrollRect scrollRect, 
            T prefab, 
            int count, 
            int columns = 1,
            float startSpaceY = 0f,
            float spaceY = 0f) where T : MonoBehaviour
        {
            var result = new List<T>();
            Vector2 cellSize = prefab.RectTransform().sizeDelta;
            var space = (scrollRect.content.rect.width - cellSize.x * columns) / (columns + 1);

            var x = space;
            var y = -startSpaceY;
            var columnIndex = 0;
            
            for (var i = 0; i < count; i++)
            {
                var cell = GameObject.Instantiate(prefab, scrollRect.content);
                var cellRt = cell.RectTransform();
                cellRt.anchoredPosition = new Vector2(x + cellSize.x / 2f, y - cellSize.y / 2f);

                columnIndex++;
                if (columnIndex >= columns)
                {
                    x = space;
                    y -= cellSize.y + spaceY;
                    columnIndex = 0;
                }
                else
                    x += cellSize.x + space;

                result.Add(cell);
            }

            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, -y);

            return result;
        }
    }
}