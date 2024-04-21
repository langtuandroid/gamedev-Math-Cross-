using UnityEngine;

namespace stickin
{
    public static class MonoBehaviourExtensions
    {
        public static RectTransform RectTransform(this MonoBehaviour mb)
        {
            return mb.transform as RectTransform;
        }
    }
}