using stickin.menus;
using UnityEngine;

namespace stickin.mathcross
{
    public class SpinMenu : BaseMenu
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Spin _spin;

        private void Start()
        {
            _spin.OnStart += OnSpinStart;
            _spin.OnEnd += OnSpinEnd;
        }

        private void OnDestroy()
        {
            _spin.OnStart -= OnSpinStart;
            _spin.OnEnd -= OnSpinEnd;
        }

        private void OnSpinStart() => _canvasGroup.interactable = false;
        private void OnSpinEnd() => _canvasGroup.interactable = true;
    }
}
