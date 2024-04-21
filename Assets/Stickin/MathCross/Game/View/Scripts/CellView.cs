using DG.Tweening;
using stickin;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _bgImage;
        [SerializeField] private Text _valueTxt;

        private Cell _model;

        private Tweener _moveTweener;
        private Tweener _scaleTweener;
        private Touchable _touchable;

        public Cell Model => _model;

        public void Init(Cell model)
        {
            _model = model;
            _model.OnChange += OnChange;

            _valueTxt.text = model.ValueString();

            Refresh();

            if (!model.IsLocked)
            {
                _touchable = gameObject.AddComponent<Touchable>();
                _touchable.OnBegan += OnBeganTouch;
            }
        }

        private void OnDestroy()
        {
            if (_touchable != null)
                _touchable.OnBegan -= OnBeganTouch;

            if (_model != null)
                _model.OnChange -= OnChange;
        }

        private void OnChange()
        {
            Refresh();
        }

        private void Refresh()
        {
            // if (_model.IsCorrect)
            //     UnlockedCorrect();
            // else
            //     UnlockedUncorrect();
            //
            // return;
            
            if (_model.IsLocked)
            {
                if (_model.IsCorrect)
                    LockedCorrect();
                else
                    LockedUncorrect();
            }
            else
            {
                if (_model.IsCorrect)
                    UnlockedCorrect();
                else
                    UnlockedUncorrect();
            }
        }

        private void LockedCorrect()
        {
            _bgImage.color = Color.green;
            _valueTxt.color = Color.black;
        }

        private void LockedUncorrect()
        {
            _bgImage.color = Color.yellow;
            _valueTxt.color = Color.black;
        }

        private void UnlockedCorrect()
        {
            _bgImage.color = Color.green;
            _valueTxt.color = new Color(0, 0.5f, 0);
        }

        private void UnlockedUncorrect()
        {
            _bgImage.color = Color.red;
            _valueTxt.color = Color.red;
        }

        public void ToLocalPosition(Vector3 position, float duration)
        {
            ResetMoveTweener();

            _moveTweener = transform.DOLocalMove(position, duration);
        }

        public void ToAnchorPosition(Vector2 position, float duration)
        {
            ResetMoveTweener();
            
            _moveTweener = this.RectTransform().DOAnchorPos(position, duration);
        }

        private void ResetMoveTweener()
        {
            if (_moveTweener != null)
            {
                _moveTweener.Kill();
                _moveTweener = null;
            }
        }

        public void ToScale(Vector3 scale, float duration)
        {
            if (_scaleTweener != null)
            {
                _scaleTweener.Kill();
                _scaleTweener = null;
            }

            _scaleTweener = transform.DOScale(scale, duration);
        }
        
        private void OnBeganTouch(Touchable arg1, PointerEventData arg2)
        {
            ResetMoveTweener();
        }
    }
}