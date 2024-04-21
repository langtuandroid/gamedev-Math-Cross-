using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonVibration : MonoBehaviour
    {
        [SerializeField] private VibrationType _vibrationType = VibrationType.Selection;

        [InjectField] private SoundsAndVibroService _soundsAndVibroService;
        
        private void Start()
        {
            InjectService.BindFields(this);
            
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _soundsAndVibroService.Vibration(_vibrationType);
        }
    }
}
