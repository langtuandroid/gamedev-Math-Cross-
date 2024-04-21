using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private string _id = "click";

        [InjectField] private SoundsAndVibroService _soundsAndVibroService;
        
        private void Start()
        {
            InjectService.BindFields(this);
            
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _soundsAndVibroService.PlaySound(_id);
        }
    }
}
