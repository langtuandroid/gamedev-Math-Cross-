using stickin.menus;
using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(Button))]
    public class ButtonOpenMenu : MonoBehaviour
    {
        [SerializeField] private BaseMenu _menu;
        [Header("or")]
        [SerializeField] private string _menuStr;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (_menu == null)
                _menu = MenusService.GetMenu(_menuStr);
            
            MenusService.Show(_menu);
        }
    }
}