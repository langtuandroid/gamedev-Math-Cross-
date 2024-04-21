using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public static class TransformExtensions
    {
        public static Rect GetWorldRect(this RectTransform tr)
        {
            var corners = new Vector3[4];
            tr.GetWorldCorners(corners);

            // 0 - Bottom Left
            // 1 – Top Left
            // 2 – Top Right
            // 3 – Bottom Right
            for (var i = 0; i < 4; i++)
            {
                
            }

            return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        }
        
        // public static void Remove<T>(this List<T> ts, List<T> removed) {
        //     foreach (var r in removed) {
        //         ts.Remove(r);
        //     }
        // }

        public static bool PositionInRt(this RectTransform rt, Vector2 position)
        {
            var rect = new Rect(
                -rt.rect.width * rt.pivot.x,
                -rt.rect.height * rt.pivot.y,
                rt.rect.width,
                rt.rect.height);

            return rect.Contains(position);
        }

        public static void RemoveChildren(this Transform tr)
        {
            foreach (Transform child in tr)
                GameObject.Destroy(child.gameObject);
        }
        
        public static void RemoveChildrenForEditor(this Transform tr)
        {
            int childs = tr.childCount;
            for (int i = childs - 1; i >= 0; i--)
                GameObject.DestroyImmediate(tr.GetChild(i).gameObject, true);

        }
    }
}