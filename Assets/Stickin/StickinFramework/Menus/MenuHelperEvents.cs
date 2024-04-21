using System.Collections;
using UnityEngine;

namespace stickin.menus
{
    public class MenuHelperEvents : MonoBehaviour
    {
        [SerializeField] private BaseMenu _menu;
        [SerializeField] private string _textKey;
        [SerializeField] private string _textValue;

        public void ShowMenu()
        {
            if (_menu != null)
            {
                Hashtable data = null;
                if (!string.IsNullOrEmpty(_textKey) && !string.IsNullOrEmpty(_textValue))
                    data = new Hashtable {[_textKey] = _textValue};
                
                MenusService.Show(_menu, data);
            }
        }

        public void HideMenu()
        {
            MenusService.Hide(_menu);
        }
    }
}