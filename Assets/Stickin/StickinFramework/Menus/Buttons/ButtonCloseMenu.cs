using stickin.menus;
using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(Button))]
    public class ButtonCloseMenu : MonoBehaviour
    {
        [SerializeField] private BaseMenu _nextMenu;
        [Header("or")]
        [SerializeField] private string _nextMenuStr;

        private BaseMenu _currentMenu;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
            _currentMenu = GetComponentInParent<BaseMenu>();
        }

        private void OnClick()
        {
            if (_nextMenu ==  null)
                _nextMenu = MenusService.GetMenu(_nextMenuStr);
            
            if (_nextMenu != null)
                MenusService.Show(_nextMenu);
            else
                MenusService.Hide(_currentMenu);
        }
    }
}