using System;
using System.Collections.Generic;
using DG.Tweening;
using stickin;
using UnityEngine;
using UnityEngine.EventSystems;

namespace stickin.mathcross
{
    public class TouchableController : TouchMonoBehaviour
    {
        [SerializeField] private Transform _touchableParent;
        
        private List<Touchable> _touchables = new ();

        public event Action<Touchable, Vector2> OnStartTouch; 
        public event Action<Touchable> OnEndTouch;

        private Touchable _currentTouchable;
        private Vector3 _needPosition;
        
        public void Init(Vector3 scale)
        {
            _touchableParent.transform.localScale = scale;
        }
        
        public void RegistrTouchable(Touchable touchable)
        {
            if (touchable != null)
            {
                _touchables.Add(touchable);

                touchable.OnBegan += OnBegan;
                touchable.OnMoved += OnMoved;
                touchable.OnEnded += OnEnded;
                touchable.OnExit += OnExit;
                touchable.OnCancel += OnCancel;
            }
        }

        public void UnregistrTouchable(Touchable touchable, bool withRemove)
        {
            if (touchable != null)
            {
                if (withRemove)
                    _touchables.Remove(touchable);

                touchable.OnBegan -= OnBegan;
                touchable.OnMoved -= OnMoved;
                touchable.OnEnded -= OnEnded;
                touchable.OnExit -= OnExit;
                touchable.OnCancel -= OnCancel;
            }
        }

        private void OnDestroy()
        {
            if (_touchables != null)
            {
                foreach (var touchable in _touchables)
                    UnregistrTouchable(touchable, false);

                _touchables.Clear();
            }
        }

        private void OnBegan(Touchable touchable, PointerEventData eventData)
        {
            touchable.transform.SetParent(_touchableParent);
            touchable.transform.DOScale(1f, 0.3f);

            var rt = touchable.RectTransform();
            AddedElementPos(rt);
            
            _currentTouchable = touchable;
            _needPosition = touchable.transform.localPosition;
                
            OnStartTouch?.Invoke(touchable, eventData.position);
        }
        
        private void OnMoved(Touchable touchable, PointerEventData eventData)
        {
            var pos = ConvertPosToTransform(eventData.position, _touchableParent);
            // touchable.transform.localPosition = pos + _prevAddedPos;
            _needPosition = pos;
        }
        
        private void OnEnded(Touchable touchable, PointerEventData eventData)
        {
            _currentTouchable = null;
            
            OnEndTouch?.Invoke(touchable);
        }
        
        private void OnExit(Touchable touchable, PointerEventData eventData)
        {
        }
        
        private void OnCancel(Touchable touchable, BaseEventData eventData)
        {
        }

        private void Update()
        {
            if (_currentTouchable)
            {
                var newPos = _needPosition + _prevAddedPos;
                _currentTouchable.transform.localPosition =
                    Vector3.MoveTowards(_currentTouchable.transform.localPosition, newPos, Time.deltaTime * 10000);
            }
        }

        #region Shift position
        [SerializeField] private Vector2 _shiftPosition = Vector2.zero;
        
        private Tweener _addedPosTweener;
        private Vector3 _prevAddedPos;
        private const float ANIMATION_DURATION = 0.2f;
        
        private void AddedElementPos(RectTransform elementRt)
        {
            ClearAddedTweener();
            _prevAddedPos = Vector3.zero;

            if (_shiftPosition != Vector2.zero)
            {
                var pos = new Vector3(_shiftPosition.x, elementRt.rect.height / 2 + _shiftPosition.y, 0);

                _addedPosTweener = DOTween.To(() => _prevAddedPos,
                    x =>
                    {
                        // elementRt.anchoredPosition += (x - _prevAddedPos);
                        _prevAddedPos = x;
                    },
                    pos, ANIMATION_DURATION);

                _addedPosTweener.onComplete = ClearAddedTweener;
            }
            // DOTween.To(()=> myVector, x=> myVector = x, new Vector3(3,4,8), 1);
        }
        
        private void ClearAddedTweener()
        {
            if (_addedPosTweener != null)
            {
                _addedPosTweener.Kill();
                _addedPosTweener = null;
            }
        }
        #endregion
    }
}