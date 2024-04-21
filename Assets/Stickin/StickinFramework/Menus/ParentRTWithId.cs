using DG.Tweening;
using UnityEngine;

namespace stickin
{
    public class ParentRTWithId : MonoBehaviour
    {
        [SerializeField] private string _id;

        private void Start()
        {
            var rts = FindObjectsByType<RTWithId>(FindObjectsSortMode.None);
            foreach (var rt in rts)
            {
                if (rt.Id == _id)
                {
                    ReparentRT(rt.RectTransform());
                    break;
                }
            }
        }

        private void ReparentRT(RectTransform rt)
        {
            rt.SetParent(transform);
            var parent = this.RectTransform();
            
            var scale = Mathf.Min(
                parent.rect.width / rt.rect.width,
                parent.rect.height / rt.rect.height);

            rt.DOScale(scale, 0.5f);
            rt.DOAnchorPos(Vector2.zero, 0.5f);
        }
    }
}