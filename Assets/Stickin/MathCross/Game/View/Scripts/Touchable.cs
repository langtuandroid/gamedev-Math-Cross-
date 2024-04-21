using System;
using stickin;
using UnityEngine;
using UnityEngine.EventSystems;

namespace stickin.mathcross
{
    public class Touchable : TouchMonoBehaviour
    {
        [SerializeField] private bool _needCentered = false;
        [SerializeField] private Vector2 _changeSizeAfterTouch = Vector2.zero;

        public event Action<Touchable, PointerEventData> OnBegan;
        public event Action<Touchable, PointerEventData> OnMoved;
        public event Action<Touchable, PointerEventData> OnEnded;
        public event Action<Touchable, PointerEventData> OnExit;
        public event Action<Touchable, BaseEventData> OnCancel;

        protected override void OnTouchedBegan(PointerEventData eventData)
        {
            base.OnTouchedBegan(eventData);
            OnBegan?.Invoke(this, eventData);
        }

        protected override void OnTouchedMoved(PointerEventData eventData)
        {
            base.OnTouchedMoved(eventData);
            OnMoved?.Invoke(this, eventData);
        }

        protected override void OnTouchedEnded(PointerEventData eventData)
        {
            base.OnTouchedEnded(eventData);
            OnEnded?.Invoke(this, eventData);
        }
        
        protected override void OnTouchedExit(PointerEventData eventData)
        {
            base.OnTouchedExit(eventData);
            OnExit?.Invoke(this, eventData);
        }

        protected override void OnTouchedCancel(BaseEventData eventData)
        {
            base.OnTouchedCancel(eventData);
            OnCancel?.Invoke(this, eventData);
        }
    }
}