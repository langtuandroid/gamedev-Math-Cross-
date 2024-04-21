using stickin;
using UnityEngine;

namespace stickin.mathcross
{
    public class RewardResourceView : MonoBehaviour
    {
        public Vector2Int Index { get; private set; }

        private RewardResourceModule _rewardResourceModule;

        public void Init(Vector2Int index, RewardResourceModule rewardResourceModule)
        {
            _rewardResourceModule = rewardResourceModule;
            
            Index = index;
        }
        
        public void Collect()
        {
            _rewardResourceModule.IncResource(1, transform);
            Destroy(gameObject);
        }
    }
}