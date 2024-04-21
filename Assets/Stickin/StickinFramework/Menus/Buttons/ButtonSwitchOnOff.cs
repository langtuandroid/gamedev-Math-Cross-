using System;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonSwitchOnOff : MonoBehaviour
    {
        [SerializeField] private GameObject _onGO;
        [SerializeField] private GameObject _offGO;
        
        private Action _clickCallback;
        private bool _isOn;

        public bool IsOn => _isOn;

        public void Init(Action clickCallback, bool isOn)
        {
            _clickCallback = clickCallback;
            SetIsOn(isOn);
        }
        
        public void SetIsOn(bool isOn)
        {
            _isOn = isOn;
            
            _onGO.SetActive(isOn);
            _offGO.SetActive(!isOn);
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _clickCallback?.Invoke();
        }
    }
}