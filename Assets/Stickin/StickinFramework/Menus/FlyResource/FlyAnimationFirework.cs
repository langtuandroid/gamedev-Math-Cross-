using UnityEngine;

namespace stickin.menus
{
    public class FlyAnimationFirework : BaseFlyAnimation
    {
        public override void Fly(int index, Transform tr, Vector3 fromPos, Vector3 toPos)
        {
            // var angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            // var direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * Random.Range(100, 200);
            //
            // var delay = 0.3f;
            //
            // if (count == 1)
            // {
            //     direction = Vector2.up * 200;
            //     // scale = 2f;
            // }
            //
            // var duration = 0.5f + i * 0.02f;
            //
            // var rt = view.RectTransform();
            // rt.DOAnchorPos(rt.anchoredPosition + direction, delay).SetUpdate(true);
            // rt.DOScale(_midScale, delay).SetUpdate(true);
            // rt.DOScale(_toScale, duration).SetDelay(delay).SetUpdate(true);
            //
            // //
            // // var tweener = view.transform.DOMove(destination.Transform.position, duration).SetDelay(delay).SetUpdate(true);
            // // {
            // var sub = destination.Transform.position - view.transform.position;
            // var tweener = view.transform.DOBlendableMoveBy(sub, duration).SetDelay(delay).SetUpdate(true);
            // // var tweener = view.transform.DOBlendableMoveBy(Vector3.right * sub.x, duration).SetDelay(delay).SetEase(Ease.InBack).SetUpdate(true);
            // // view.transform.DOBlendableMoveBy(Vector3.up * sub.y, duration).SetDelay(delay).SetEase(Ease.OutBack).SetUpdate(true);
            // // }
            //             
            // // tweener.onPlay = () => { destination.ResourceText.SetValue(resourceValue - changeValue); };
            //
            // // if (i == countPrefabs - 1)
            // tweener.onComplete = () =>
            // {
            //     RefreshCountText(count);
            //     Destroy(view.gameObject);
            //
            //     destination.ResourceText.SetValue(resourceValue);
            // };
        }
    }
}