using UnityEngine;

namespace stickin
{
    [RequireComponent(typeof(HintButtonUniversal))]
    public class OneHintView : MonoBehaviour
    {
        [SerializeField] private HintSO _hintSo;
        
        public void Init(Game game)
        {
            var btn = GetComponent<HintButtonUniversal>();
            btn.Init(_hintSo, game);
        }
    }
}