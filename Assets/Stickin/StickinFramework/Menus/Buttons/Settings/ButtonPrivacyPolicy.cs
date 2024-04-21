using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonPrivacyPolicy : MonoBehaviour
    {
        [SerializeField] private string _privacyPolicyUrl;
       
        private void Start()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (!string.IsNullOrEmpty(_privacyPolicyUrl))
                Application.OpenURL(_privacyPolicyUrl);
            else
                Debug.LogError("PrivacyPolicy URL is empty");
        }
    }
}