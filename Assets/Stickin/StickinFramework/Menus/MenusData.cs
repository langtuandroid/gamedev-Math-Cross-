using UnityEngine;

namespace stickin.menus
{
    [CreateAssetMenu(fileName = "MenusData", menuName = "Stickin/MenusConfig")]
    public class MenusData : ScriptableObject
    {
        [SerializeField] private BaseMenu _startMenu;
        [SerializeField] private BaseMenu[] _menusPrefabs;
        [SerializeField] private BaseMenu[] _linkMenusPrefabs;

        public BaseMenu StartMenu => _startMenu;
        public BaseMenu[] MenusPrefabs => _menusPrefabs;
        public BaseMenu[] LinkMenusPrefabs => _linkMenusPrefabs;

    }
}