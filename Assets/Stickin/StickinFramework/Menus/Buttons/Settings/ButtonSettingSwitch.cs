using UnityEngine;

namespace stickin.menus
{
    public enum ButtonSettingSwitchType
    {
        Music,
        Sound,
        Vibration
    } 
    
    [RequireComponent(typeof(ButtonSwitchOnOff))]
    public class ButtonSettingSwitch : MonoBehaviour
    {
        [SerializeField] private ButtonSettingSwitchType _type;

        [InjectField] private SoundsAndVibroService _soundsAndVibroService;

        private ButtonSwitchOnOff _buttonSwitch;
        
        private void Awake()
        {
            InjectService.BindFields(this);
            
            var value = GetValue();
            
            _buttonSwitch = GetComponent<ButtonSwitchOnOff>();
            _buttonSwitch.Init(OnClick, value);
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void OnClick()
        {
            var value = GetValue();
            SetValue(!value);
            Refresh();
        }

        private void Refresh()
        {
            _buttonSwitch.SetIsOn(GetValue());
        }

        private bool GetValue()
        {
            var result = true;
            
            switch (_type)
            {
                case ButtonSettingSwitchType.Sound:
                    result = _soundsAndVibroService.IsSoundEnabled;
                    break;
                
                case ButtonSettingSwitchType.Music:
                    result = _soundsAndVibroService.IsMusicEnabled;
                    break;
                
                case ButtonSettingSwitchType.Vibration:
                    result = _soundsAndVibroService.IsVibrationEnabled;
                    break;
            }

            return result;
        }

        private void SetValue(bool value)
        {
            switch (_type)
            {
                case ButtonSettingSwitchType.Sound:
                    _soundsAndVibroService.SetSoundEnabled(value);
                    break;

                case ButtonSettingSwitchType.Music:
                    _soundsAndVibroService.SetMusicEnabled(value);
                    break;

                case ButtonSettingSwitchType.Vibration:
                    _soundsAndVibroService.SetVibrationEnabled(value);
                    break;
            }
        }
    }
}