using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Text))]
    public class TextTimer : MonoBehaviour
    {
        [SerializeField] private bool _withDoubleZero = true;
        
        private Text _txt;
        private GameTimer _timer;
        
        public void Init(GameTimer timer)
        {
            _timer = timer;
            _txt = GetComponent<Text>();

            _timer.AddedCallback(OnChangeTimer);
        }

        private void OnDestroy()
        {
            if (_timer != null)
                _timer.RemoveCallback(OnChangeTimer);
        }

        private void OnChangeTimer(float seconds)
        {
            var str = StringExtensions.SecondsToText((int)seconds, _withDoubleZero);
            _txt.text = str;
        }
    }
}