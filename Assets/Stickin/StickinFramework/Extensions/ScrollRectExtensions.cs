using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    public static class ScrollRectExtensions
    {
        public static void VerticalLayout(this ScrollRect scrollRect, int countColumns = 1)
        {
            if (scrollRect.content.childCount > 0)
            {
                var cell = scrollRect.content.GetChild(0);
                var cellSize = (cell as RectTransform).rect.size;

                var border = (int) ((scrollRect.content.rect.size.x - countColumns * cellSize.x) / (countColumns + 1));

                for (var i = 0; i < scrollRect.content.childCount; i++)
                {
                    var row = i / countColumns;
                    var column = i % countColumns;

                    var cellRt = scrollRect.content.GetChild(i) as RectTransform;
                    var pos = GetPosition(row, column, border, cellRt.pivot, cellRt.rect.size);
                    cellRt.anchoredPosition = pos;

                    scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x,
                        -pos.y + cellRt.pivot.y * cellRt.rect.size.y + border);
                }
            }
        }

        public static void HorizontalLayout(this ScrollRect scrollRect, int countRows = 1)
        {
            Debug.LogError("ScrollRectExtensions: NEED CODE");
        }

        private static Vector2 GetPosition(int row, int column, float border, Vector2 cellPivot, Vector2 cellSize)
        {
            var x = (column + cellPivot.x) * cellSize.x + border * (column + 1);
            var y = (row + 1 - cellPivot.y) * cellSize.y + border * (row + 1);

            return new Vector2(x, -y);
        }
    }
}