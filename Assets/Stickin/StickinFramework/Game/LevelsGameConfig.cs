using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class LevelsGameConfig : GameConfig
    {
        [Header("Levels")] 
        public List<OrderAssetConfig> OrdersAssets;
        public TextAsset LevelsTextAsset;
        
        public virtual LevelsModel<T> GetLevelsModels<T>()
        {
            return JsonUtility.FromJson<LevelsModel<T>>(LevelsTextAsset.text);
        }
        
        public T GetLevelModel<T>(LevelsModel<T> levelsModel, int index, OrderAssetType orderType)
        {
            var orderAsset = GetOrderAsset(orderType);
            index = orderAsset.GetLevelIndex(index);

            return levelsModel.GetLevelModel(index);
        }
        
        public T GetLevelModel<T>(int index, OrderAssetType orderType)
        {
            var levelsModel = GetLevelsModels<T>();
            return GetLevelModel(levelsModel, index, orderType);
        }

        private OrderAssetConfig GetOrderAsset(OrderAssetType type)
        {
            foreach (var orderAsset in OrdersAssets)
            {
                if (orderAsset.Type == type)
                    return orderAsset;
            }

            return null;
        }
    }
}