using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace stickin
{
    public enum TouchType
    {
        POINTER_UP,
        END_DRAG
    }

    public class TouchMonoBehaviour : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, ICancelHandler,
        IBeginDragHandler, IEndDragHandler, IPointerExitHandler
    {
        private const int INVALID_POINTER_ID = -1;
        
        protected TouchType _touchType = TouchType.POINTER_UP;

        protected Camera Camera => _isInitCamera ? _camera : GetCamera();
        private bool _isInitCamera;
        protected Camera _camera;
        private int _touchPointerId = INVALID_POINTER_ID;
        private bool _interactable = true;

        protected Canvas ParentCanvas => _parentCanvas != null ? _parentCanvas : GetParentCanvas();
        private Canvas _parentCanvas;

        public event Action<PointerEventData> OnTouchedBeganEvent;
        public event Action<PointerEventData> OnTouchedMovedEvent;
        public event Action<PointerEventData> OnTouchedEndedEvent;
        public event Action<PointerEventData> OnTouchedExitEvent;
        public event Action<BaseEventData> OnTouchedCancelEvent;

        #region Events

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            
            // Debug.LogError("OnPointerDown 1 _touchPointerId = " + _touchPointerId);
            if (_touchPointerId != INVALID_POINTER_ID)
                return;
            
            // Debug.LogError("OnPointerDown 2");
            _touchPointerId = eventData.pointerId;
            OnTouchedBegan(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            
            if (_touchPointerId != eventData.pointerId)
                return;
            
            OnTouchedMoved(eventData);
        }

        protected virtual void OnDisable()
        {
            _touchPointerId = INVALID_POINTER_ID;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            
            // Debug.LogError("OnPointerUp 1    _touchPointerId = " + _touchPointerId + "      eventData.pointerId = " + eventData.pointerId);
            if (_touchPointerId != eventData.pointerId)
                return;

            // Debug.LogError("OnPointerUp 2 _touchType = " + _touchType);
            if (_touchType == TouchType.POINTER_UP)
                OnTouchedEnded(eventData); // это нужно по любому, потому что можем поднять палец не сделав свайпа

            // Debug.LogError("RESET _touchPointerId");
            _touchPointerId = INVALID_POINTER_ID;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            
            OnTouchedExit(eventData);
        }

        public void OnCancel(BaseEventData eventData)
        {
            if (!_interactable)
                return;
            
            OnTouchedCancel(eventData);
        }

        #endregion

        protected Camera GetCamera()
        {
            _isInitCamera = true;
            
            if (ParentCanvas != null && ParentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
                _camera = ParentCanvas.worldCamera;

            return _camera;
        }
        private Canvas GetParentCanvas()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            return _parentCanvas;
        }

        #region Protected Methods

        protected void InitCamera(Camera camera)
        {
            _isInitCamera = true;
            _camera = camera;
        }

        protected virtual void OnTouchedBegan(PointerEventData eventData)
        {
            OnTouchedBeganEvent?.Invoke(eventData);
        }

        protected virtual void OnTouchedMoved(PointerEventData eventData)
        {
            OnTouchedMovedEvent?.Invoke(eventData);
        }

        protected virtual void OnTouchedEnded(PointerEventData eventData)
        {
            OnTouchedEndedEvent?.Invoke(eventData);
        }
        
        protected virtual void OnTouchedCancel(BaseEventData eventData)
        {
            OnTouchedCancelEvent?.Invoke(eventData);
        }
        
        protected virtual void OnTouchedExit(PointerEventData eventData)
        {
            OnTouchedExitEvent?.Invoke(eventData);
        }
        
        protected Vector2 ConvertPosToTransform(Vector2 pos, Transform parent = null)
        {
            var result = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent == null ? transform as RectTransform : parent as RectTransform,
                pos,
                Camera,
                out result);

            return result;
        }
        
        protected Vector3 ConvertToWorldPoint(Vector2 pos)
        {
            return Camera.ScreenToWorldPoint(new Vector3(pos.x, 0, pos.y));
        }

        protected Vector2 ConvertTo2DWorldPosition(Vector2 pos)
        {
            return Camera.ScreenToWorldPoint(new Vector2(pos.x, pos.y));
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_touchPointerId != INVALID_POINTER_ID)
                return;
            
            if (_touchType == TouchType.END_DRAG)
                OnTouchedEnded(eventData); // это надо для скролла меню скинов - если первый тач был по кнопке (перехват рейкаста)

            if (_touchPointerId == eventData.pointerId)
            {
                // Debug.LogError("RESET OnEndDrag _touchPointerId");
                _touchPointerId = INVALID_POINTER_ID;
            }
        }

        #endregion

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
        }
    }
}