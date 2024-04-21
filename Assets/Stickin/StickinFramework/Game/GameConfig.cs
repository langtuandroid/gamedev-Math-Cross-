using System;
using System.Collections.Generic;
using stickin;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class OrderAssetConfig
    {
        public OrderAssetType Type;
        public TextAsset Asset;

        [NonSerialized] private List<int> _orderList;
        
        public void Init()
        {
            _orderList = ParseOrder(Asset);
        }
        
        private List<int> ParseOrder(TextAsset textAsset)
        {
            var result = new List<int>();

            if (textAsset != null)
            {
                var array = textAsset.text.Split(',');
                foreach (var el in array)
                    result.Add(int.Parse(el));
            }

            return result;
        }

        public int GetLevelIndex(int index)
        {
            if (_orderList == null || _orderList.Count <= 0)
                Init();
            
            return _orderList.GetElement(index);
        }
    }

    public class GameConfig : ScriptableObject
    {
        [Header("Hints")]
        public List<HintSO> Hints;
    }
}