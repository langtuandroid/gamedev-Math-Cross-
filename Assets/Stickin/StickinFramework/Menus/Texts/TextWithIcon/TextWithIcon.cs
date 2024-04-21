using System;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [ExecuteAlways]
    public class TextWithIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _text;
        [SerializeField] private bool _needCentered;
        [SerializeField] private float _space = 0;
        [SerializeField] private bool _textFirst = true;

        private void Init()
        {
            
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
        
        public void SetText(string str)
        {
            _text.text = str;

            Refresh();
        }

        private void Refresh()
        {
            if (_needCentered)
            {
                _text.rectTransform.sizeDelta = new Vector2(_text.preferredWidth, _text.preferredHeight);

                var w = _icon.rectTransform.sizeDelta.x + _space + _text.rectTransform.sizeDelta.x;
                this.RectTransform().sizeDelta = new Vector2(w, this.RectTransform().sizeDelta.y);

                var iconX = 0f;
                var textX = 0f;

                if (!_textFirst)
                {
                    iconX = _icon.rectTransform.sizeDelta.x / 2 - w / 2;
                    textX = _icon.rectTransform.sizeDelta.x + _space + _text.rectTransform.sizeDelta.x / 2 - w / 2;
                }
                else
                {
                    textX = _text.rectTransform.sizeDelta.x / 2 - w / 2;
                    iconX = _text.rectTransform.sizeDelta.x + _space + _icon.rectTransform.sizeDelta.x / 2 - w / 2;
                }

                _icon.rectTransform.anchoredPosition = new Vector2(iconX, _icon.rectTransform.anchoredPosition.y);
                _text.rectTransform.anchoredPosition = new Vector2(textX, _text.rectTransform.anchoredPosition.y);
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
                Refresh();
        }
    }
}