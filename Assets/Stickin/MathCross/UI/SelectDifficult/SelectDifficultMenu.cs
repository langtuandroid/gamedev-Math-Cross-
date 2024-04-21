using System;
using System.Collections;
using stickin.menus;
using UnityEngine;

namespace stickin.mathcross
{
    public class SelectDifficultMenu : BaseMenu
    {
        [SerializeField] private Transform _popup;

        private Action<LevelDifficult> _selectDifficultCallback;
        
        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            var position = (Vector3) data["position"];
            _selectDifficultCallback = (Action<LevelDifficult>)data["callback"];
            _popup.position = position;
        }
        
        public void OnClickDifficultBtn(LevelDifficult difficult)
        {
            _selectDifficultCallback?.Invoke(difficult);
        }
    }
}
