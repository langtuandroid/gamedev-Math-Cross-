using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace stickin.menus
{
    public class FlyAnimationBezier : BaseFlyAnimation
    {
        [SerializeField] private float _showBetweenDelay = 0.1f;
        [SerializeField] private float _animationDuration = 0.5f;

        [Header("Ease")] [SerializeField] private Ease _easeX = Ease.Linear;
        [SerializeField] private Ease _easeY = Ease.Linear;
        [SerializeField] private Ease _easeZ = Ease.InCubic; // cubic

        [Header("Scale")] [SerializeField] private Vector3 _fromScale = Vector3.one;
        [SerializeField] private Vector3 _midScale = Vector3.one;
        [SerializeField] private Vector3 _toScale = Vector3.one;

        public override void Fly(int index, Transform tr, Vector3 fromPos, Vector3 toPos)
        {
            tr.position = fromPos;
            tr.localScale = _fromScale;
            tr.gameObject.SetActive(false);

            StartCoroutine(FlyCoroutine(index, tr, fromPos, toPos));
        }

        private IEnumerator FlyCoroutine(int index, Transform tr, Vector3 fromPos, Vector3 toPos)
        {
            yield return new WaitForSeconds(index * _showBetweenDelay);

            var sub = toPos - fromPos;

            tr.gameObject.SetActive(true);

            var tween = tr.DOBlendableMoveBy(Vector3.up * sub.y, _animationDuration).SetEase(_easeX);
            tr.DOBlendableMoveBy(Vector3.right * sub.x, _animationDuration).SetEase(_easeY);
            tr.DOBlendableMoveBy(Vector3.forward * sub.z, _animationDuration).SetEase(_easeZ);

            var halfDuration = _animationDuration / 2f;
            tr.DOScale(_midScale, halfDuration);
            tr.DOScale(_toScale, halfDuration).SetDelay(halfDuration);

            tween.onComplete = () => Destroy(tr.gameObject);

            yield return null;
        }
    }
}