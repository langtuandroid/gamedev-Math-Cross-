using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stickin.menus
{
    public class MenusService : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Parents")] 
        [SerializeField] private Transform _menusParent;
        [SerializeField] private Transform _linkMenusParent;

        [Header("Data")] 
        [SerializeField] private MenusData _menusData;
        #endregion

        private static MenusService _instance;
        private Dictionary<Type, BaseMenu> _menusMap = new Dictionary<Type, BaseMenu>();
        private Dictionary<Type, BaseMenu> _linkMenusMap = new Dictionary<Type, BaseMenu>();
        private List<BaseMenu> _menusStack = new List<BaseMenu>();

        public event Action OnInitComplete;
        public event Action<BaseMenu> OnHideMenu;
        public event Action<BaseMenu> OnShowMenu;

        #region Static methods
        public static void Show<T>(Hashtable data = null) where T : BaseMenu
        {
            var prefab = _instance.GetPrefab(typeof(T));
            Show(prefab, data);
        }
        
        public static void Show(BaseMenu menu, Hashtable data = null)
        {
            _instance.ShowWithType(menu, data);
        }
        
        public static void Hide(BaseMenu menu)
        {
            _instance.HideWithType(menu);
        }

        public static BaseMenu GetMenu(string className)
        {
            return _instance.GetPrefab(className);
        }
        
        public static BaseMenu GetLinkMenu(BaseMenu prefab)
        {
            return _instance.InstantiateMenu(prefab.GetType(), prefab, true);
        }

        public static void OnShowMenuRegistr(Action<BaseMenu> action)
        {
            if (_instance != null)
                _instance.OnShowMenu += action;
        }

        public static void OnHideMenuRegistr(Action<BaseMenu> action)
        {
            if (_instance != null)
                _instance.OnHideMenu += action;
        }
        
        public static void OnShowMenuUnregistr(Action<BaseMenu> action)
        {
            if (_instance != null)
                _instance.OnShowMenu -= action;
        }

        public static void OnHideMenuUnregistr(Action<BaseMenu> action)
        {
            if (_instance != null)
                _instance.OnHideMenu -= action;   
        }
        #endregion
        
        private void Awake()
        {
            _instance = this;
        }
        
        private void Start()
        {
            if (_menusData.StartMenu != null)
                Show(_menusData.StartMenu);
            Input.multiTouchEnabled = false;
            OnInitComplete?.Invoke();
        }
        
        private bool IsContains<T>() where T : BaseMenu
        {
            return _menusMap.ContainsKey(typeof(T));
        }

        private void ShowWithType<T>(T type, Hashtable data) where T : BaseMenu
        {
            if (type != null && _menusMap != null)
            {
                var menuPrefab = GetPrefab(type.GetType(), false);

                if (menuPrefab == null)
                {
                    Debug.LogError($"MenusService.ShowWithType: Not find menu prefab for type = {type}");
                    return;
                }

                var menu = InstantiateMenu( menuPrefab.GetType(), menuPrefab);

                List<BaseMenu> allLinkMenus = null;
                if (menu.NeedHideOtherMenus)
                {
                    allLinkMenus = HideMenus();

                    if (menu.LinkMenus != null)
                    {
                        foreach (var linkMenu in menu.LinkMenus)
                        {
                            allLinkMenus.Remove(linkMenu);
                            linkMenu.Show();
                        }
                    }

                    foreach (var linkMenu in allLinkMenus)
                    {
                        linkMenu.Hide();
                    }
                }

                menu.SetData(data);
                menu.Show();
                _menusStack.Add(menu);

                OnShowMenu?.Invoke(menu);
            }
        }

        private void HideWithType<T>(T menuType) where T : BaseMenu
        {
            if (menuType != null)
            {
                var type = menuType.GetType();
                if (_menusMap.ContainsKey(type))
                {
                    var menu = _menusMap[type];
                    if (menu != null)
                    {
                        menu.Hide();
                        OnHideMenu?.Invoke(menu);
                    }
                    else
                        Debug.LogError($"MenusService.HideWithType:  menu is null in _menusMap");
                }
                else
                    Debug.LogError($"MenusService.HideWithType: not find menu in _menusMap");
            }
            else
                Debug.LogError($"MenusService.HideWithType: menuType is null");
        }
        
        private BaseMenu InstantiateMenu(Type t, BaseMenu prefab, bool isLinkMenu = false)
        {
            var map = isLinkMenu ? _linkMenusMap : _menusMap;
            var parent = isLinkMenu ? _linkMenusParent : _menusParent;

            prefab = GetPrefab(prefab.GetType(), isLinkMenu);

            if (map != null && prefab != null && !map.ContainsKey(prefab.GetType()))
            {
                var menu = Instantiate(prefab, parent);
                map[t] = menu;
                menu.Init();
                menu.InitLinkMenus();
                
                return menu;
            }

            if (prefab == null)
                Debug.LogError($"InstantiateMenu: ERROR Prefab is null for type = {t}");
            
            return map[prefab.GetType()];
        }

        private BaseMenu GetPrefab(string className, bool isLinkMenu = false)
        {
            var list = isLinkMenu ? _menusData.LinkMenusPrefabs : _menusData.MenusPrefabs;
            foreach (var prefab in list)
            {
                if (className == prefab.GetType().Name)
                    return prefab;
            }

            return null;
        }
        
        private BaseMenu GetPrefab(Type type, bool isLinkMenu = false)
        {
            return GetPrefab(type.Name, isLinkMenu);
            
            // var list = isLinkMenu ? _menusData.LinkMenusPrefabs : _menusData.MenusPrefabs;
            // foreach (var prefab in list)
            // {
            //     if (type.IsAssignableFrom(prefab.GetType()))
            //         return prefab;
            // }
            //
            // return null;
        }
        
        private List<BaseMenu> HideMenus()
        {
            var result = new List<BaseMenu>();
            foreach (var menu in _menusMap)
            {
                var linkMenus = menu.Value.LinkMenus;
                if (linkMenus != null)
                {
                    foreach (var linkMenu in linkMenus)
                    {
                        if (!result.Contains(linkMenu))
                        {
                            result.Add(linkMenu);
                        }
                    }
                }

                Hide(menu.Value);
                menu.Value.Hide();
            }

            return result;
        }
        
// #if UNITY_EDITOR || UNITY_ANDROID
//         private void Update()
//         {
//             if (Input.GetKeyDown(KeyCode.Escape) && _currentShowMenu != null)
//             {
//                 _currentShowMenu.ClickBackButton();
//             }
//         }
// #endif

    }
}