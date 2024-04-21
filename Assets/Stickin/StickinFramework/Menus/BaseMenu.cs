using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class BaseMenu : MonoBehaviour, ITick
    {
        #region Serialized Fields
        [SerializeField] private bool _needHideOtherMenus = true;
        [SerializeField] private List<BaseMenu> _linkMenusPrefabs;
        #endregion

        private Animator _animator;
        private List<BaseMenu> _linkMenus;

        public bool IsShow { get; private set; }
        public bool NeedHideOtherMenus => _needHideOtherMenus;
        public List<BaseMenu> LinkMenus => _linkMenus;
        public List<BaseMenu> LinkMenusPrefabs => _linkMenusPrefabs;

        #region Public Methods
        public void Init()
        {
            IsShow = false;
            gameObject.SetActive(false);

            if (_linkMenus != null)
            {
                foreach (var linkMenu in _linkMenus)
                {
                    linkMenu.Init();
                }
            }
        }

        public virtual void SetData(Hashtable data = null)
        {

        }

        public void Show()
        {
            if (!IsShow)
            {
                IsShow = true;
                gameObject.SetActive(true);
                (transform as RectTransform).SetAsLastSibling();

                ShowStart();

                if (_animator != null)
                {
                    Updater.Instance.Add(this);
                    _animator.enabled = true;
                    ResetTriggers();
                    _animator.SetTrigger("show");
                }
                else
                {
                    ShowComplete();
                }
            }
        }

        public void Hide()
        {
            if (Updater.Instance == null)
                Debug.LogError("Updater is NULL");
            
            if (IsShow)
            {
                IsShow = false;
                HideStart();

                if (_animator != null)
                {
                    Updater.Instance.Add(this);
                    _animator.enabled = true;
                    ResetTriggers();
                    _animator.SetTrigger("hide");

                }
                else
                {
                    gameObject.SetActive(false);
                    Updater.Instance.Remove(this);
                    HideComplete();
                }
            }
        }
        #endregion

        #region Protected Methods
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void ShowStart() { }
        protected virtual void HideStart() { }
        protected virtual void ShowComplete() { }
        protected virtual void HideComplete() { }
        protected virtual void ClickBackButton() { }

        protected void AddedLinkMenu(BaseMenu prefab)// where T : BaseMenu
        {
            // AddedLinkMenu<T>();
            if (_linkMenus == null)
                _linkMenus = new List<BaseMenu>();
            
            var menu = MenusService.GetLinkMenu(prefab);
            _linkMenus.Add(menu);
        }
        
        // protected void AddedLinkMenu<T>() where T : BaseMenu
        // {
        //     if (_linkMenus == null)
        //         _linkMenus = new List<BaseMenu>();
        //
        //     var menu = MenusService.GetLinkMenu<T>();
        //     _linkMenus.Add(menu);
        // }

        protected void AddedListenerToButton(Button btn, UnityAction action)
        {
            if (btn != null)
                btn.onClick.AddListener(action);
        }


        public virtual void InitLinkMenus()
        {
            foreach (var linkMenuPrefab in _linkMenusPrefabs)
                AddedLinkMenu(linkMenuPrefab);
        }

        #endregion

        #region Private Methods

        private void ResetTriggers()
        {
            _animator.ResetTrigger("show");
            _animator.ResetTrigger("hide");
        }

        #endregion

        // public static T ShowGG<T>(Hashtable data = null) where T : BaseMenu
        // {
        //     MenusController.Instance.Show<T>(data);
        // }

        public void Tick()
        {
            if (_animator != null && _animator.enabled)
            {
                var state = _animator.GetCurrentAnimatorStateInfo(0);

                if (IsShow && state.IsName("show"))
                {
                    if (state.normalizedTime >= 1f)
                    {
                        Debug.Log("BaseMenu: end show " + name);
                        _animator.enabled = false;
                        Updater.Instance.Remove(this);
                        ShowComplete();
                    }
                }
                else if (!IsShow && state.IsName("hide"))
                {
                    if (state.normalizedTime >= 1f)
                    {
                        Debug.Log("BaseMenu: end hide " + name);
                        gameObject.SetActive(false);
                        Updater.Instance.Remove(this);
                        HideComplete();
                    }
                }
            }
        }

        public void TickSecond()
        {
        }
    }
}