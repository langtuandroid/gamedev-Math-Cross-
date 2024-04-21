using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    public class HintsView : MonoBehaviour
    {
        [SerializeField] private HintButtonUniversal _hintPrefab;

        private Game _game;
        
        private void Start()
        {
            var hhs = GetComponentsInChildren<HorizontalLayoutGroup>();

            foreach (var hh in hhs)
                LayoutRebuilder.ForceRebuildLayoutImmediate(hh.transform as RectTransform);
        }

        public void Init(Game game, List<HintSO> hints)
        {
            _game = game;
            
            foreach (var hintSo in hints)
            {
                var btn = Instantiate(_hintPrefab, transform);
                btn.Init(hintSo, game);
            }
        }
    }
}