using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonShowHideObjects : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _allGO;
        [SerializeField] private List<GameObject> _ifTrueGO;
        [SerializeField] private List<GameObject> ifFalseGO;
        [SerializeField] private bool _isActiveOnStart;

        private bool _currentIsActive;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
            // SetActive(_isActiveOnStart);
        }

        private void OnEnable()
        {
            // SetActive(_isActiveOnStart);
        }

        public void SetActive(bool value)
        {
            _currentIsActive = value;

            foreach (var go in _allGO)
                go.SetActive(false);

            var list = _currentIsActive ? _ifTrueGO : ifFalseGO;
            foreach (var go in list)
                go.SetActive(true);
        }

        private void OnClick()
        {
            SetActive(!_currentIsActive);
        }
    }
}