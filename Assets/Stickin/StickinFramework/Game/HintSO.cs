using stickin;
using UnityEngine;

namespace stickin
{
    [CreateAssetMenu(fileName = "Hint", menuName = "Stickin/HintConfig")]
    public class HintSO : ScriptableObject
    {
        public string ResourceId;
        public Sprite Icon;
        public int Price;
        public int CountInOneGame = 0;
        public string LogicClass;
        public HintPriceType PriceType = HintPriceType.Ad;
        
        public RewardResourcesConfig RewardResourcesConfig;
        
        [InjectField] private ResourcesService _resourcesService;

        public bool TryBuy(Transform transform = null)
        {
            InjectService.BindFields(this);
            
            var coins = _resourcesService.GetResourceValueInt("coin");

            if (coins >= Price && RewardResourcesConfig != null)
            {
                RewardResourcesConfig.Collected(transform);
                _resourcesService.ChangeResource("coin", -Price);
            
                return true;
            }

            return false;
        }

    }
}